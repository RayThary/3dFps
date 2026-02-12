using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySniperState : IEnemySniperState
{
    private float sniperCooltime = 5;
    private float lastUesdTime = 0;
    public bool CanEnter
    { get { return Time.time >= lastUesdTime + sniperCooltime; } set { CanEnter = value; } }

    public bool SniperShot { get; set; }

    private float damage;

    private float shotTime = 2;
    //제외할레이어가 늘어나면 수정필요
    private int excludeLayer => ~LayerMask.GetMask("Player");

    private Enemy enemy;
    private Transform snipingTrs;
    private Transform targetTrs;

    private LineRenderer lineR;

    public EnemySniperState(Enemy _enemy, Transform _enemyTrs, Transform _targetTrs, LineRenderer _lineR, float _damage)
    {
        enemy = _enemy;
        snipingTrs = _enemyTrs;
        targetTrs = _targetTrs;
        lineR = _lineR;
        damage = _damage;
        lastUesdTime = -sniperCooltime;
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
            targetVec = targetTrs.position + (Vector3.up * 2);
            dir = targetVec - snipingTrs.position;
            if (Physics.Raycast(snipingTrs.position, dir, out hit, Mathf.Infinity, excludeLayer))
            {

                lineR.SetPosition(0, snipingTrs.position);
                lineR.SetPosition(1, hit.point);

            }
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        targetVec = targetTrs.position + (Vector3.up * 2);
        dir = targetVec - snipingTrs.position;
        if (Physics.Raycast(snipingTrs.position, dir, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Player"))
            {
                GameManager.instance.GetUnit.TakeDamge(damage);
            }

        }

        lineR.enabled = false;
        enemy.Animator.speed = 1;
    }
    public void Exit()
    {
        lastUesdTime = Time.time;
        enemy.EnemyStop = false;
    }

}
