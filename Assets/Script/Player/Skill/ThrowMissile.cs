using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowMissile : MonoBehaviour
{

    private float damage;
    private Vector3 shootDir;
    private Vector3 targetVec;
    [SerializeField] private LayerMask hitObject;
    [SerializeField] private float speed = 30;
    private bool isCiritical = false;

    private bool envCollisionArmed = false;

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
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }

        if (!envCollisionArmed)
            return;

        if ((hitObject.value & (1 << layer)) != 0)
        {
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }



    }


    public void SetUp(float _damage, float _missileSpeed, Vector3 _shootDir,Vector3 _targetVec, bool _isCiritical)
    {
        damage = _damage;
        speed = _missileSpeed;
        shootDir = _shootDir;
        targetVec = _targetVec;
        isCiritical = _isCiritical;
        envCollisionArmed = false;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += shootDir * Time.deltaTime * speed;

        if (!envCollisionArmed)
        {
            float forwardDot = Vector3.Dot(transform.position - targetVec, shootDir);
            if (forwardDot >= 0.5f)
            {
                envCollisionArmed = true;
            }
        }
    }
}
