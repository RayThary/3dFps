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

    //부딪히지않아야할레이어
    [SerializeField] private LayerMask outLayer;

    [SerializeField] private Transform skillSpawnTrs;
    private Transform spawnL;
    private Transform spawnR;

    private float coolTime;
    public float GetCoolTime { get { return coolTime; } }
    [SerializeField]
    private bool useSkill = false;
    public bool UseSkill { get { return useSkill; } set { useSkill = value; } }


    void Start()
    {
        currentSkillData = skillData.Find(x => x.SkillName == skillName.ToString());
        coolTime = currentSkillData.SkillCoolTime;

        unit = GetComponent<Unit>();
        spawnL = skillSpawnTrs.GetChild(0);
        spawnR = skillSpawnTrs.GetChild(1);
        int nowOutLayer = ~outLayer.value;
        switch (skillName)
        {
            case eSkillName.ThrowSlash:
                throwSlash = new UnitSkillThrowSlash();
                throwSlash.SetUp(this, currentSkillData.SkillDamage, coolTime, spawnR, spawnL, nowOutLayer);
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
                    useSkill = throwSlash.TryUesSkill();
                    break;
            }

        }


    }
}
