using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemySpawn : MonoBehaviour
{
    private float spawnCoolTime;
    [SerializeField] private float spawnRange;
    [SerializeField]
    private int spawnEnemyCount;
    private int spawnWave;

    [SerializeField]//나중에 지워주고 외부에서 시작을알려줘야함
    private bool waveStart = false;
    [SerializeField] private LayerMask obstacleMask;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }
    void Start()
    {

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

    private void spawn()
    {
        //spawnEnemyCount = Random.Range(5, 10);
        bool spawnCheck = false;
        for (int i = 0; i < spawnEnemyCount; i++)
        {
            spawnCheck = false;
            int retry = 0;
            while (!spawnCheck && retry < 50)
            {
                Vector2 rand = Random.insideUnitCircle * spawnRange;
                Vector3 spawnPos = transform.position + new Vector3(rand.x, 0, rand.y);
                if (!Physics.CheckSphere(spawnPos, 0.5f, obstacleMask))
                {
                    GameObject obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.EnemyA, GameManager.instance.GetPoolinRoot);
                    obj.transform.position = spawnPos;
                    spawnCheck = true;
                }
                retry++;
                
            }
        }
    }
}
