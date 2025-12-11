using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    Damage,
    CoolDown,
    //미사일
    MissileCount,
    FireInterval,
    MissileSpeed,
    CriticalEnable,
    //쇼크웨이브
    Radius,
    Slow,
    ResidueShockwave,
    DoubleShockwave,
}

public class UnitSkill : MonoBehaviour
{
    public enum eSkillName
    {
        ThrowMissile,
        Shockwave,
    }
    [SerializeField] private eSkillName skillName;
    public eSkillName SkillName { get { return skillName; } set { skillName = value; } }


    private Unit unit;
    private UnitSkillThrowMissile unitThrowMissile;
    private UnitSkillShockwave unitShockwave;

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

    [System.Serializable]
    public class ThrowMissile
    {
        public float damage = 70;
        public int missileCount = 4;
        public float fireInterval = 0.15f;
        public float coolTime;
        public float missileSpeed = 30f;

        // 업그레이드
        public float damageUp;
        public float coolDownRate;

        public int missileCountUp;
        public float fireIntervalUp;
        public float missileSpeedUp;
        public bool criticalEnable = false;
    }
    [SerializeField] private ThrowMissile throwMissile;
    [System.Serializable]
    public class ThrowMissileUpgradeLevel
    {
        public int damageLevel;
        public int coolDownLevel;
        public int missileCountLevel;
        public int fireIntervalLevel;
        public int missileSpeedLevel;
        public int criticalEnableLevel;

        // 최대 레벨들 (Inspector에서 설정 가능)
        public int damageMaxLevel = 5;
        public int coolDownMaxLevel = 3;
        public int missileCountMaxLevel = 4;
        public int fireIntervalMaxLevel = 4;//발사간격
        public int missileSpeedMaxLevel = 4;//미사일속도
        public int criticalEnableMaxLevel = 1; // 1번만 가능
    }
    [SerializeField] private ThrowMissileUpgradeLevel throwMissileUpgradeLevel;
    public ThrowMissileUpgradeLevel missileUpLevel { get { return throwMissileUpgradeLevel; } }

    [System.Serializable]
    public class Shockwave
    {
        public float damage = 70;
        public float coolTime;
        public float shockwaveRadius;

        //업그레이드수치
        public float damageUp;
        public float coolTimeUp;
        public float radiusUp;
        public float slowAmount;
        public float residueDuration;  // 지속 시간
        public bool doubleShockwave;
    }
    [SerializeField] private Shockwave shockwave;
    [System.Serializable]
    public class ShockwaveUpgradeLevel
    {
        // 현재 레벨
        public int damageLevel;
        public int coolTimeLevel;
        public int radiusLevel;
        public int slowLevel;
        public int residueLevel;
        public int doubleShockwaveLevel;

        // 최대 레벨 
        public int damageMaxLevel = 5;
        public int coolTimeMaxLevel = 3;
        public int radiusMaxLevel = 3;
        public int slowMaxLevel = 2;
        public int residueMaxLevel = 3;
        public int doubleShockwaveMaxLevel = 1;
    }
    [SerializeField] private ShockwaveUpgradeLevel shockwaveUpgradeLevel;
    public ShockwaveUpgradeLevel shockwaveUpLevel { get { return shockwaveUpgradeLevel; } }


