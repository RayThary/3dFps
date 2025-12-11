using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public static ShopUI instance;

    private WeaponShopUI weaponShopUI;
    public WeaponShopUI WeaponShopUI { get { return weaponShopUI; } }

    private ConsumableShopUI consumableShopUI;
    public ConsumableShopUI ConsumableShopUI { get { return consumableShopUI; } }

    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        weaponShopUI = GetComponentInChildren<WeaponShopUI>();
        consumableShopUI = GetComponentInChildren<ConsumableShopUI>();
    }

}
