using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eWeaponType
{
    HandGun,
    SubMachineGun,
    Hammer,
}


public static class WeaponFactory
{
    public static Dictionary<eWeaponType, WeaponData> dataDic;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        dataDic = new Dictionary<eWeaponType, WeaponData>();

        var all = Resources.LoadAll<WeaponData>("WeaponData");
        foreach(var data in all)
        {
            dataDic[data.WeaponType] = data;
        }
    }

    public static Weapon CreateWeapon(eWeaponType type)
    {
        if (!dataDic.ContainsKey(type))
        {
            Debug.LogError($"{type}이(가) 없습니다.");
            return null;
        }

        var data = dataDic[type];
        switch (type)
        {
            case eWeaponType.HandGun:
                return new HandGun(data);

            case eWeaponType.SubMachineGun:
                return new SubMachineGun(data);

            case eWeaponType.Hammer:
                return new Hammer(data);

            default:
                Debug.LogError($"{type}이(가) 없습니다");
                return null;
        }
    }


}