    public float debugSlamRadius;
    private void OnDrawGizmos()
    {
        if (debugSlamRadius > 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, debugSlamRadius);
        }
    }
    private void Awake()
    {
        switch (skillName)
        {
            case eSkillName.ThrowMissile:
                coolTime = throwMissile.coolTime;
                break;
            case eSkillName.Shockwave:
                coolTime = shockwave.coolTime;
                break;
        }
    }


    void Start()
    {

        unit = GetComponent<Unit>();
        spawnL = skillSpawnTrs.GetChild(0);
        spawnR = skillSpawnTrs.GetChild(1);
        int nowOutLayer = ~outLayer.value;
        switch (skillName)
        {
            case eSkillName.ThrowMissile:
                unitThrowMissile = new UnitSkillThrowMissile();
                unitThrowMissile.SetUp(this, throwMissile.damage, throwMissile.missileCount, throwMissile.missileSpeed, throwMissile.fireInterval,
                    coolTime, spawnR, spawnL, nowOutLayer);
                break;
            case eSkillName.Shockwave:
                unitShockwave = new UnitSkillShockwave();
                unitShockwave.SetUp(this, shockwave.damage, coolTime, shockwave.shockwaveRadius, unit);
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
                case eSkillName.ThrowMissile:
                    useSkill = unitThrowMissile.TryUesSkill();
                    break;
                case eSkillName.Shockwave:
                    useSkill = unitShockwave.TryUseSkill();
                    break;
            }

        }
    }

    public List<UpgradeType> GetAvailableUpgradeCards()
    {
        List<UpgradeType> list = new();

        if (skillName == eSkillName.Shockwave)
        {
            var up = shockwaveUpgradeLevel;

            if (up.damageLevel < up.damageMaxLevel)
                list.Add(UpgradeType.Damage);

            if (up.coolTimeLevel < up.coolTimeMaxLevel)
                list.Add(UpgradeType.CoolDown);

            if (up.radiusLevel < up.radiusMaxLevel)
                list.Add(UpgradeType.Radius);

            if (up.slowLevel < up.slowMaxLevel)
                list.Add(UpgradeType.Slow);

            if (up.residueLevel < up.residueMaxLevel)
                list.Add(UpgradeType.ResidueShockwave);

            if (up.doubleShockwaveLevel < up.doubleShockwaveMaxLevel)
                list.Add(UpgradeType.DoubleShockwave);
        }


        if (skillName == eSkillName.ThrowMissile)
        {
            var up = throwMissileUpgradeLevel;

            if (up.damageLevel < up.damageMaxLevel)
                list.Add(UpgradeType.Damage);

            if (up.coolDownLevel < up.coolDownMaxLevel)
                list.Add(UpgradeType.CoolDown);

            if (up.missileCountLevel < up.missileCountMaxLevel)
                list.Add(UpgradeType.MissileCount);

            if (up.fireIntervalLevel < up.fireIntervalMaxLevel)
                list.Add(UpgradeType.FireInterval);

            if (up.missileSpeedLevel < up.missileSpeedMaxLevel)
                list.Add(UpgradeType.MissileSpeed);

            if (up.criticalEnableLevel < up.criticalEnableMaxLevel)
                list.Add(UpgradeType.CriticalEnable);
        }

        return list;
    }
    public void ShockwaveSkillUpgrade(UpgradeType _type)
    {
        unitShockwave.ApplyUpgrade(_type, shockwave);
        var up = shockwaveUpgradeLevel;

        switch (_type)
        {
            case UpgradeType.Damage:
                up.damageLevel++;
                break;

            case UpgradeType.CoolDown:
                up.coolTimeLevel++;
                break;

            case UpgradeType.Radius:
                up.radiusLevel++;
                break;

            case UpgradeType.Slow:
                up.slowLevel++;
                break;

            case UpgradeType.ResidueShockwave:
                up.residueLevel++;
                break;

            case UpgradeType.DoubleShockwave:
                up.doubleShockwaveLevel = 1;
                break;
        }
    }

    public void MissileSkillUpgrade(UpgradeType type)
    {
        unitThrowMissile.ApplyUpgrade(type, throwMissile);

        var up = throwMissileUpgradeLevel;

        switch (type)
        {
            case UpgradeType.Damage:
                up.damageLevel++;
                break;

            case UpgradeType.CoolDown:
                up.coolDownLevel++;
                break;

            case UpgradeType.MissileCount:
                up.missileCountLevel++;
                break;

            case UpgradeType.FireInterval:
                up.fireIntervalLevel++;
                break;

            case UpgradeType.MissileSpeed:
                up.missileSpeedLevel++;
                break;

            case UpgradeType.CriticalEnable:
                up.criticalEnableLevel = 1;
                break;
        }
    }
}
