using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemySpawn : MonoBehaviour
{
    private float spawnCoolTime;
    [SerializeField] private float spawnRange;// 몬스터 소환범위
    private int spawnWave;



    [SerializeField] private LayerMask obstacleMask;
    public LayerMask ObstacleMask { get { return obstacleMask; } }

    [SerializeField] private float inner;
    public float Inner { get { return inner; } }

    [SerializeField] private float outer;
    public float Outer { get { return outer; } }


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
        if (inner == 0)
        {
            inner = 4;
        }
        else
        {
            inner = transform.localScale.x / 2;
        }
        outer = spawnRange / 2;
    }




}