using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossState : IEnemyState
{
    private Enemy enemy;
    private Transform missilePort1;
    private Transform missilePort2;

    private float lastUesdTime;
    public bool CanEnter => true;

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
        if (lastUesdTime + 2 < Time.time)
        {
            attack();
        }
    }
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
        lastUesdTime = Time.time + 2;
    }
    private void rockPatten()
    {
        enemy.Animator.SetTrigger("Jump");
        lastUesdTime = Time.time;
    }
    private void movePatten()
    {
        enemy.Animator.SetTrigger("Move");
        lastUesdTime = Time.time + 5;
    }


    public void Exit()
    {

    }

}
