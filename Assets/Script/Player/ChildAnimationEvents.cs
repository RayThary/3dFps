using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildAnimationEvents : MonoBehaviour
{
    private Unit unit;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        unit = GetComponentInParent<Unit>();
    }

    private void reloadEnd()
    {
        unit.ReloadEnd();
    }

    private void meleeEnd()
    {
        unit.UnitMeleeEnd();
    }
  
}
