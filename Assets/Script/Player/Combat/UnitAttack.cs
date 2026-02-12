using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static WeaponView;

public class UnitAttack : MonoBehaviour
{
    [SerializeField] private float hitDuration = 0.2f;
    private Color nomalCrossColor = Color.white;
    private Color criticalCrossColor = Color.red;

    private float hitDealyTime = 0.1f;
    private Vector3 defaultScale;

    [SerializeField] private LayerMask hitRay;
    [SerializeField] private LayerMask hitHeadRay;
    private bool isAttackAuto = false;
    private UnitRotation unitRot;
    private Unit unit;

    private bool isRecoil;
    public bool GetIsRecoil { get { return isRecoil; } }
    private bool lockAttack = false;

    //크리티컬관련  확정크리티컬 / 확정크리티컬시간
    private bool forceCritical;
    private bool forceCriticalTime;
    private float forceCriticalUntilTime;

    private Weapon currentWeapon;
    public void SetCurrentWeapon(Weapon _weapon)
    {
        currentWeapon = _weapon;
    }


    private void Start()
    {
        defaultScale = GameManager.instance.Crosshair.rectTransform.localScale;
        unit = GetComponent<Unit>();
        var weaponChange = unit.UnitAttackModule.GetUnitWeaponChange;

        // 현재 무기 초기 설정
        SetCurrentWeapon(weaponChange.GetCurrentWeapon());

        // 무기 교체될 때마다 자동 갱신
        weaponChange.OnWeaponSwitched += SetCurrentWeapon;

    }

    public void SetUnitAttack(UnitRotation _unitRot)
    {
        unitRot = _unitRot;
    }


    public void Attack_Single(Weapon _weapon, WeaponView _weaponView, bool _zoom, float _SpreadRange)
    {
        if (lockAttack)
        {
            return;
        }

        Transform muzzle = _zoom ? null : _weaponView.GetMuzzlePoint;

        bool shot = _weapon.Attack(muzzle);


        if (shot)
        {
            if (_weaponView != null) _weaponView.UnitAttackSingleAnim();

            StartCoroutine(gunHit(hitDealyTime, _weaponView.GunDamage, _zoom, _SpreadRange));
            isRecoil = true;
            lockAttack = true;
            unitRot.unitRecoil(_weapon.GetRecoilPower, _weapon.GetRecoilRecoverSpeed);
            StartCoroutine(EndSingleRecoil());

            if (_weapon.GetCurrentAmmo == 0)
            {
                _weapon.Reload(_weaponView);
            }
            StartCoroutine(fireCooldown(_weapon.FireCooldown));
        }
        else
        {
            if (_weapon.GetReserveAmmo >= 0)
            {
                _weapon.Reload(_weaponView);
            }
        }

    }
    private IEnumerator EndSingleRecoil()
    {
        yield return null;
        isRecoil = false;
    }


    public void Attack_Auto(Weapon _weapon, PlayerInput _input, WeaponView _weaponView, float _shotDelay, float _SpreadRange)
    {

        if (!isAttackAuto)
        {
            StartCoroutine(attackAuto(_weapon, _input, _weaponView, _shotDelay, _SpreadRange));
        }
    }
    private IEnumerator attackAuto(Weapon _weapon, PlayerInput _input, WeaponView _weaponView, float _shotDelay, float _SpreadRange)
    {
        isAttackAuto = true;
        isRecoil = true;
        if (_weaponView != null) _weaponView.UnitAttackAutoAnim(true);
        while (_input.ButtonHold[InputAction.Fire] && currentWeapon == _weapon)
        {
            if (_input.ButtonUp[InputAction.Fire])
            {
                break;
            }

            bool shot = _weapon.Attack(_weaponView.GetMuzzlePoint);
            if (shot)
            {

                unitRot.unitRecoil(_weapon.GetRecoilPower, _weapon.GetRecoilRecoverSpeed);
                StartCoroutine(gunHit(hitDealyTime, _weaponView.GunDamage, false, _SpreadRange));
                yield return new WaitForSeconds(_shotDelay);
                if (_weapon.GetCurrentAmmo == 0)
                {
                    yield return new WaitForSeconds(_shotDelay);
                    _weapon.Reload(_weaponView);
                    break;
                }
            }
            else
            {
                if (_weapon.GetCurrentAmmo >= 0)
                {
                    _weapon.Reload(_weaponView);
                }
                break;
            }

        }
        isRecoil = false;
        unitRot.ResetMouseRecoil();
        if (_weaponView != null) _weaponView.UnitAttackAutoAnim(false);
        yield return new WaitForSeconds(0.1f);
        isAttackAuto = false;

    }

