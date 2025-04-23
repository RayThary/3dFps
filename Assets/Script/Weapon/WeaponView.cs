using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponView : MonoBehaviour
{
    //���⿡�־�Ѱ�
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
            //�����ݸ���� Attack���ιٲ����ʿ䰡����
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
