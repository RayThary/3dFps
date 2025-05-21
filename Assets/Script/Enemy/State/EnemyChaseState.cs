using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    private Enemy enemy;
    private Transform playerTrs;
    private Transform enemyTrs;
    private float speed;
    private float stopDistance;

    private bool wasChasing = false;

    public bool CanEnter => true;

    public EnemyChaseState(Enemy _enemy, Transform _playerTrs, Transform _enemyTrs, float _speed, float _stopDistance)
    {
        enemy = _enemy;
        enemyTrs = _enemyTrs;
        playerTrs = _playerTrs;
        speed = _speed;
        stopDistance = _stopDistance;
    }

    public void Enter()
    {
        enemy.NavMesh.speed = speed;
        enemy.NavMesh.ResetPath();
        enemy.NavMesh.SetDestination(playerTrs.position);
    }


    public void Update()
    {
        bool isChasingNow = chase();

        if (isChasingNow != wasChasing)
        {
            wasChasing = isChasingNow;
        }

        if (isChasingNow == false)
        {
            enemy.NavMesh.SetDestination(playerTrs.position);
        }

    }

    private bool chase()
    {
        if (enemy.EnemyStop)
        {
            return true;
        }

        float dis = Vector3.Distance(playerTrs.position, enemyTrs.position);
        if (enemy.EnemyAttackState.CanEnter && dis <= stopDistance)
        {
            enemy.StateMachine.ChangeState(enemy.EnemyAttackState);
            return true;
        }
        else
        {
            return false;
        }

    }
    public void Exit()
    {
        enemy.EnemyStop = true;
        wasChasing = false;
    }
}
