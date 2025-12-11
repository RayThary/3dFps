using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowMissile : MonoBehaviour
{

    private float damage;
    private Vector3 shootDir;
    [SerializeField] private LayerMask hitObject;
    [SerializeField] private float speed = 30;
    private bool isCiritical = false;
    private void OnTriggerEnter(Collider other)
    {
        int layer = other.gameObject.layer;
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (!isCiritical)
            {
                other.GetComponent<Enemy>().HitEnemy(damage, 1, false);
            }
            else
            {
                other.GetComponent<Enemy>().HitEnemy(damage, 1.5f, true);
            }
        }
        else if ((hitObject.value & (1 << layer)) != 0)
        {
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }



    }


    public void SetUp(float _damage, float _missileSpeed, Vector3 _shootDir,bool _isCiritical)
    {
        damage = _damage;
        speed = _missileSpeed;
        shootDir = _shootDir;
        isCiritical = _isCiritical;
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
