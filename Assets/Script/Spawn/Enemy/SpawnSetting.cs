using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static PoolingManager;

public class SpawnSetting : MonoBehaviour
{
    [SerializeField] private List<EnemySpawn> spawnTower = new List<EnemySpawn>();
    [SerializeField] private SpawnData spawnData;



    [SerializeField] private LayerMask obstacleMask;
    private float inner;
    private float outer;

    [SerializeField] private List<NavMeshAgent> enemy = new List<NavMeshAgent>();
    private List<Vector3> enemyPos = new List<Vector3>();
    private bool enemyAggro = true;
    void Start()
    {
        GameManager.instance.SetSpawnSetting = this;

        for (int i = 0; i < transform.childCount; i++)
        {
            EnemySpawn eSpawn = transform.GetChild(i).GetComponent<EnemySpawn>();
            if (eSpawn != null)
            {
                spawnTower.Add(eSpawn);

            }
        }
        inner = spawnTower[0].Inner;
        outer = spawnTower[0].Outer;
        obstacleMask = spawnTower[0].ObstacleMask;

        int nowSettingNum = GameManager.instance.GetStageNum - 1;
        if (nowSettingNum >= 0)
        {
            spawnSetting spSetting = spawnData.SpawnSetting[nowSettingNum];
            GameManager.instance.EnemyMaxCount = spSetting.StageSpawnCount;
            if (spSetting != null)
                basicSpawn(spSetting);
        }

        StartCoroutine(spawnSetUp());
    }

    private void Update()
    {
        if (enemyAggro && GameManager.instance.StageEnemyAggro)
        {
            for (int i = 0; i < enemy.Count; i++)
            {
                if (enemy[i].gameObject.activeSelf)
                {
                    Enemy _enemy = enemy[i].GetComponent<Enemy>();
                    _enemy.HitCheck = true;
                }
            }
            enemyAggro = false;
        }
    }

    IEnumerator spawnSetUp()
    {
        yield return null;
        for (int i = 0; i < enemy.Count; i++)
        {
            enemy[i].Warp(enemyPos[i]);
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
        for (int i = 0; i < _spawnSetting.CountD; i++)
            pool.Add(ePoolingObject.EnemyD);
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

            spawn(pool[i], spawnTower[towerCount].transform.position);
            towerCount++;
            if (towerCount == spawnTower.Count)
            {
                towerCount = 0;
            }

        }

    }

    private Vector3 spawnPoint(Vector3 _spawnOffest)
    {
        if (inner == 0 || outer == 0)
        {
            Debug.LogError("SpawnPointError");
        }
        float inner2 = inner * inner;
        float outer2 = outer * outer;

        float pointU = Random.value;

        float r2 = inner2 + pointU * (outer2 - inner2);

        float r = Mathf.Sqrt(r2);

        float theta = Random.Range(0, Mathf.PI * 2f);


        Vector3 offset = new Vector3(Mathf.Cos(theta) * r, 0f, Mathf.Sin(theta) * r);

        //최종소환 위치
        Vector3 spawnPos = _spawnOffest + offset;
        spawnPos.y = 0;
        return spawnPos;
    }

    private void spawn(PoolingManager.ePoolingObject _Enemy, Vector3 _spawnOffest)
    {

        bool spawnCheck = false;

        int retry = 0;
        string enemyName = _Enemy.ToString();
        GameObject obj = PoolingManager.Instance.CreateObject(_Enemy, GameManager.instance.PoolingParents[enemyName]);
        obj.GetComponent<NavMeshAgent>().enabled = false;
        obj.transform.position = Vector3.zero;
        float objR = obj.GetComponent<BoxCollider>().bounds.extents.magnitude;
        objR += 1;

        Vector3 spawnPos;
        while (!spawnCheck && retry < 50)
        {

            spawnPos = spawnPoint(_spawnOffest);
            if (!Physics.CheckSphere(spawnPos, objR, obstacleMask))
            {
                obj.transform.position = spawnPos;
                obj.GetComponent<Enemy>().SetUpStat();
                spawnCheck = true;
                enemy.Add(obj.GetComponent<NavMeshAgent>());
                enemyPos.Add(spawnPos);
                obj.GetComponent<NavMeshAgent>().enabled = true;
                break;
            }

            retry++;


            if (retry == 50 || transform.position == spawnPos)
            {
                Debug.Log($"타워 {transform.name}실패:{spawnPos}");
            }
        }

        if (!spawnCheck)
        {
            Debug.Log("스폰실패 위치찾지못함");
            PoolingManager.Instance.RemovePoolingObject(obj);
        }

    }

    //버그가생겨서 포탈이나왔을경우 몬스터제거용
    public void RemoveEnemy()
    {
        for (int i = 0; i < enemy.Count; i++)
        {
            if (enemy[i].gameObject != null)
            {
                if (enemy[i].gameObject.activeSelf == true)
                {
                    PoolingManager.Instance.RemovePoolingObject(enemy[i].gameObject);
                }
            }
        }
    }
}
