using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eWeaponType
{
    HandGun,
    SubMachineGun,
    Hammer,
    Sniper,
    ShotGun,
    Rifle,
}


public static class WeaponFactory
{
    public static Dictionary<eWeaponType, WeaponData> dataDic;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        dataDic = new Dictionary<eWeaponType, WeaponData>();

        var all = Resources.LoadAll<WeaponData>("Data/WeaponData");
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

            case eWeaponType.Sniper:
                return new Sniper(data);

            case eWeaponType.ShotGun:
                return new ShotGun(data);

            case eWeaponType.Rifle:
                return new Rifle(data);

            default:
                Debug.LogError($"{type}이(가) 없습니다");
                return null;
        }
    }


}
