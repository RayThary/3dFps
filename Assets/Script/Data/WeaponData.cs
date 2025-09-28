using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/WeaponData", fileName = "NewWeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Basic Settings")]
    public eWeaponType WeaponType;
    public string WeaponName;
    public float Damage;
    public float Speed;
    public GameObject Prefab;//무기
    public Animator Animator;
    public bool isMelee;
    public int MaxAmmo;
    public int PullAmmo;
    public int weaponMaxLevel;
    public float weaponLevelUpDamage;
    public float RecoilRecoverSpeed;

    [Header("Gun Settings")]
    public float RecoilPower;
    public bool Automatic;
    public PoolingManager.ePoolingObject PoolingMuzzle;
    public Bullet.BulletType BulletType;

    //[Header("Melee Settings")] 근접전용추가할일있으면 넣어줄곳

}
