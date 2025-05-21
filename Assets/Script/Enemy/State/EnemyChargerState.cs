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
    private bool isEnd = false;

    //��Ÿ�� 
    private float chargerCooltime = 2;
    private float lastUesdTime = 0;
    public bool CanEnter => Time.time >= lastUesdTime + chargerCooltime;

    //��Ա������� ������ navmesh���޾Ƽ� �ӵ����Ѽ����÷��ְ� exit������ �ٿ��ִ°ɷ� update�� ��ǥ�������ٸ� return�Ǵ� �ð���ŭ�������ΰ����
    public EnemyChargerState(Enemy _enemy, Transform _enemyTrs, Transform _targetTrs, float _speed)
    {
        enemy = _enemy;
        enemyTrs = _enemyTrs;
        targetTrs = _targetTrs;
        speed = _speed;
        chargerSpeed = 20;
    }
    public void Enter()
    {

        enemy.NavMesh.speed = chargerSpeed;
        enemy.NavMesh.SetDestination(targetTrs.position);
        targetVec = targetTrs.position;
        lastUesdTime = Time.time;
        isEnd = false;
    }


    public void Update()
    {

        chargerAttack();
    }

    private void chargerAttack()
    {
        float dis = Vector3.Distance(enemyTrs.position, targetVec);
        if (dis <= 1 && !isEnd)
        {
            enemy.StateMachine.ChangeState(enemy.EnemyChaseState);
            isEnd = true;
        }


    }
    public void Exit()
    {
        lastUesdTime = Time.time;
        enemy.NavMesh.speed = speed;
        enemy.EnemyStop = false;
        isAttack = false;

    }
}
