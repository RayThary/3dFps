using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySniperState : IEnemySniperState
{
    private float sniperCooltime = 50;
    private float lastUesdTime = 0;
    public bool CanEnter
    { get { return Time.time >= lastUesdTime + sniperCooltime; } set { CanEnter = value; } }

    public bool SniperShot { get; set; }

    private float damage;

    private float shotTime = 2;

    private LayerMask obstacleMask;

    private Enemy enemy;
    private Transform snipingTrs;
    private Transform targetTrs;

    private LineRenderer lineR;

    public EnemySniperState(Enemy _enemy, Transform _enemyTrs, Transform _targetTrs, LineRenderer _lineR, float _damage, LayerMask _obstacleMask)
    {
        enemy = _enemy;
        snipingTrs = _enemyTrs;
        targetTrs = _targetTrs;
        lineR = _lineR;
        damage = _damage;
        lastUesdTime = -sniperCooltime;
        obstacleMask = _obstacleMask;
    }

    public void Enter()
    {
        enemy.Animator.SetTrigger("Attack");
        lastUesdTime = Time.time;
        SniperShot = false;
    }

    public void Update()
    {
        enemy.transform.LookAt(targetTrs);
        if (SniperShot)
        {
            enemy.StartCoroutine(sniping());
            SniperShot = false;
        }
    }

    IEnumerator sniping()
    {
        float endTime = Time.time + shotTime;
        lineR.enabled = true;
        RaycastHit hit;
        Vector3 dir;
        Vector3 targetVec;
        while (endTime >= Time.time)
        {

            Transform camTrs = Camera.main.transform;
            Vector3 point = camTrs.position + camTrs.forward * 0.3f;
            point.y -= 1.5f;
            lineR.SetPosition(0, snipingTrs.position);
            lineR.SetPosition(1, point);
            yield return null;
        }

        lineR.enabled = false;
        targetVec = targetTrs.position + (Vector3.up * 2);
        dir = (targetVec - snipingTrs.position).normalized;

        yield return new WaitForSeconds(0.1f);

        int mask = obstacleMask | LayerMask.GetMask("Player", "Ground");
        if (Physics.Raycast(snipingTrs.position, dir, out hit, Mathf.Infinity, mask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (!enemy.IsDead)
                {
                    GameManager.instance.GetUnit.TakeDamge(damage);
                }
            }

        }

        enemy.Animator.speed = 1;
    }
    public void Exit()
    {
        lastUesdTime = Time.time;
        enemy.EnemyStop = false;
    }

}
