using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTest : MonoBehaviour
{
    public bool testSpawn = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (testSpawn)
        {
            GameObject obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.EnemyA, transform);
            obj.transform.position = Vector3.zero;
            testSpawn = false;
        }
    }
}
