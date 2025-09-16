using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyRangerChaseState : IEnemyState
{
    private Enemy enemy;
    private Transform playerTrs;
    private Transform enemyTrs;
    private float speed;
    private float stopDistance;//공격하기전멈출거리
    private float chaseDistance;//추격거리

    private float roamRadius;
    private LayerMask obstacleMask;
    private Vector3 targetVec;
    private Vector3 lastTargetVec;

    private bool wasChasing = false;
    public bool CanEnter { get; set; } = true;

    public EnemyRangerChaseState(Enemy _enemy, Transform _playerTrs, Transform _enemyTrs,
         LayerMask _obstacleMask, float _roamRadius, EnemyData _enemyData)
    {
        enemy = _enemy;
        enemyTrs = _enemyTrs;
        playerTrs = _playerTrs;

        obstacleMask = _obstacleMask;
        roamRadius = _roamRadius;

        speed = _enemyData.Speed;
        stopDistance = _enemyData.AttackStopRange;
        chaseDistance = _enemyData.chaseDistance;
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

    private void RoamTarget()
    {
        float dis = Vector3.Distance(enemyTrs.position, targetVec);
        if (dis > 1.25f)
        {
            enemy.NavMesh.SetDestination(targetVec);

            return;
        }
        else
        {
            nextPoint();
        }
    }

    private void nextPoint()
    {
        for (int i = 0; i < 20; i++)
        {
            if (i == 19)
            {
                Debug.Log("추격실패");
                enemy.NavMesh.SetDestination(targetVec);
                break;
            }
            Vector3 tempPoint = targetPoint(5);

            NavMeshHit navHit;
            if (!NavMesh.SamplePosition(tempPoint, out navHit, roamRadius, NavMesh.AllAreas))
            {
                continue;
            }

            tempPoint = navHit.position;

            float checkRadius = enemy.NavMesh.radius * 1.1f;
            if (Physics.CheckSphere(tempPoint, checkRadius, obstacleMask))
            {
                continue;
            }

            NavMeshPath path = new NavMeshPath();
            enemy.NavMesh.CalculatePath(tempPoint, path);
            if (path.status != NavMeshPathStatus.PathComplete)
            {
                continue;
            }
            lastTargetVec = targetVec;
            targetVec = tempPoint;
            enemy.NavMesh.SetDestination(targetVec);
            break;
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
