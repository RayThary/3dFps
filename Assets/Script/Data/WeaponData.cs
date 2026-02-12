using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/WeaponData", fileName = "NewWeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("기본 설정")]
    public eWeaponType WeaponType;
    public string WeaponName;
    public GameObject Prefab;//무기

    [Header("기본 스텟")]
    public float Damage;
    public int MaxAmmo;//탄약개수
    public int ReserveAmmo;//탄창개수
    public float RecoilRecoverSpeed;
    public float SpreadRange;
    public float RecoilPower;
    public float criticalChance;
    public float criticalDamage;

    [Header("총기 사격 설정")]
    public PoolingManager.ePoolingObject PoolingMuzzle;
    public bool Automatic;
    public bool ZoomWeapon;
    public float fireDelay;// 오토일경우에만 설정
    public float fireCooldown;

    [Header("업그레이드 설정")]
    public int weaponMaxLevel;

    [Header("업그레이드 증가수치")]
    public float damageUp;           
    public float critChanceUp;       
    public float critDamageUp;       

    public float fireRateUp;          // 연사속도 증가 % 단발은 RecoilRecoverSpeed를 올려줄것
    public int MaxAmmoUp;       
    public int ReserveAmmoUp;   
    public float spreadDown;        // 탄퍼짐 감소량

}
