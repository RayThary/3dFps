using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class WeaponView : MonoBehaviour
{
    [SerializeField]
    private eWeaponType weaponType;
    public eWeaponType WeaponType { get { return weaponType; } }
    //무기에넣어둘것
    private Animator animator;
    private Weapon weapon;

    private Transform weaponPickup;
    public Transform WeaponPickup { get { return weaponPickup; } }

    private Transform muzzlePoint;
    public Transform GetMuzzlePoint { get { return muzzlePoint; } }

    private Transform meshObject;
    public Transform MeshObject { get { return meshObject; } }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        meshObject = transform.GetChild(0);
        muzzlePoint = meshObject.Find("MuzzlePoint");

        if (weaponPickup == null)
        {
            weaponPickup = transform.Find("Mesh Object/WeaponPickup");
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

    private void reloadEnd()
    {
        weapon.ReloadAmmo();
        weapon.IsReloading = false;
    }

}
