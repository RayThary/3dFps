using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    //기본정보 
    protected eWeaponType weaponType;
    public eWeaponType WeaponType { get { return weaponType; } }

    protected string weaponName;
    public string GetName { get { return weaponName; } }

    protected GameObject weaponPrefeb;
    public GameObject WeaponPrefeb { get { return weaponPrefeb; } protected set { weaponPrefeb = value; } }

    //기본스텟 
    protected float damage;
    public float GetDamage { get { return damage; } }

    protected float criticalChance;
    public float CriticalChance { get { return criticalChance; } }

    protected float criticalDamage;
    public float CriticalDamage { get { return criticalDamage; } }

    protected float spreadRange;
    public float SpreadRange { get { return spreadRange; } }

    protected float recoilPower;
    public float GetRecoilPower { get { return recoilPower; } }

    protected float recoilRecoverSpeed;
    public float GetRecoilRecoverSpeed { get { return recoilRecoverSpeed; } }

    //총기설정 
    protected bool automatic;
    public bool Automatic { get { return automatic; } protected set { automatic = value; } }

    protected float fireDelay;
    public float FireDelay { get { return fireDelay; } }

    protected float fireCooldown;
    public float FireCooldown { get { return fireCooldown; } }

    protected PoolingManager.ePoolingObject poolingMuzzle;

    protected bool zoomWeapon;
    public bool ZoomWeapon { get { return zoomWeapon; } }

    //탄얀 설정 
    protected int maxAmmo;
    protected int currentAmmo;

    protected int reserveAmmo;
    protected int maxReserveAmmo;

    // UI용
    public int GetCurrentAmmo { get { return currentAmmo; } }
    public int GetReserveAmmo { get { return reserveAmmo; } }

    protected bool isReloading;
    public bool IsReloading { set { isReloading = value; } }

    //업그레이드 설정
    protected int weaponMaxLevel;
    public int WeaponMaxLevel { get { return weaponMaxLevel; } }

    // 증가 수치
    protected float damageUp;
    protected float critChanceUp;
    protected float critDamageUp;

    protected float fireRateUp;

    protected int maxAmmoUp;
    protected int ReserveAmmoUp;

    protected float spreadDown;


    public Weapon(WeaponData _data)
    {
        // 기본 정보
        weaponType = _data.WeaponType;
        weaponName = _data.WeaponName;
        weaponPrefeb = _data.Prefab;

        // 기본 스탯
        damage = _data.Damage;
        criticalChance = _data.criticalChance;
        criticalDamage = _data.criticalDamage;

        spreadRange = _data.SpreadRange;
        recoilPower = _data.RecoilPower;
        recoilRecoverSpeed = _data.RecoilRecoverSpeed;

        // 탄약
        currentAmmo = _data.MaxAmmo;
        maxAmmo = _data.MaxAmmo;

        reserveAmmo = _data.ReserveAmmo / 2;
        maxReserveAmmo = _data.ReserveAmmo;

        // 사격 설정
        automatic = _data.Automatic;
        fireDelay = _data.fireDelay;
        fireCooldown = _data.fireCooldown;
        poolingMuzzle = _data.PoolingMuzzle;
        zoomWeapon = _data.ZoomWeapon;

        // 업그레이드
        weaponMaxLevel = _data.weaponMaxLevel;

        damageUp = _data.damageUp;
        critChanceUp = _data.critChanceUp;
        critDamageUp = _data.critDamageUp;

        fireRateUp = _data.fireRateUp;

        maxAmmoUp = _data.MaxAmmoUp;
        ReserveAmmoUp = _data.ReserveAmmo;

        spreadDown = _data.spreadDown;
    }

    public abstract bool Attack(Transform _muzzlePoint);
    public virtual void Reload(WeaponView _weaponView)
    {
        if (currentAmmo == maxAmmo || reserveAmmo <= 0 || isReloading)
        {
            return;
        }
        isReloading = true;
        _weaponView.UnitReloadAnim();
    }

    public virtual void Reload(Animator _anim)
    {
        if (currentAmmo == maxAmmo || reserveAmmo <= 0 || isReloading)
        {
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

    public void GetGunDamage(int _weaponLevel)
    {
        damage = damage + (_weaponLevel * 5);
    }

    public abstract void Zoomable(CinemachineVirtualCamera _vCamera, bool _zoom);

    //드랍용
    public void AddAmmo()
    {
        int addAmount = Mathf.FloorToInt(maxAmmo * 0.66f);

        reserveAmmo = Mathf.Min(reserveAmmo + addAmount, maxReserveAmmo);
    }
    //구매용
    public void BuyAddAmmo()
    {
        int addAmount = Mathf.FloorToInt(maxAmmo * 0.66f);
        addAmount += maxAmmo;
        reserveAmmo = Mathf.Min(reserveAmmo + addAmount, maxReserveAmmo);
    }

    public string Upgrade(WeaponView _view, int _upgradeNum, bool _auto)
    {
        float randFactor = Random.Range(0.8f, 1.2f);

        float tempDamage = _view.GunDamage + (damageUp * randFactor);

        _view.GunDamage = Mathf.RoundToInt(tempDamage);

        switch (_upgradeNum)
        {
            case 0:
                float critAdd = critChanceUp * randFactor;
                criticalChance += critAdd;
                return $"크리티컬 확률 +{critAdd:F1}%";

            case 1:
                float critDmgAdd = critDamageUp * randFactor;
                criticalDamage += critDmgAdd;
                return $"크리티컬 데미지 +{critDmgAdd:F1}%";

            case 2:
                float fireAdd = fireRateUp * randFactor;

                if (_auto)
                {
                    fireDelay -= fireAdd;
                    fireDelay = Mathf.Max(0.05f, fireDelay);
                    return $"연사속도 +{fireAdd:F3}";
                }
                else
                {
                    recoilPower -= fireAdd;
                    recoilPower = Mathf.Max(0.1f, recoilPower);
                    return $"반동 감소 -{fireAdd:F3}";
                }

            case 3:
                float ammoAdd = maxAmmoUp * randFactor;
                maxAmmo += Mathf.RoundToInt(ammoAdd);
                return $"탄창 +{Mathf.RoundToInt(ammoAdd)}";

            case 4:
                float reserveAdd = ReserveAmmoUp * randFactor;
                reserveAmmo += Mathf.RoundToInt(reserveAdd);
                return $"비축 탄약 +{Mathf.RoundToInt(reserveAdd)}";

            case 5:
                float spreadAdd = spreadDown * randFactor;
                spreadRange -= spreadAdd;
                spreadRange = Mathf.Max(0.0f, spreadRange);
                return $"탄퍼짐 감소 -{spreadAdd:F3}";
        }
        return "";
    }



}




