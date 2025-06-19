using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkillThrowSlash
{

    private UnitSkill unitSkill;

    //대미지관련
    private float damage;
    //대미지배율
    private float damageMultiplier = 1.5f;

    private float criticalChance;
    private float criticalDamage;


    //쿨타임
    private float coolTime;
    private float lastUsedTime = -Mathf.Infinity;

    //소환위치
    private Transform spawnPoint1;
    private Transform spawnPoint2;

    //제외할 인식안할레이어
    private int outLayer;


    public void SetUp(UnitSkill _unitSkill, float _damage, float _coolTime, Transform _spawnPoint1, Transform _spawnPoint2, int _outLayer)
    {
        unitSkill = _unitSkill;
        damage = _damage;

        coolTime = _coolTime;
        spawnPoint1 = _spawnPoint1;
        spawnPoint2 = _spawnPoint2;
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
        while (count < 8)
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

            GameObject obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.TempSkillMissle, GameManager.instance.PoolingParents["TempSkillMissle"]);
            obj.transform.position = spawnPoint;
            dir = (targetPoint - spawnPoint).normalized;

            obj.transform.rotation = Quaternion.LookRotation(dir);
            obj.GetComponent<ThrowSlash>().SetUp(damage, dir);
            count++;
            yield return new WaitForSeconds(0.1f);
        }
    }

}
