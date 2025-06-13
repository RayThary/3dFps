using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyRangerChaseState : IEnemyState
{
    private Enemy enemy;
    private Transform playerTrs;
    private Transform enemyTrs;
    private float speed;
    private float stopDistance;

    private bool wasChasing = false;

    public bool CanEnter => true;

    public EnemyRangerChaseState(Enemy _enemy, Transform _playerTrs, Transform _enemyTrs, float _speed, float _stopDistance)
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

        float dis = Vector3.Distance(enemyTrs.position, playerTrs.position);


        if (isChasingNow != wasChasing)
        {
            wasChasing = isChasingNow;
            enemy.NavMesh.updateRotation = !isChasingNow;
        }

        if (Mathf.Abs(dis - (stopDistance - 0.5f)) <= 0.5f)
        {
            enemy.NavMesh.ResetPath();
            enemy.Animator.SetBool("Idle", true);
            return;
        }
        else
        {
            enemy.Animator.SetBool("Idle", false);
        }

        if (isChasingNow == false)
        {
            enemy.NavMesh.SetDestination(playerTrs.position);
        }
        else
        {
            Vector3 dir = (enemyTrs.position - playerTrs.position).normalized;
            Vector3 retreatPos = playerTrs.position + dir * stopDistance;
            enemy.transform.LookAt(playerTrs);
            enemy.NavMesh.SetDestination(retreatPos);
        }

    }

    private bool chase()
    {
        if (enemy.EnemyStop && enemy.EnemyAttackState.CanEnter)
        {
            enemy.EnemyStop = false;
        }

        if (enemy.EnemyStop)
        {
            return true;
        }

        float dis = Vector3.Distance(playerTrs.position, enemyTrs.position);
        if (dis <= stopDistance)
        {
            if (enemy.EnemyAttackState.CanEnter)
            {
                enemy.StateMachine.ChangeState(enemy.EnemyAttackState);
                enemy.EnemyStop = true;
            }

            return true;
        }
        else
        {
            return false;
        }


    }
    public void Exit()
    {
        wasChasing = false;
    }
}
