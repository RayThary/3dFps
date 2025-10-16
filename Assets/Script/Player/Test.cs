using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.EnemyD, transform);
        obj.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
     
        
    }
}
