using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private bool poolingCreate = true;
    private void OnEnable()
    {
        if (poolingCreate)
        {
            poolingCreate = false;
            return;
        }
        Invoke("removePooling", 0.1f);
    }
    void Start()
    {        
    }

    // Update is called once per frame
    void Update()
    {
        bulletMuzzle();
    }

    private void bulletMuzzle()
    {
        //지금은모든게머즐이들어가므로 나중에머즐없는경우에만 따로if추가할것
        transform.Rotate(new Vector3(3, 0, 0) * Time.deltaTime * 180);
    }

    private void removePooling()
    {
        PoolingManager.Instance.RemovePoolingObject(gameObject);
    }
}
