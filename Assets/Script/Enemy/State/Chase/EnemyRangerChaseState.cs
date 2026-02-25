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

    private bool isAggro = false;
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
        enemy.NavMesh.updateRotation = true;
        nextPoint();
        lastTargetVec = targetVec;
    }


    public void Update()
    {
        if (enemy.EnemyStop)
        {
            return;
        }
        chaseCheck();
        if (isAggro)
        {
            bool isChasingNow = chase();

            if (isChasingNow)
            {
                bool isShotCheck = rayCheck();

                if (isShotCheck)
                {

                    if (enemy.EnemyAttackState.CanEnter)
                    {
                        enemy.StateMachine.ChangeState(enemy.EnemyAttackState);
                        enemy.EnemyStop = true;

                        //공격중 회전방지
                        enemy.NavMesh.updateRotation = false;
                    }
                }
                else
                {
                    enemy.NavMesh.SetDestination(playerTrs.position);
                }
            }
            else
            {

                float dis = Vector3.Distance(enemyTrs.position, playerTrs.position);
                bool isDeadZone = Mathf.Abs(dis - stopDistance) <= 0.5f;
                bool canShotPos = rayCheck();
                //사격가능한지 확인및 데드존체크
                if (isDeadZone)
                {
                    if (canShotPos)
                    {
                        enemy.NavMesh.ResetPath();
                        enemy.Animator.SetBool("Idle", true);
                        return;
                    }
                    else
                    {
                        enemy.NavMesh.SetDestination(playerTrs.position);
                        enemy.Animator.SetBool("Idle", false);
                        return;
                    }
                }

                enemy.Animator.SetBool("Idle", false);

                if (dis > stopDistance)
                {
                    enemy.NavMesh.SetDestination(playerTrs.position);
                }
                else
                {
                    //뒤로가기
                    Vector3 dir = (enemyTrs.position - playerTrs.position).normalized;
                    Vector3 retreatPos = playerTrs.position + dir * stopDistance;
                    enemy.transform.LookAt(playerTrs);
                    enemy.NavMesh.SetDestination(retreatPos);
                }
            }
        }
        else
        {
            RoamTarget();
        }

    }



    private void chaseCheck()
    {
        if (isAggro)
        {
            return;
        }
        bool checkChase = Vector3.Distance(enemyTrs.position, playerTrs.position) < chaseDistance;
        if (checkChase)
        {
            isAggro = true;
        }
    }

    private bool chase()
    {


        float dis = Vector3.Distance(playerTrs.position, enemyTrs.position);
        if (dis <= stopDistance)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private bool rayCheck()
    {

        bool frontCheck = frontBlocked();
        if (frontCheck)
        {
            return false;
        }

        Vector3 origin = enemyTrs.position + Vector3.up * 1.5f;   // 눈 높이
        Vector3 target = playerTrs.position + Vector3.up * 1.0f;  // 플레이어 중심
        Vector3 dir = (target - origin).normalized;

        int mask = obstacleMask | LayerMask.GetMask("Player");

        // 플레이어가 바로 맞으면 공격 가능
        if (Physics.Raycast(origin, dir, out RaycastHit hit, stopDistance, mask))
        {
            if (hit.transform == playerTrs)
            {
                return true;
            }
        }

        return false;
    }

    private bool frontBlocked()
    {
        Vector3 center = enemyTrs.position + enemyTrs.forward * 2 + Vector3.up * 1.5f;

        Vector3 halfExtents = new Vector3(3f, 3f, 1f);

        return Physics.CheckBox(center, halfExtents, enemyTrs.rotation, obstacleMask);
    }
    private void RoamTarget()
    {
        float dis = Vector3.Distance(enemyTrs.position, targetVec);
        if (dis > enemy.NavMesh.stoppingDistance + 0.3f)
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
                enemy.NavMesh.SetDestination(targetVec);
                break;
            }
            Vector3 tempPoint = targetPoint(10);

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
    }
}
