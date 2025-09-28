using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    private LineRenderer lineR;

    [SerializeField] private Transform startPoint;
    private Transform endPoint;
    private Vector3 hitPoint;

    private RaycastHit hit;
    [SerializeField] private bool isStart = false;

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

            }
            if (layer == LayerMask.NameToLayer("Player"))
            {
                //임시대미지
                GameManager.instance.GetUnit.TakeDamge(5);
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
            hitPoint = endPoint.position;
        }
        lineR.SetPosition(0, startPoint.position);
        lineR.SetPosition(1, hitPoint);
    }
}
