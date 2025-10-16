using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
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
    public bool LockAttack { set { lockAttack = value; } }

    //크리티컬관련  확정크리티컬 / 확정크리티컬시간
    private bool forceCritical;
    private bool forceCriticalTime;
    private float forceCriticalUntilTime;

   

    private void Start()
    {
        defaultScale = GameManager.instance.Crosshair.rectTransform.localScale;
        unit = GetComponent<Unit>();
    }

    public void SetUnitAttack(UnitRotation _unitRot )
    {
        unitRot = _unitRot;
    }
   
    //근접공격부분 이렇게 모션이긴공격은 모션끝에 자동장전을넣어놓을것
    public void Attack_Melee(Weapon _weapon, WeaponView _weaponView, Animator _anim)
    {

        if (_weapon.IsMelee)
        {
            bool shot = _weapon.Attack(null);
            if (shot)
            {
                _anim.SetTrigger("Attack");

                _weaponView.meleeStart(hitHeadRay, _weapon.GetDamage);
            }
            else
            {
                _weapon.Reload(_anim);
            }
        }

    }
    public void Attack_Single(Weapon _weapon, WeaponView _weaponView, bool _zoom)
    {
        if (lockAttack)
        {
            return;
        }

        bool shot = _weapon.Attack(_weaponView.GetMuzzlePoint);
        if (shot)
        {
            if (_weaponView != null) _weaponView.UnitAttackAnim();

            StartCoroutine(gunHit(hitDealyTime, _weaponView.GunDamage, _zoom));
            isRecoil = true;
            lockAttack = true;
            unitRot.unitRecoil(_weapon.GetRecoilPower, _weapon.GetRecoilRecoverSpeed);
            StartCoroutine(EndSingleRecoil());

            if (_weapon.GetCurrentAmmo == 0)
            {
                _weapon.Reload(_weaponView);
            }
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


    public void Attack_Auto(Weapon _weapon, PlayerInput _input, WeaponView _weaponView)
    {
        if (!isAttackAuto)
        {
            StartCoroutine(attackAuto(_weapon, _input, _weaponView));
        }
    }
    private IEnumerator attackAuto(Weapon _weapon, PlayerInput _input, WeaponView _weaponView)
    {
        isAttackAuto = true;
        isRecoil = true;
        while (_input.ButtonHold[InputAction.Fire])
        {
            bool shot = _weapon.Attack(_weaponView.GetMuzzlePoint);
            if (shot)
            {
                if (_weaponView != null) _weaponView.UnitAttackAnim();
                unitRot.unitRecoil(_weapon.GetRecoilPower, _weapon.GetRecoilRecoverSpeed);
                StartCoroutine(gunHit(hitDealyTime, _weaponView.GunDamage, false));
                yield return new WaitForSeconds(0.1f);
                if (_weapon.GetCurrentAmmo == 0)
                {
                    yield return new WaitForSeconds(0.3f);
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
        isAttackAuto = false;
        unitRot.ResetMouseRecoil();
    }

    public void Attack_ShotGun(Weapon _weapon, WeaponView _weaponView)
    {
        if (lockAttack)
        {
            return;
        }

        bool shot = _weapon.Attack(_weaponView.GetMuzzlePoint);
        if (shot)
        {
            if (_weaponView != null) _weaponView.UnitAttackAnim();


            for (int i = 0; i < 7; i++)
            {
                StartCoroutine(gunHit(hitDealyTime, _weaponView.GunDamage, false));
            }
            isRecoil = true;
            lockAttack = true;
            unitRot.unitRecoil(_weapon.GetRecoilPower, _weapon.GetRecoilRecoverSpeed);
            StartCoroutine(EndSingleRecoil());

            if (_weapon.GetCurrentAmmo == 0)
            {
                _weapon.Reload(_weaponView);
            }
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

            if (Random.value < unit.CurrentStat.criticalChance * 0.01f)
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

        if (Random.value < unit.CurrentStat.criticalChance * 0.01f)
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
    private IEnumerator gunHit(float _hitDealyTime, float _damage, bool _zoom)
    {

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100f, Color.red, 0.1f);

        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);

        RaycastHit hit;
        bool rayCheck;

        if (_zoom)
        {
            rayCheck = Physics.Raycast(ray, out hit, 100, hitRay);
        }
        else
        {
            Vector3 dir = bulletSpread(ray, 3);
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
        _enemy.HitEnemy(_damage, unit.CurrentStat.criticalDamage, _isCritical);
    }

    public void HandleMeleeHits(List<HitInfo> _hits)
    {
        if (_hits == null || !_hits.Any(x => x.enemy != null))
        {
            return;
        }
        bool criticalCheck = _hits.Any(x => x.IsCritical);
        foreach (var hit in _hits)
        {
            if (hit.enemy != null)
            {
                bool ciriticalCheck = forceCriticalCheck(hit.IsCritical);
                hit.enemy.HitEnemy(hit.Damage, unit.CurrentStat.criticalDamage, ciriticalCheck);
            }
        }
        StartCoroutine(hitMuzzle(criticalCheck));
    }

 
}
