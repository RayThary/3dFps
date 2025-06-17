using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSlash : MonoBehaviour
{

    private float damage;
    private Vector3 shootDir;
    [SerializeField] private LayerMask hitObject;
    [SerializeField] private float speed = 30;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.GetComponent<Enemy>().HitEnemy(damage, 1, false);
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.layer == hitObject)
        {
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }
    }
    public void SetUp(float _damage,Vector3 _shootDir)
    {
        damage = _damage;
        shootDir = _shootDir;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += shootDir * Time.deltaTime * speed;
    }
}
