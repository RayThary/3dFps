using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoomChaseState : IEnemyState
{
    private Enemy enemy;
    private Transform playerTrs;
    private Transform enemyTrs;
    private float speed;
    private float stopDistance;
    private LayerMask obstacleMask;


    public bool CanEnter { get; set; } = true;
    public EnemyBoomChaseState(Enemy _enemy, Transform _playerTrs, Transform _enemyTrs,
     LayerMask _obstacleMask, EnemyData _enemyData)
    {
        enemy = _enemy;
        enemyTrs = _enemyTrs;
        playerTrs = _playerTrs;

        speed = _enemyData.Speed;
        stopDistance = _enemyData.AttackStopRange;
        obstacleMask = _obstacleMask;
    }

    public void Enter()
    {
        enemy.NavMesh.speed = speed;
        if (enemy.NavMesh.isOnNavMesh)
            enemy.NavMesh.ResetPath();

    }

    public void Update()
    {
        float dis = Vector3.Distance(playerTrs.position, enemyTrs.position);
        if (dis >= stopDistance)
        {
            enemy.NavMesh.SetDestination(playerTrs.position);
        }
        else
        {
            enemy.StateMachine.ChangeState(enemy.EnemyAttackState);
            enemy.BoxCollider.enabled = false;
            enemy.EnemyStop = true;
        }
    }
    public void Exit()
    {
    }

}
