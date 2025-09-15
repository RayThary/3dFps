using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCurve : iMissile
{

    Transform target;
    private float speed;
    private Vector3 p0, p1, p2;
    private float curveT = 0;



    public MissileCurve(Transform _target, float _speed)
    {
        target = _target;
        speed = _speed;
    }
    public void SetP0(Vector3 _p0)
    {
        p0 = _p0;
        p2 = GameManager.instance.GetUnit.transform.position;

        Vector3 mid = (p0 + p2) * 0.5f;
        float randX = Random.Range(-10f, 10f);
        float randY = Random.Range(-2, 10f);
        float randZ = Random.Range(-10f, 10f);
        p1 = mid + new Vector3(randX, randY, randZ);
    }

    public void OnHit(GameObject obj)
    {
        PoolingManager.Instance.RemovePoolingObject(obj);
    }

    public void Update()
    {
        curveT += Time.deltaTime * speed * 0.1f;
        float t = Mathf.Clamp01(curveT);
        Vector3 tan = bezierTangent(p0, p1, p2, t);

        target.position = bezier(p0, p1, p2, curveT);

        target.rotation = Quaternion.LookRotation(tan.normalized, Vector3.up);

        if (curveT >= 1f)
        {
            PoolingManager.Instance.RemovePoolingObject(target.gameObject);
        }
    }


    private Vector3 bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        Vector3 P1 = Vector3.Lerp(p0, p1, t);
        Vector3 P2 = Vector3.Lerp(p1, p2, t);
        return Vector3.Lerp(P1, P2, t);
    }

    //πÊ«‚
    private Vector3 bezierTangent(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return 2f * (1f - t) * (p1 - p0) + 2f * t * (p2 - p1);
    }


}
