using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTest : MonoBehaviour
{
    public bool testSpawn = false;
    public bool a, b, c;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (testSpawn)
        {
            GameObject obj;
            if (a)
            {
                obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.EnemyA, transform);
            }
            else if (b)
            {
                obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.EnemyB, transform);
            }
            else
            {
                obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.EnemyC, transform);
            }
            obj.transform.position = Vector3.zero;
            testSpawn = false;
        }


    }
}
