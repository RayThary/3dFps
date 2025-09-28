using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileStright : iMissile
{
    private Transform target;
    private float speed;

    public MissileStright(Transform _target, float _speed)
    {
        target = _target;
        speed = _speed;
    }

    public void OnHit(GameObject obj)
    {
        PoolingManager.Instance.RemovePoolingObject(target.gameObject);
    }

    public void Update()
    {
        target.position += target.forward * Time.deltaTime * speed;
    }
}
