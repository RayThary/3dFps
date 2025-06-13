using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/SkillData", fileName = "NewSkillData")]
public class SkillData : ScriptableObject
{
    public string SkillName;

    public int SkillCount;

    public float SkillDamage;
    public float SkillCoolTime;
  
}


