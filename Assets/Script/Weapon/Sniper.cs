using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon
{
    public Sniper(WeaponData _data) : base(_data) { }

    public override bool Attack(Transform _muzzlePoint)
    {
        if (currentAmmo > 0 && !base.isReloading)
        {
            GameObject bullet = PoolingManager.Instance.CreateObject(poolingMuzzle, _muzzlePoint);

            bullet.transform.SetParent(_muzzlePoint);
            bullet.transform.localPosition = Vector3.zero;
            bullet.transform.localRotation = Quaternion.identity;

            currentAmmo--;
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Zoomable(CinemachineVirtualCamera _vCamera, bool _zoom)
    {
        if (_zoom)
        {
            _vCamera.m_Lens.FieldOfView = 30;
            GameManager.instance.ZoomScope.SetActive(true);
        }
        else
        {
            _vCamera.m_Lens.FieldOfView = 60;
            GameManager.instance.ZoomScope.SetActive(false);
        }
    }
}
