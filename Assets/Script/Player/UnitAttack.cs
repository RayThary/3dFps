using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitAttack : MonoBehaviour
{
    //���ؼ�
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

    public void Attack(Weapon gun, WeaponView _weaponView, Transform _unitHead)
    {
        bool shot = gun.Attack(_weaponView.GetMuzzlePoint);
        if (shot)
        {
            if (_weaponView != null) _weaponView.UnitAttackAnim();
            StartCoroutine(gunHit(hitDealyTime, gun.GetDamage));
            isRecoil = true;
            unitRot.unitRecoil(gun.GetRecoilPower);
            StartCoroutine(EndSingleRecoil());
        }
        else
        {
            if (gun.GetReserveAmmo >= 0)
            {
                gun.Reload(_weaponView);
            }
        }
    }
    private IEnumerator EndSingleRecoil()
    {
        yield return null;
        isRecoil = false;
    }


    public void Attack(Weapon gun, PlayerInput _input, WeaponView _weaponView, Transform _unitHead)
    {
        if (!isAttackAuto)
        {
            StartCoroutine(attackAuto(gun, _input, _weaponView, _unitHead));

        }
    }
    private IEnumerator attackAuto(Weapon gun, PlayerInput _input, WeaponView _weaponView, Transform _unitHead)
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

        //���ʵڿ� �ǰ��������ش�.
        yield return new WaitForSeconds(_hitDealyTime);

        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, hitRay))
        {
            Enemy enemy = hit.collider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                int iHitLayer = 1 << hit.collider.gameObject.layer;
                bool isCritical = ((hitHeadRay & iHitLayer) != 0) ? true : false;

                //��Ʈ���� 
                StartCoroutine(hitMuzzle(hit.point, isCritical));

                //������ ������κ�
                hitEnemy(enemy, _damage, isCritical);
            }
            else
            {
                //����ȯ���ǰݽó����ºκ�
                spawnBulletHole(hit.point, hit.normal);
            }

        }

    }
    private IEnumerator hitMuzzle(Vector3 _hitPoint, bool _criticalHit)
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
}
