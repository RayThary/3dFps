using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PoolingManager;

public class SpawnSetting : MonoBehaviour
{
    [SerializeField] private List<EnemySpawn> spawnTower = new List<EnemySpawn>();
    [SerializeField] private SpawnData spawnData;


    public int testStageNum;
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            EnemySpawn eSpawn = transform.GetChild(i).GetComponent<EnemySpawn>();
            if (eSpawn != null)
            {
                eSpawn.SpawnStart = true;
                spawnTower.Add(eSpawn);

            }
        }

        int nowSettingNum = GameManager.instance.GetStageNum - 1;
        nowSettingNum = testStageNum;
        if (nowSettingNum >= 0)
        {
            spawnSetting spSetting = spawnData.SpawnSetting[nowSettingNum];
            if (spSetting != null)
                basicSpawn(spSetting);
        }


    }

    private void basicSpawn(spawnSetting _spawnSetting)
    {
        int maxEnemy = _spawnSetting.StageSpawnCount;
        List<ePoolingObject> pool = new List<ePoolingObject>();

        for (int i = 0; i < _spawnSetting.CountA; i++)
            pool.Add(ePoolingObject.EnemyA);
        for (int i = 0; i < _spawnSetting.CountB; i++)
            pool.Add(ePoolingObject.EnemyB);
        for (int i = 0; i < _spawnSetting.CountC; i++)
            pool.Add(ePoolingObject.EnemyC);
        for (int i = 0; i < _spawnSetting.CountF; i++)
            pool.Add(ePoolingObject.EnemyF);

        for (int i = pool.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = pool[i];
            pool[i] = pool[j];
            pool[j] = temp;
        }

        int towerCount = 0;

        for (int i = 0; i < maxEnemy; i++)
        {

            spawnTower[towerCount].spawn(pool[i]);
            towerCount++;
            if (towerCount == spawnTower.Count)
            {
                towerCount = 0;
            }

        }
    }

    void Update()
    {

    }
}
