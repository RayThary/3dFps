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
    private void Awake()
    {
        defaultScale = crosshair.rectTransform.localScale;
    }
    public void SetUnitRot(UnitRotation _unitRot)
    {
        unitRot = _unitRot;
    }
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
            StartCoroutine(gunHit(hitDealyTime, _weapon.GetDamage));
            isRecoil = true;
            unitRot.unitRecoil(_weapon.GetRecoilPower);
            StartCoroutine(EndSingleRecoil());
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
                StartCoroutine(gunHit(hitDealyTime, gun.GetDamage));
                yield return new WaitForSeconds(0.1f);
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

    private IEnumerator gunHit(float _hitDealyTime, float _damage)
    {

        //이초뒤에 피격판정을준다.
        yield return new WaitForSeconds(_hitDealyTime);
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100f, Color.red, 0.1f);

        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, hitRay))
        {
            Enemy enemy = hit.collider.GetComponentInParent<Enemy>();
            Debug.Log($"Hit {hit.collider.gameObject.name}, enabled={hit.collider.enabled}, activeInHierarchy={hit.collider.gameObject.activeInHierarchy}");

            if (enemy != null)
            {
                int iHitLayer = 1 << hit.collider.gameObject.layer;
                bool isCritical = ((hitHeadRay & iHitLayer) != 0) ? true : false;

                //히트머즐 
                StartCoroutine(hitMuzzle(isCritical));

                //적관련 대미지부분
                hitEnemy(enemy, _damage, isCritical);
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
        GameObject bulletHole = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.BulletHole, GameManager.instance.TempParent);
        bulletHole.transform.position = _point + _nomal * 0.02f;
        bulletHole.transform.rotation = Quaternion.LookRotation(-_nomal);
    }


    private void hitEnemy(Enemy _enemy, float _damage, bool _isCritical)
    {
        _enemy.HitEnemy(_damage, _isCritical);
    }

    public void HandleMeleeHits(List<HitInfo> _hits)
    {
        bool criticalCheck = _hits.Any(x => x.IsCritical);
        foreach (var hit in _hits)
        {
            if (hit.enemy != null)
            {
                hit.enemy.HitEnemy(hit.Damage, hit.IsCritical);
            }
        }
        StartCoroutine(hitMuzzle(criticalCheck));
    }
}
