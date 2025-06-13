using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSlash : MonoBehaviour
{

    private float damage;
    [SerializeField] private LayerMask hitObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.GetComponent<Enemy>().HitEnemy(damage, 1, false);
        }

        if (other.gameObject.layer == hitObject)
        {
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }

    }

    public void SetUp(float _damage)
    {
        damage = _damage;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * 5;
    }
}
