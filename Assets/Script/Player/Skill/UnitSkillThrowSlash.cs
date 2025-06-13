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


    public void SetUp(UnitSkill _unitSkill, float _damage,   float _coolTime, Transform _spawnPoint1,Transform _spawnPoint2)
    {
        unitSkill = _unitSkill;
        damage = _damage;
        
        coolTime = _coolTime;
        spawnPoint1 = _spawnPoint1;
        spawnPoint2 = _spawnPoint2;
    }

    public void TryUesSkill()
    {
        if (Time.time < lastUsedTime + coolTime)
            return;

        lastUsedTime = Time.time;
        unitSkill.StartCoroutine(spwanThrow());

    }

    IEnumerator spwanThrow()
    {
        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        Vector3 shootDir = ray.direction;
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

            GameObject obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.Temp, GameManager.instance.GetPoolinRoot);
            obj.transform.position = spawnPoint;
            obj.transform.rotation = Quaternion.LookRotation(shootDir);
            obj.GetComponent<ThrowSlash>().SetUp(damage);

            count++;
            yield return new WaitForSeconds(0.3f);
        }
    }

}
