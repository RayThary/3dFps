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
    protected float recoilPower;
    public float GetRecoilPower { get { return recoilPower; } }

    protected float damage;
    public float GetDamage { get { return damage; } }
    protected bool isReloading;
    public bool IsReloading { set { isReloading = value; } }


    //UI ÃÑ¾Ë
    public int GetCurrentAmmo { get { return currentAmmo; } }
    public int GetReserveAmmo { get { return reserveAmmo; } }

    protected bool automatic;
    public bool Automatic { get { return automatic; } protected set { automatic = value; } }

    protected GameObject weaponPrefeb;
    public GameObject WeaponPrefeb { get { return weaponPrefeb; } protected set { weaponPrefeb = value; } }

    private Animator animator;
    public Animator Animator { get { return animator; } set { animator = value; } }

    //ÃÑ, ±ÙÁ¢ ¼ø¼­
    public Weapon(WeaponData _data)
    {
        weaponName = _data.WeaponName;
        damage = _data.Damage;
        currentAmmo = _data.MaxAmmo;
        maxAmmo = _data.MaxAmmo;
        speed = _data.Speed;
        automatic = _data.Automatic;
        bulletType = _data.BulletType;
        poolingMuzzle = _data.PoolingMuzzle;
        weaponPrefeb = _data.Prefab;
        reserveAmmo = _data.PullAmmo / 2;
        maxReserveAmmo = _data.PullAmmo;
        recoilPower = _data.RecoilPower;
    }
    public Weapon(string _name, float _damage)
    {
        this.weaponName = _name;
        this.damage = _damage;
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
    //        Debug.Log("ÃÑ¾Ë´Ù¾¸");
    //    }
    //}
    #endregion
    public virtual void Reload(WeaponView _weaponView)
    {
        if (currentAmmo == maxAmmo || reserveAmmo <= 0 || isReloading)
        {
            Debug.Log("ÀåÀüÁß ¶Ç´Â ÃÑ¾ËÀÌ ²ËÂ÷ÀÖ°Å³ª ºÎÁ·ÇÔ");
            return;
        }
        isReloading = true;
        _weaponView.UnitReloadAnim();
        Debug.Log("ÀåÀü ");
    }

    public void ReloadAmmo()
    {
        int needAmmo = maxAmmo - currentAmmo;
        int ammoLoad = Mathf.Min(needAmmo, reserveAmmo);
        currentAmmo += ammoLoad;
        reserveAmmo -= ammoLoad;
    }

}


