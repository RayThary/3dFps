using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDodge : MonoBehaviour
{
    private Unit unit;
    private UnitDodge unitDodge;
    private Image dodgeCool;
    private bool isDodge;
    private float dodgeCoolTime;
    void Start()
    {
        unit = GameManager.instance.GetUnit;
        unitDodge= unit.GetComponent<UnitDodge>();
        dodgeCool = GetComponent<Image>();
        dodgeCoolTime = unitDodge.GetDodgeCool;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDodge && unit.IsDodge)
        {
            dodgeCool.fillAmount = 1;
            isDodge = true;
        }
        if (isDodge)
        {
            dodgeCool.fillAmount -= Time.deltaTime / dodgeCoolTime;
            if (dodgeCool.fillAmount <= 0)
            {
                isDodge = false;
            }
        }
    }
}
