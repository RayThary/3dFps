using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemySpawn : MonoBehaviour
{
    private float spawnCoolTime;
    [SerializeField] private float spawnRange;// 몬스터 소환범위
    [SerializeField] //몬스터소환개수 나중에지정해줄지 따로설정필요
    private int spawnEnemyCount;
    private int spawnWave;


    [SerializeField]//나중에 지워주고 외부에서 시작을알려줘야함
    private bool waveStart = false;
    [SerializeField] private LayerMask obstacleMask;

    private float inner;
    private float outer;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }
    void Start()
    {
        inner = transform.localScale.x / 2;
        outer = spawnRange / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (waveStart)
        {
            spawn();
            waveStart = false;
        }
    }

    private Vector3 spawnPoint()
    {
        float inner2 = inner * inner;
        float outer2 = outer * outer;

        float pointU = Random.value;

        float r2 = pointU * (outer2 + inner2) + inner2;

        float r = Mathf.Sqrt(r2);

        float theta = Random.Range(0, Mathf.PI * 2f);


        Vector3 offset = new Vector3(Mathf.Cos(theta) * r, 0f, Mathf.Sin(theta) * r);

        // 6) 최종 소환 위치
        Vector3 spawnPos = transform.position + offset;
        return spawnPos;
    }

    private void spawn()
    {
        bool spawnCheck = false;
        for (int i = 0; i < spawnEnemyCount; i++)
        {
            spawnCheck = false;

            int retry = 0;
            GameObject obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.EnemyA, GameManager.instance.GetPoolinRoot);
            obj.transform.position = Vector3.zero * -200;
            float objR = obj.GetComponent<BoxCollider>().bounds.extents.magnitude;
            Debug.Log(objR);
            while (!spawnCheck && retry < 50)
            {

                Vector3 spawnPos;
                spawnPos = spawnPoint();
                if (!Physics.CheckSphere(spawnPos, objR, obstacleMask))
                {
                    obj.transform.position = spawnPos;
                    spawnCheck = true;
                }

                retry++;

            }
        }
    }
}