    public void Attack_ShotGun(Weapon _weapon, WeaponView _weaponView, float _SpreadRange)
    {
        if (lockAttack)
        {
            return;
        }

        bool shot = _weapon.Attack(_weaponView.GetMuzzlePoint);
        if (shot)
        {
            if (_weaponView != null) _weaponView.UnitAttackSingleAnim();


            for (int i = 0; i < 7; i++)
            {
                StartCoroutine(gunHit(hitDealyTime, _weaponView.GunDamage, false, _SpreadRange));
            }
            isRecoil = true;
            lockAttack = true;
            unitRot.unitRecoil(_weapon.GetRecoilPower, _weapon.GetRecoilRecoverSpeed);
            StartCoroutine(EndSingleRecoil());

            if (_weapon.GetCurrentAmmo == 0)
            {
                _weapon.Reload(_weaponView);
            }
            StartCoroutine(fireCooldown(_weapon.FireCooldown));
        }
        else
        {
            if (_weapon.GetReserveAmmo >= 0)
            {
                _weapon.Reload(_weaponView);
            }
        }
    }



    private bool forceCriticalCheck(RaycastHit _hit)
    {
        if (forceCriticalTime)
        {
            if (Time.time >= forceCriticalUntilTime)
            {
                forceCriticalTime = false;
            }
            return true;
        }
        else if (forceCritical)
        {
            forceCritical = false;
            return true;
        }
        else
        {

            if (Random.value < currentWeapon.CriticalChance)
            {
                return true;
            }
            else
            {
                int iHitLayer = 1 << _hit.collider.gameObject.layer;
                return ((hitHeadRay & iHitLayer) != 0) ? true : false;
            }
        }

    }
    private bool forceCriticalCheck(bool _isCritical)
    {
        if (_isCritical)
        {
            return true;
        }

        if (forceCriticalTime)
        {
            if (Time.time >= forceCriticalUntilTime)
            {
                forceCriticalTime = false;
            }
            return true;
        }

        if (forceCritical)
        {
            forceCritical = false;
            return true;
        }

        if (Random.value < currentWeapon.CriticalChance)
        {
            return true;
        }

        return false;
    }
    // 1회용 확정크리티컬 
    public void SetforceCritical()
    {
        forceCritical = true;
    }
    // 지속시간동안 크리티컬이뜨게하는것
    public void SetforceCritical(float _criticalDuration)
    {
        forceCriticalTime = true;
        forceCriticalUntilTime = Time.time + _criticalDuration;
    }

    private Vector3 bulletSpread(Ray _ray, float _maxAngle)
    {
        float angle = Random.Range(0, _maxAngle);
        float azimuth = Random.Range(0f, 360f);
        Vector3 dir = Quaternion.AngleAxis(angle, Random.insideUnitSphere) * _ray.direction;

        return dir;

    }
    private IEnumerator gunHit(float _hitDealyTime, float _damage, bool _zoom, float _SpreadRange)
    {

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100f, Color.red, 0.1f);

        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);

        RaycastHit hit;
        bool rayCheck;

        if (_zoom)
        {
            Vector3 dir;
            if (unit.UnitMovementModule.IsMoving || unit.IsDodge)
            {
                dir = bulletSpread(ray, _SpreadRange * 0.45f);
            }
            else
            {
                dir = bulletSpread(ray, 0.3f);
            }
            rayCheck = Physics.Raycast(ray.origin, dir, out hit, 100f, hitRay);
        }
        else
        {
            Vector3 dir = bulletSpread(ray, _SpreadRange);
            rayCheck = Physics.Raycast(ray.origin, dir, out hit, 100f, hitRay);
        }

        if (rayCheck)
        {
            Enemy enemy = hit.collider.GetComponentInParent<Enemy>();

            if (enemy != null)
            {

                //적관련 대미지부분
                bool isCritical = forceCriticalCheck(hit);

                hitEnemy(enemy, _damage, isCritical);

                //이초뒤에 피격판정을준다.
                yield return new WaitForSeconds(_hitDealyTime);
                //히트머즐 
                StartCoroutine(hitMuzzle(isCritical));
            }
            else
            {
                //총이환경피격시나오는부분
                spawnBulletHole(hit.point, hit.normal);
            }

        }

    }

    private IEnumerator fireCooldown(float _cooldown)
    {
        yield return new WaitForSeconds(_cooldown);
        lockAttack = false;
    }
    private IEnumerator hitMuzzle(bool _criticalHit)
    {
        if (_criticalHit)
            GameManager.instance.Crosshair.color = criticalCrossColor;

        GameManager.instance.Crosshair.rectTransform.localScale = defaultScale * 1.2f;
        GameManager.instance.HitCrosshair.gameObject.SetActive(true);

        yield return new WaitForSeconds(hitDuration);

        if (_criticalHit)
            GameManager.instance.Crosshair.color = nomalCrossColor;
        GameManager.instance.HitCrosshair.gameObject.SetActive(false);

        GameManager.instance.Crosshair.rectTransform.localScale = defaultScale;
    }

    private void spawnBulletHole(Vector3 _point, Vector3 _nomal)
    {
        GameObject bulletHole = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.BulletHole, GameManager.instance.PoolingParents["BulletHole"]);
        bulletHole.transform.position = _point + _nomal * 0.02f;
        bulletHole.transform.rotation = Quaternion.LookRotation(-_nomal);
    }


    private void hitEnemy(Enemy _enemy, float _damage, bool _isCritical)
    {
        _enemy.HitEnemy(_damage, currentWeapon.CriticalDamage, _isCritical);
    }



}
