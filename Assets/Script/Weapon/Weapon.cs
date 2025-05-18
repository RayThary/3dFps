using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    protected PoolingManager.ePoolingObject poolingMuzzle;
    protected Bullet.BulletType bulletType;

    protected string weaponName;
    protected int maxAmmo;
    protected int currentAmmo;
    protected int reserveAmmo;
    protected int maxReserveAmmo;
    protected float speed;
    protected bool isMelee;
    public bool IsMelee { get { return isMelee; } }

    protected float recoilPower;
    public float GetRecoilPower { get { return recoilPower; } }

    protected float damage;
    public float GetDamage { get { return damage; } }

    protected bool isReloading;
    public bool IsReloading { set { isReloading = value; } }

    protected eWeaponType weaponType;
    public eWeaponType WeaponType { get { return weaponType; } }


    //UI 총알
    public int GetCurrentAmmo { get { return currentAmmo; } }
    public int GetReserveAmmo { get { return reserveAmmo; } }

    protected bool automatic;
    public bool Automatic { get { return automatic; } protected set { automatic = value; } }

    protected GameObject weaponPrefeb;
    public GameObject WeaponPrefeb { get { return weaponPrefeb; } protected set { weaponPrefeb = value; } }

    private Animator animator;
    public Animator Animator { get { return animator; } set { animator = value; } }

    //총, 근접 순서
    public Weapon(WeaponData _data)
    {
        weaponType = _data.WeaponType;
        weaponName = _data.WeaponName;
        damage = _data.Damage;
        weaponPrefeb = _data.Prefab;
        isMelee = _data.isMelee;

        currentAmmo = _data.MaxAmmo;
        maxAmmo = _data.MaxAmmo;

        reserveAmmo = _data.PullAmmo / 2;
        maxReserveAmmo = _data.PullAmmo;

        automatic = _data.Automatic;
        bulletType = _data.BulletType;
        poolingMuzzle = _data.PoolingMuzzle;
        recoilPower = _data.RecoilPower;
    }
    public Weapon(WeaponData _data,bool _melee)
    {
        weaponType = _data.WeaponType;
        weaponName = _data.WeaponName;
        damage = _data.Damage;
        weaponPrefeb = _data.Prefab;
        isMelee = _data.isMelee;
        reserveAmmo = _data.PullAmmo / 2;
        maxReserveAmmo = _data.PullAmmo;
        currentAmmo = _data.MaxAmmo;
        maxAmmo = _data.MaxAmmo;
    }

    public abstract bool Attack(Transform _muzzlePoint);

    #region
    //public virtual void Fire(Transform _parent)
    //{
    //    if (currentAmmo > 0)
    //    {
    //        GameObject bullet = PoolingManager.Instance.CreateObject(poolingMuzzle, _parent);

    //        bullet.transform.position = _parent.position;
    //        bullet.transform.SetParent(_parent);

    //        Bullet bulletTemp = bullet.GetComponent<Bullet>();
    //        if (bulletTemp != null)
    //        {

    //            bulletTemp.Setup(damage, speed, bulletType);
    //        }

    //        currentAmmo--;
    //    }
    //    else
    //    {
    //        Debug.Log("총알다씀");
    //    }
    //}
    #endregion
    public virtual void Reload(WeaponView _weaponView)
    {
        if (currentAmmo == maxAmmo || reserveAmmo <= 0 || isReloading)
        {
            Debug.Log("장전중 또는 총알이 꽉차있거나 부족함");
            return;
        }
        isReloading = true;
        _weaponView.UnitReloadAnim();
        Debug.Log("장전 ");
    }

    public virtual void Reload(Animator _anim)
    {
        if (currentAmmo == maxAmmo || reserveAmmo <= 0 || isReloading)
        {
            Debug.Log("장전중 또는 총알이 꽉차있거나 부족함");
            return;
        }
        isReloading = true;
        _anim.SetTrigger("Reload");
    }

    public void ReloadAmmo()
    {
        int needAmmo = maxAmmo - currentAmmo;
        int ammoLoad = Mathf.Min(needAmmo, reserveAmmo);
        currentAmmo += ammoLoad;
        reserveAmmo -= ammoLoad;
    }

}




