using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class testItem : MonoBehaviour
{
    public bool a = false;
    void Start()
    {

    }

    int count = 0;
    public Transform a1;
    // Update is called once per frame
    void Update()
    {
        if (a)
        {
            GameObject obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.ItemCoin, a1);
            obj.transform.position = transform.position;
            count++;
            if (count > 3)
            {
                a = false;
                count = 0;
            }
        }
    }
}
