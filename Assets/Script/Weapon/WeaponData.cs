using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
  menuName = "Game/WeaponData",
  fileName = "NewWeaponData"
)]
public class WeaponData : ScriptableObject
{
    public eWeaponType WeaponType;
    public string WeaponName;
    public int MaxAmmo;
    public int PullAmmo;
    public float Damage;
    public float Speed;
    public float RecoilPower;
    public bool Automatic;
    public PoolingManager.ePoolingObject PoolingMuzzle;
    public Bullet.BulletType BulletType;
    public GameObject Prefab;
    public Animator Animator;
}
