using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeChaseState : IEnemyState
{
    private Enemy enemy;
    private Transform playerTrs;
    private Transform enemyTrs;
    private float speed;
    private float stopDistance;//공격하기전멈출범위
    private float chaseDistance;//추격범위
    private float roamRadius;
    private LayerMask obstacleMask;
    private Vector3 targetVec;

    private bool wasChasing = false;

    public bool CanEnter => true;

    public EnemyMeleeChaseState(Enemy _enemy, Transform _playerTrs, Transform _enemyTrs,
        LayerMask _obstacleMask,float _roamRadius, EnemyData _enemyData)
    {
        enemy = _enemy;
        enemyTrs = _enemyTrs;
        playerTrs = _playerTrs;

        speed = _enemyData.Speed;
        stopDistance = _enemyData.AttackStopRange;
        chaseDistance = _enemyData.chaseDistance;
        obstacleMask = _obstacleMask;
        roamRadius = _roamRadius;
    }

    public void Enter()
    {
        enemy.NavMesh.speed = speed;
        enemy.NavMesh.ResetPath();
        enemy.NavMesh.SetDestination(playerTrs.position);
    }


    public void Update()
    {
        bool checkChase = Vector3.Distance(enemyTrs.position, playerTrs.position) < chaseDistance;
        if (checkChase)
        {
            bool isAttackCheck = chase();

            if (isAttackCheck != wasChasing)
            {
                wasChasing = isAttackCheck;
            }

            if (isAttackCheck == false)
            {
                enemy.NavMesh.SetDestination(playerTrs.position);
            }
        }
        else
        {
            RoamTarget();
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
        if (enemy.EnemyAttackState.CanEnter && dis <= stopDistance)
        {
            enemy.StateMachine.ChangeState(enemy.EnemyAttackState);
            enemy.EnemyStop = true;
            return true;
        }
        else
        {
            return false;
        }

    }

    private void RoamTarget()
    {
        float dis = Vector3.Distance(enemyTrs.position, targetVec);
        if (dis < 0.1f)
        {
            for (int i = 0; i < 20; i++)
            {

                //Vector3 randomOffset = Random.insideUnitSphere * roamRadius;
                //randomOffset.y = 0;
                //Vector3 tempPoint = enemyTrs.position + randomOffset;
                Vector3 tempPoint = targetPoint(5);

                NavMeshHit navHit;
                if (!NavMesh.SamplePosition(tempPoint, out navHit, roamRadius, NavMesh.AllAreas))
                {
                    continue;
                }

                tempPoint = navHit.position;

                float checkRadius = enemy.NavMesh.radius * 1.1f;
                if (!Physics.CheckSphere(tempPoint, checkRadius, obstacleMask))
                {
                    continue;
                }

                NavMeshPath path = new NavMeshPath();
                enemy.NavMesh.CalculatePath(tempPoint, path);
                if (path.status != NavMeshPathStatus.PathComplete)
                {
                    continue;
                }
                targetVec = tempPoint;
                enemy.NavMesh.SetDestination(targetVec);
                break;
            }
        }
    }

    private Vector3 targetPoint(float _inner)
    {
        float inner2 = _inner * _inner;
        float outer2 = roamRadius * roamRadius;

        float pointU = Random.value;

        float r2 = pointU * (outer2 + inner2) + inner2;

        float r = Mathf.Sqrt(r2);

        float theta = Random.Range(0, Mathf.PI * 2f);


        Vector3 offset = new Vector3(Mathf.Cos(theta) * r, 0f, Mathf.Sin(theta) * r);

        //최종소환 위치
        Vector3 spawnPos = enemyTrs.position + offset;
        return spawnPos;
    }

    public void Exit()
    {
        wasChasing = false;
    }
}
