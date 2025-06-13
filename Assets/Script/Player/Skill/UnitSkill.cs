using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkill : MonoBehaviour
{
    public enum eSkillName
    {
        ThrowSlash,
    }
    [SerializeField] private eSkillName skillName;

    [SerializeField] private List<SkillData> skillData = new List<SkillData>();
    private SkillData currentSkillData;

    private Unit unit;
    private UnitSkillThrowSlash throwSlash;


    [SerializeField] private Transform skillSpawnTrs;
    [SerializeField]private Transform spawnL;
    [SerializeField]private Transform spawnR;


    void Start()
    {
        currentSkillData = skillData.Find(x => x.SkillName == skillName.ToString());

        unit = GetComponent<Unit>();
        spawnL = skillSpawnTrs.GetChild(0);
        spawnR = skillSpawnTrs.GetChild(1);
        switch (skillName)
        {
            case eSkillName.ThrowSlash:
                throwSlash = new UnitSkillThrowSlash();
                throwSlash.SetUp(this, currentSkillData.SkillDamage, currentSkillData.SkillCoolTime, spawnR, spawnL);
                break;

        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            switch (skillName)
            {
                case eSkillName.ThrowSlash:
                    throwSlash.TryUesSkill();
                    break;
            }
        }
    }
}
