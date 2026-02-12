using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHole : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(removePoolingObject(1f));
    }

    private IEnumerator removePoolingObject(float _removeTime)
    {
        yield return new WaitForSeconds(_removeTime);
        PoolingManager.Instance.RemovePoolingObject(gameObject);
    }
    
}
