using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandMotion : MonoBehaviour
{
    private Unit unit;
    [SerializeField] private Transform unitHand;
    [SerializeField] private float speed;

    private bool isHand = false;
    public bool IsHand { set { IsHand = value; } }


    void Start()
    {
        unit = GetComponent<Unit>();
    }

    public void handMotion(UnitWeaponChange _weaponChange, int _num)
    {
        if (_num == _weaponChange.GetCurrentSlot)
            return;

        if (isHand || !_weaponChange.IsChange)
            return;

        isHand = true;

        StartCoroutine(handMotions(_weaponChange, _num));
    }

    IEnumerator handMotions(UnitWeaponChange _weaponChange, int _num)
    {

        float downLimit = -1.4f;
        float upLimit = 0.3f;

        while (unitHand.localPosition.y > downLimit)
        {
            unitHand.localPosition += Vector3.down * speed * Time.deltaTime;
            yield return null;
        }


        _weaponChange.WeaponSwitch(_num);

        while (unitHand.localPosition.y < upLimit)
        {
            unitHand.localPosition += Vector3.up * speed * Time.deltaTime;
            yield return null;
        }

        isHand = false;

    }

}
