using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Weapon
{
    public Hammer(WeaponData _data) : base(_data, true) { }
    
    public override bool Attack(Transform _muzzlePoint)
    {
        if (currentAmmo > 0 && !base.isReloading)
        {
            


            currentAmmo--;
            return true;
        }
        else
        {
            return false;
        }
    }


}
