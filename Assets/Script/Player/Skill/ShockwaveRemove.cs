using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveRemove : MonoBehaviour
{
    private bool poolingCreate = true;
    private void OnEnable()
    {
        if (poolingCreate)
        {
            poolingCreate = false;
            return;
        }
        Invoke("remove", 1f);
    }

    private void remove()
    {
        PoolingManager.Instance.RemovePoolingObject(gameObject);
    }
}
