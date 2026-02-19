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
    [SerializeField]
    private Animator animator;
    public Animator Anim { get { return animator; } }
    private Weapon weapon;

    private Transform weaponPickup;
    public Transform WeaponPickup { get { return weaponPickup; } }

    private Transform muzzlePoint;
    public Transform GetMuzzlePoint { get { return muzzlePoint; } }

    //어웨이크끼리의 충돌경우방지
    [SerializeField] private Transform meshObject;
    public Transform MeshObject { get { return meshObject; } }


    [SerializeField] private float weaponLevel = 1;
    public float WeaponLevel { get { return weaponLevel; } set { weaponLevel = value; } }
    private int weaponMaxLevel;
    [SerializeField]
    private float gunDamage;
    public float GunDamage { get { return gunDamage; } set { gunDamage = value; } }

    private MeshRenderer mesh;

    private int weaponUpPrice = 100;
    public int WeaponUpPrice { get { return weaponUpPrice; } set { weaponUpPrice = value; } }

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
        if (animator != null)
            animator.enabled = true;
        transform.localPosition = Vector3.zero;
    }



    private void Awake()
    {
        animator = GetComponent<Animator>();
        mesh = GetComponentInChildren<MeshRenderer>();

        muzzlePoint = meshObject.Find("MuzzlePoint");

        if (weaponPickup == null)
        {
            weaponPickup = transform.Find("Mesh Object/WeaponPickup");
        }

    }

    public void WeaponPicupLayer(bool _value)
    {

        meshObject.gameObject.layer = _value ?
            LayerMask.NameToLayer("FirstPersonWeapon") : LayerMask.NameToLayer("Weapon");

    }
    public void WeaponZoom(bool _zoom)
    {
        mesh.enabled = !_zoom;
    }

    public string WeaponUpgrade()
    {
        if (weaponLevel >= weaponMaxLevel)
        {
            return "";
        }
        int rand = UnityEngine.Random.Range(0, 6);
        weaponLevel++;
        upgradePrice();
        return weapon.Upgrade(this, rand, weapon.Automatic);
    }

    private void upgradePrice()
    {
        int basePrice = 100;

        // 가격 상승률
        float priceRate = 1.15f;

        float rand = UnityEngine.Random.Range(0.9f, 1.15f);

        float price = basePrice * (weaponLevel * priceRate) * rand;

        weaponUpPrice = Mathf.RoundToInt(price);
    }
    public void UnitAttackSingleAnim()
    {
        if (animator != null)
        {
            //모든공격모션은 Attack으로바꿔줄필요가있음
            animator.SetTrigger("Attack");
        }
    }
    public void UnitAttackAutoAnim(bool _value)
    {
        if (animator != null)
        {
            animator.SetBool("Attack", _value);
        }
    }
    public void UnitReloadAnim()
    {
        if (animator != null)
        {
            animator.SetTrigger("Reload");
        }
    }





    //애니메이션
    private void reloadEnd()
    {
        weapon.ReloadAmmo();
        weapon.IsReloading = false;
    }

}
