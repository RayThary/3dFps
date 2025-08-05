using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemySpawn : MonoBehaviour
{
    private float spawnCoolTime;
    [SerializeField] private float spawnRange;// 몬스터 소환범위
    private int spawnEnemyCount = 50;
    private int spawnWave;


    [SerializeField]//나중에 지워주고 외부에서 시작을알려줘야함
    private bool spawnStart = false;
    public bool SpawnStart { set { spawnStart = value; } }
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField]
    private float inner;
    [SerializeField]
    private float outer;
    public bool ShowGizmo = false;
    void OnDrawGizmos()
    {
        if (ShowGizmo)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, spawnRange);
        }
    }
    private void Awake()
    {
        inner = transform.localScale.x / 2;
        outer = spawnRange / 2;
    }



    // Update is called once per frame
    void Update()
    {

    }

    private Vector3 spawnPoint()
    {
        if (inner == 0 || outer == 0)
        {
            Debug.Log("문제");
        }
        float inner2 = inner * inner;
        float outer2 = outer * outer;

        float pointU = Random.value;

        //float r2 = pointU * (outer2 + inner2) + inner2;
        float r2 = inner2 + pointU * (outer2 - inner2);

        float r = Mathf.Sqrt(r2);

        float theta = Random.Range(0, Mathf.PI * 2f);


        Vector3 offset = new Vector3(Mathf.Cos(theta) * r, 0f, Mathf.Sin(theta) * r);

        //최종소환 위치
        Vector3 spawnPos = transform.position + offset;
        return spawnPos;
    }

    public void spawn(PoolingManager.ePoolingObject _Enemy)
    {

        bool spawnCheck = false;

        int retry = 0;
        string enemyName = _Enemy.ToString();
        GameObject obj = PoolingManager.Instance.CreateObject(_Enemy, GameManager.instance.PoolingParents[enemyName]);
        obj.transform.position = Vector3.zero * -200;
        float objR = obj.GetComponent<BoxCollider>().bounds.extents.magnitude;
        objR += 1;

        Vector3 spawnPos;
        while (!spawnCheck && retry < 50)
        {

            spawnPos = spawnPoint();
            if (!Physics.CheckSphere(spawnPos, objR, obstacleMask))
            {
                obj.transform.position = spawnPos;
                obj.GetComponent<Enemy>().SetUpStat();
                spawnCheck = true;
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
}