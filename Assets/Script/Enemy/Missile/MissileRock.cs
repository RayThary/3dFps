using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileRock : iMissile
{
    private Transform target;
    private Transform targetMesh;
    private float speed;
    private float rotateSpeed = 300;


    public MissileRock(Transform _target, float _speed)
    {
        target = _target;
        speed = _speed;
        targetMesh = target.GetChild(0);
    }
    public void OnHit(GameObject obj)
    {
        PoolingManager.Instance.RemovePoolingObject(target.gameObject);
    }

    public void Update()
    {
        target.position += target.forward * Time.deltaTime * speed;
        targetMesh.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);
    }

  
}
