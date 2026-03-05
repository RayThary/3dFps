using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChargerState : IEnemyState
{
    private Enemy enemy;
    private Transform targetTrs;
    private Transform enemyTrs;
    private Vector3 targetVec;
    private float speed;
    private float chargerSpeed;

    private bool isAttack = false;

    private BoxCollider attackBox;

    //ÄðÅ¸ÀÓ 
    private float chargerCooltime = 3;
    private float lastUesdTime = 0;
    public bool CanEnter
    { get { return Time.time >= lastUesdTime + chargerCooltime; } set { CanEnter = value; } }


    public EnemyChargerState(Enemy _enemy, Transform _enemyTrs, Transform _targetTrs, BoxCollider _attackBox, float _speed)
    {
        enemy = _enemy;
        enemyTrs = _enemyTrs;
        targetTrs = _targetTrs;
        speed = _speed;
        attackBox = _attackBox;
        chargerSpeed = 25;
        lastUesdTime = -chargerCooltime;
    }
    public void Enter()
    {

        enemy.NavMesh.speed = chargerSpeed;
        enemy.NavMesh.SetDestination(targetTrs.position);
        targetVec = targetTrs.position;
        lastUesdTime = Time.time;
        isAttack = false;
    }


    public void Update()
    {
        chargerAttack();
    }

    private void chargerAttack()
    {
        float dis = Vector3.Distance(enemyTrs.position, targetVec);
        if (dis <= 12 && !isAttack)
        {
            enemy.Animator.SetTrigger("Attack");
            attackBox.enabled = true;
            isAttack = true;
        }
    }
    public void Exit()
    {
        lastUesdTime = Time.time;
        enemy.NavMesh.speed = speed;
        enemy.EnemyStop = false;
        attackBox.enabled = false;
        isAttack = false;

    }
}
