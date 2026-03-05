using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkillThrowMissile
{

    private UnitSkill unitSkill;

    private float damage;
    private int skillCount;
    private float fireInterval;
    private float missileSpeed;
    private bool canCritical;


    //쿨타임
    private float coolTime;
    private float lastUsedTime = -Mathf.Infinity;

    //소환위치
    private Transform spawnPoint1;
    private Transform spawnPoint2;

    private Transform unitTrs;

    //제외할 인식안할레이어
    private int outLayer;


    public void SetUp(UnitSkill _unitSkill, float _damage, int _skillCount, float _coolTime, float _missileSpeed, float _fireInterval
        , Transform _spawnPoint1, Transform _spawnPoint2, Transform _unitTrs, int _outLayer)
    {
        unitSkill = _unitSkill;
        damage = _damage;
        skillCount = _skillCount;
        missileSpeed = _missileSpeed;
        fireInterval = _fireInterval;
        canCritical = false;

        coolTime = _coolTime;

        spawnPoint1 = _spawnPoint1;
        spawnPoint2 = _spawnPoint2;
        unitTrs = _unitTrs;

        outLayer = _outLayer;
    }

    public bool TryUesSkill()
    {
        if (Time.time < lastUsedTime + coolTime)
            return false;

        lastUsedTime = Time.time;
        unitSkill.StartCoroutine(spwanThrow());
        return true;
    }

    IEnumerator spwanThrow()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        Vector3 targetPoint;
        Vector3 dir;

        float maxDistance = 100;
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, outLayer))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(maxDistance);

        }

        int count = 0;
        while (count < skillCount)
        {
            Vector3 spawnPoint;
            if (count % 2 == 0)
            {
                spawnPoint = spawnPoint1.position;
            }
            else
            {
                spawnPoint = spawnPoint2.position;
            }

            GameObject obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.SkillMissile, GameManager.instance.PoolingParents["SkillMissile"]);
            obj.transform.position = spawnPoint;
            dir = (targetPoint - spawnPoint).normalized;

            obj.transform.rotation = Quaternion.LookRotation(dir);

            bool isCrit = canCritical ? ciriticalCheck() : false;

            obj.GetComponent<ThrowMissile>().SetUp(damage, missileSpeed, dir, unitTrs.position, isCrit);
            count++;
            yield return new WaitForSeconds(fireInterval);
        }
    }

    private bool ciriticalCheck()
    {
        float rand = Random.Range(0f, 1f);

        if (rand < 0.2f) return true;

        return false;
    }

    public void ApplyUpgrade(UpgradeType _type, UnitSkill.ThrowMissile up)
    {
        switch (_type)
        {
            case UpgradeType.Damage:
                damage *= up.damageUp;
                break;

            case UpgradeType.CoolDown:
                coolTime *= up.coolDownRate;        //쿨타임
                break;

            case UpgradeType.MissileCount:
                skillCount += up.missileCountUp;    //개수
                break;

            case UpgradeType.FireInterval:
                fireInterval -= up.fireIntervalUp;  // 발사속도
                break;

            case UpgradeType.MissileSpeed:
                missileSpeed += up.missileSpeedUp;  //속도강화
                break;

            case UpgradeType.CriticalEnable:
                canCritical = true;                 //  크리티컬 ON
                break;
        }
    }

}
