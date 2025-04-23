using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponView : MonoBehaviour
{
    //무기에넣어둘것
    private Animator animator;
    private Weapon weapon;

    private Transform muzzlePoint;
    public Transform GetMuzzlePoint { get { return muzzlePoint; } }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        muzzlePoint = transform.Find("MuzzlePoint");
    }

    public void Initialize(Weapon _weapon)
    {
        this.weapon = _weapon;
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
