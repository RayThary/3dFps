using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class UnitWeaponChange
{
    private Dictionary<int, Weapon> weaponSlot;
    private GameObject slot1Obj;
    private GameObject slot2Obj;

    private Dictionary<int, WeaponView> weaponViewSlot = new Dictionary<int, WeaponView>();
    private int currentSlot;

    
    public Weapon GetCurrentWeapon()
    {
        return weaponSlot[currentSlot];
    }
    public UnitWeaponChange(Dictionary<int, Weapon> _weaponSlot, GameObject _slot1Obj, GameObject _slot2Obj, int _defaultSlot = 1)
    {
        this.weaponSlot = _weaponSlot;
        this.slot1Obj = _slot1Obj;
        this.slot2Obj = _slot2Obj;

        currentSlot = _defaultSlot;     

        slotSwitch();
        weaponInstantiate(_slot1Obj, _slot2Obj);
    }

    private void slotSwitch()
    {
        slot1Obj.SetActive(currentSlot == 1);
        slot2Obj.SetActive(currentSlot == 2);
    }

    private void weaponInstantiate(GameObject _slot1, GameObject _slot2)
    {
        var weapon1 = weaponSlot[1];
        var weapon1Obj = GameObject.Instantiate(weapon1.WeaponPrefeb, _slot1.transform);
        var view1 = weapon1Obj.GetComponent<WeaponView>();
        weaponViewSlot[1] = view1;

        var weapon2 = weaponSlot[2];
        var weapon2Obj = GameObject.Instantiate(weapon2.WeaponPrefeb, _slot2.transform);
        var view2 = weapon2Obj.GetComponent<WeaponView>();
        weaponViewSlot[2] = view2;

    }
    public WeaponView GetCurrentWeaponview()
    {
        return weaponViewSlot[currentSlot];
    }


    public void WeaponSwitch(int _slotNum)
    {
        if (_slotNum == currentSlot) return;
        if (!weaponSlot.ContainsKey(_slotNum)) return;

        currentSlot = _slotNum;
        weaponViewSlot[currentSlot].Initialize(weaponSlot[currentSlot]);

       slotSwitch();
    }

    public void WeaponChange(GameObject _newWeapon)
    {
        weaponSlot.Remove(currentSlot);
        //weaponSlot.Add()
    }
}
