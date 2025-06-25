using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class WeaponView : MonoBehaviour
{
    [SerializeField]
    private eWeaponType weaponType;
    public eWeaponType WeaponType { get { return weaponType; } }
    public enum WeaponCategory
    {
        Ranged,
        Melee,
    }
    [SerializeField] private WeaponCategory weaponCategory;
    //무기에넣어둘것
    private Animator animator;
    private Weapon weapon;
    private BoxCollider box;


    private Transform weaponPickup;
    public Transform WeaponPickup { get { return weaponPickup; } }

    private Transform muzzlePoint;
    public Transform GetMuzzlePoint { get { return muzzlePoint; } }

    private Transform meshObject;
    public Transform MeshObject { get { return meshObject; } }

    public event Action<List<HitInfo>> OnMeleeHit;

    [SerializeField] private float weaponLevel = 1;
    public float WeaponLevel { get { return weaponLevel; } set { weaponLevel = value; } }
    private int weaponMaxLevel;
    private float weaponLevelUpDamage;
    private float gunDamage;
    public float GunDamage { get { return gunDamage; } }

    private LayerMask headMask;
    private List<HitInfo> hitList = new List<HitInfo>();
    private float meleeDamage;


    public float GetWeaponLevel()
    {
        float addDamage = weaponLevel * 0.5f;
        return addDamage;
    }

    public struct HitInfo
    {
        public Enemy enemy;
        public bool IsCritical;
        public float Damage;

        public HitInfo(Enemy _enemy, bool _isCritical, float _damage)
        {
            enemy = _enemy;
            IsCritical = _isCritical;
            Damage = _damage;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (weaponCategory == WeaponCategory.Melee)
        {
            Enemy enemy = other.GetComponentInParent<Enemy>();

            if (enemy == null)
                return;

            if (hitList.Exists(x => x.enemy == enemy))
                return;

            int layerMask = 1 << other.gameObject.layer;
            bool crit = (headMask & layerMask) != 0;
            hitList.Add(new HitInfo(enemy, crit, meleeDamage));

        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        meshObject = transform.GetChild(0);
        muzzlePoint = meshObject.Find("MuzzlePoint");

        if (weaponPickup == null)
        {
            weaponPickup = transform.Find("Mesh Object/WeaponPickup");
        }

        if (weaponCategory == WeaponCategory.Melee)
        {
            box = GetComponent<BoxCollider>();
            box.enabled = false;
        }

    }


    public void Initialize(Weapon _weapon)
    {
        this.weapon = _weapon;

        if (weaponPickup == null)
        {
            weaponPickup = transform.Find("Mesh Object/WeaponPickup");
        }
        weaponPickup.GetComponent<BoxCollider>().enabled = false;
        gunDamage = _weapon.GetDamage;
        weaponMaxLevel = _weapon.WeaponMaxLevel;
        weaponLevelUpDamage = _weapon.WeaponLevelUpDamage;
    }

    public bool WeaponUpgrade()
    {
        if (weaponLevel >= weaponMaxLevel)
        {
            return false;
        }
        gunDamage *= (1 + weaponLevelUpDamage);
        weaponLevel++;
        return true;
    }

    public void UnitAttackAnim()
    {
        if (animator != null)
        {
            //모든공격모션은 Attack으로바꿔줄필요가있음
            animator.SetTrigger("Attack");
        }
    }
    public void UnitReloadAnim()
    {
        if (animator != null)
        {
            animator.SetTrigger("Reload");
        }
    }
    public void meleeStart(LayerMask _headMask, float _damage)
    {
        hitList.Clear();
        if (weaponCategory == WeaponCategory.Melee)
        {
            box.enabled = true;
            headMask = _headMask;
            meleeDamage = _damage;
        }
        else
        {
            Debug.Log("카테고리 설정잘못");
        }
    }

    public void MeleeEnd()
    {
        meleeEnd();
    }
    private void meleeEnd()
    {
        OnMeleeHit?.Invoke(hitList);

        hitList.Clear();
        box.enabled = false;
        meleeDamage = 0;

    }

    //애니메이션
    private void reloadEnd()
    {
        weapon.ReloadAmmo();
        weapon.IsReloading = false;
    }

}
