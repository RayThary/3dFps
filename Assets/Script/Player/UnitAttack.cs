using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static WeaponView;

public class UnitAttack : MonoBehaviour
{
    //조준선
    [SerializeField] private Image crosshair;
    [SerializeField] private Image hitCrosshair;
    [SerializeField] private float hitDuration = 0.2f;
    private Color nomalCrossColor = Color.white;
    private Color criticalCrossColor = Color.red;

    private float hitDealyTime = 0.1f;
    private Vector3 defaultScale;

    [SerializeField] private LayerMask hitRay;
    [SerializeField] private LayerMask hitHeadRay;
    private bool isAttackAuto = false;
    private UnitRotation unitRot;

    private bool isRecoil;
    public bool GetIsRecoil { get { return isRecoil; } }

    //크리티컬관련  확정크리티컬 / 확정크리티컬시간
    private bool forceCritical;
    private bool forceCriticalTime;
    private float forceCriticalUntilTime;

    private float criticalChance;
    private float criticalDamage;

    private void Awake()
    {
        defaultScale = crosshair.rectTransform.localScale;
    }
    public void SetUnitAttack(UnitRotation _unitRot, float _criticalChance, float _criticalDamage)
    {
        unitRot = _unitRot;
        criticalChance = _criticalChance;
        criticalDamage = _criticalDamage;
    }

    //근접공격부분 이렇게 모션이긴공격은 모션끝에 자동장전을넣어놓을것
    public void Attack(Weapon _weapon, WeaponView _weaponView, Animator _anim)
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
    public void Attack(Weapon _weapon, WeaponView _weaponView)
    {
        bool shot = _weapon.Attack(_weaponView.GetMuzzlePoint);
        if (shot)
        {
            if (_weaponView != null) _weaponView.UnitAttackAnim();
            StartCoroutine(gunHit(hitDealyTime, _weaponView.GunDamage));
            isRecoil = true;
            unitRot.unitRecoil(_weapon.GetRecoilPower);
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


    public void Attack(Weapon gun, PlayerInput _input, WeaponView _weaponView)
    {
        if (!isAttackAuto)
        {
            StartCoroutine(attackAuto(gun, _input, _weaponView));
        }
    }
    private IEnumerator attackAuto(Weapon gun, PlayerInput _input, WeaponView _weaponView)
    {
        isAttackAuto = true;
        isRecoil = true;
        while (_input.GetFireHold)
        {
            bool shot = gun.Attack(_weaponView.GetMuzzlePoint);
            if (shot)
            {
                if (_weaponView != null) _weaponView.UnitAttackAnim();
                unitRot.unitRecoil(gun.GetRecoilPower);
                StartCoroutine(gunHit(hitDealyTime, _weaponView.GunDamage));
                yield return new WaitForSeconds(0.1f);
                if (gun.GetCurrentAmmo == 0)
                {
                    yield return new WaitForSeconds(0.2f);
                    gun.Reload(_weaponView);
                    break;
                }
            }
            else
            {
                if (gun.GetCurrentAmmo >= 0)
                {
                    gun.Reload(_weaponView);
                }
                break;
            }
        }
        isRecoil = false;
        isAttackAuto = false;
        unitRot.ResetMouseRecoil();
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

            if (Random.value < criticalChance * 0.01f)
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

        if (Random.value < criticalChance * 0.01f)
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
    private IEnumerator gunHit(float _hitDealyTime, float _damage)
    {

        //이초뒤에 피격판정을준다.
        yield return new WaitForSeconds(_hitDealyTime);
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100f, Color.red, 0.1f);

        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);

        RaycastHit hit;
        Vector3 dir = bulletSpread(ray, 3);
        //if (Physics.Raycast(ray, out hit, 100, hitRay))
        if (Physics.Raycast(ray.origin, dir, out hit, 100f, hitRay))
        {
            Enemy enemy = hit.collider.GetComponentInParent<Enemy>();

            if (enemy != null)
            {

                //적관련 대미지부분
                bool isCritical = forceCriticalCheck(hit);

                hitEnemy(enemy, _damage, isCritical);

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
            crosshair.color = criticalCrossColor;

        crosshair.rectTransform.localScale = defaultScale * 1.2f;
        hitCrosshair.gameObject.SetActive(true);

        yield return new WaitForSeconds(hitDuration);

        if (_criticalHit)
            crosshair.color = nomalCrossColor;
        hitCrosshair.gameObject.SetActive(false);

        crosshair.rectTransform.localScale = defaultScale;
    }

    private void spawnBulletHole(Vector3 _point, Vector3 _nomal)
    {
        GameObject bulletHole = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.BulletHole, GameManager.instance.PoolingParents["BulletHole"]);
        bulletHole.transform.position = _point + _nomal * 0.02f;
        bulletHole.transform.rotation = Quaternion.LookRotation(-_nomal);
    }


    private void hitEnemy(Enemy _enemy, float _damage, bool _isCritical)
    {
        _enemy.HitEnemy(_damage, criticalDamage, _isCritical);
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
                hit.enemy.HitEnemy(hit.Damage, criticalDamage, ciriticalCheck);
            }
        }
        StartCoroutine(hitMuzzle(criticalCheck));
    }

    public void SetUnitCritical(float _criticalChance, float _criticalDamage)
    {
        criticalChance = _criticalChance;
        criticalDamage = _criticalDamage;
    }
}
