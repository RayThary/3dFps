using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGun : Weapon
{
    public HandGun(WeaponData _data) : base(_data)
    {
        //�ڵ�� �߰������Ұ������� �߰�
    }

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

}

