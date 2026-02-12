using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoomRemove : MonoBehaviour
{
    private bool poolingCreate = true;
    private void OnEnable()
    {
        if (poolingCreate)
        {
            poolingCreate = false;
            return;
        }
        Invoke("remove", 1);
    }

    private void remove()
    {
        PoolingManager.Instance.RemovePoolingObject(gameObject);
    }
}
