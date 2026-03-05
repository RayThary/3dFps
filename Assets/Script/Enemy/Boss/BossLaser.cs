using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    private LineRenderer lineR;

    private Transform startPoint;
    private Transform endPoint;
    private Vector3 hitPoint;

    [SerializeField] private float damage;

    private RaycastHit hit;
    private bool isStart = false;

    public void SetUp(Transform _startPoint)
    {
        startPoint = _startPoint;
        isStart = true;
    }
    
    public void LaserHitCheck(Enemy _enemy)
    {
        Vector3 dir = endPoint.position - startPoint.position;
        dir = dir.normalized;
        if (Physics.Raycast(startPoint.position, dir, out hit, Mathf.Infinity, LayerMask.GetMask("BossWall", "Player")))
        {
            int layer = hit.transform.gameObject.layer;
            if (layer == LayerMask.NameToLayer("BossWall"))
            {
                hit.transform.GetComponentInParent<BossWall>().RemoveWall();
                _enemy.WallCount = Mathf.Max(0, _enemy.WallCount - 1);
                PoolingManager.Instance.RemovePoolingObject(gameObject);
                return;
            }

            if (layer == LayerMask.NameToLayer("Player"))
            {
                GameManager.instance.GetUnit.TakeDamge(damage);
            }
        }

        isStart = false;
        PoolingManager.Instance.RemovePoolingObject(gameObject);
    }
    void Start()
    {
        lineR = GetComponent<LineRenderer>();
        endPoint = GameManager.instance.GetUnit.transform;
    }

    void Update()
    {
        if (isStart)
        {
            laserPatten();
        }
    }
    private void laserPatten()
    {
        Vector3 dir = endPoint.position - startPoint.position;
        dir = dir.normalized;

        if (Physics.Raycast(startPoint.position, dir, out hit, Mathf.Infinity, LayerMask.GetMask("BossWall")))
        {
            hitPoint = hit.point;
        }
        else
        {
            Transform camTrs = Camera.main.transform;
            Vector3 point = camTrs.position + camTrs.forward * 0.3f;
            point.y -= 1.5f;
            hitPoint = point;
        }
        lineR.SetPosition(0, startPoint.position);
        lineR.SetPosition(1, hitPoint);
    }
}
