using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveRemove : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("remove", 0.5f);
    }

    private void remove()
    {
        PoolingManager.Instance.RemovePoolingObject(gameObject);
    }
}
