using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTest : MonoBehaviour
{
    public bool testSpawn = false;

    public bool testD;
    public Transform testDistance;
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

        if (testD)
        {
            float a = Vector3.Distance(transform.position, testDistance.position);
            Debug.Log(a);

        }
    }
}
