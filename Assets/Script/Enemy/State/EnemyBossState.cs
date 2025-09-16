using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossState : IEnemyState
{
    private Enemy enemy;
    private Transform missilePort1;
    private Transform missilePort2;

    private float lastUesdTime;

    private float delayTime = 2;

    public bool CanEnter { get; set; } = true; 

    public EnemyBossState(Enemy _enemy, Transform _port1, Transform _port2)
    {
        enemy = _enemy;
        missilePort1 = _port1;
        missilePort2 = _port2;
    }

    public void Enter()
    {
        movePatten();
        lastUesdTime = Time.time;
    }

    public void Update()
    {
        if (lastUesdTime + delayTime < Time.time )
        {
            attack();
        }

        if (CanEnter)
        {
            lastUesdTime = Time.time;
            CanEnter = false;
        }
    }
    //임시코드
    private void attack()
    {
        int a = Random.Range(0, 2);
        switch (a)
        {
            case 0:
                missilePatten(); break;
            case 1:
                rockPatten(); break;
        }
    }

    private void missilePatten()
    {
        enemy.Animator.SetTrigger("Missile");
        delay();
    }
    private void rockPatten()
    {
        enemy.Animator.SetTrigger("Jump");
        delay();
    }
    private void movePatten()
    {
        enemy.Animator.SetTrigger("Move");
        delay();
    }

    private void delay()
    {
        delayTime = Random.Range(2f, 5f);
        lastUesdTime = Mathf.Infinity;
    }

    public void Exit()
    {

    }

}
