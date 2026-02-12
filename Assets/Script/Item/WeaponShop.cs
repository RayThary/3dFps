using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShop : MonoBehaviour
{
    private bool isPlayerIn = false;
    private Unit unit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayerIn = true;
            unit = other.GetComponent<Unit>();
            GameManager.instance.CenterTextObj.SetActive(true);
            GameManager.instance.CenterText.text = "[F] 상점열기";
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayerIn = false;
            GameManager.instance.CenterTextObj.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!isPlayerIn) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            openShop();
        }
    }
    private void openShop()
    {
        Cursor.lockState = CursorLockMode.None;

        var slot1 = unit.CurrentSlot.weaponSlot[1];
        var slot2 = unit.CurrentSlot.weaponSlot[2];

        var slot1View = unit.UnitAttackModule.GetUnitWeaponChange.GetWeaponview(1);
        var slot2View = unit.UnitAttackModule.GetUnitWeaponChange.GetWeaponview(2);

        Debug.Log($"{slot1.GetName}, {slot2.GetName}");
        ShopUI.instance.WeaponShopUI.SetWeaponShop(slot1View, slot1.GetName, slot2View, slot2.GetName);

        ShopUI.instance.WeaponShopUI.ShopOpen();
        GameManager.instance.CenterTextObj.SetActive(false);
    }
}
