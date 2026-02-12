using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBossState : IEnemyState
{
    private Enemy enemy;
    private Transform missilePort1;
    private Transform missilePort2;

    private float lastUesdTime;
    private float delayTime = 2;

    private bool skillStop = false;


    private int skillCount = 0;

    public bool CanEnter { get; set; } = true;

    private float hp;
    public EnemyBossState(Enemy _enemy, Transform _port1, Transform _port2)
    {
        enemy = _enemy;
        missilePort1 = _port1;
        missilePort2 = _port2;
    }

    public void Enter()
    {
        hp = enemy.Hp;
        skillStop = true;
        lastUesdTime = Time.time;
    }

    public void Update()
    {        
        if (!skillStop)
        {

            Vector3 unitVec = GameManager.instance.GetUnit.transform.position;
            unitVec.y = 0;
            Quaternion targetRot = Quaternion.LookRotation(unitVec - enemy.transform.position);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRot, Time.deltaTime * 2);
        }

        if (CanEnter && skillStop)
        {
            lastUesdTime = Time.time;
            skillStop = false;
            CanEnter = false;
        }

        if (lastUesdTime + delayTime < Time.time)
        {
            attack();
        }
    }

    private void attack()
    {
        bool half = (enemy.Hp / hp) > 0.5f;
        int nextPatten = getNextPatten(half);

        switch (nextPatten)
        {
            case 0:
                missilePatten(); break;
            case 1:
                wallPatten(); break;
            case 2:
                rockPatten(); break;
            case 3:
                laserPatten(); break;
            case 4:
                movePatten(); break;
        }
        CanEnter = false;
        skillStop = true;
    }

    private int getNextPatten(bool _hpHalf)
    {
        if (skillCount == 0)
            return 1;

        int missileChance = 30;
        int jumpChance = 15;
        int rockChance = 25;
        int laserChance = 15;
        int moveChance = 0;

        if (!_hpHalf)
        {
            laserChance += 5;
            rockChance += 5;
        }


        if (skillCount >= 8) moveChance = 70;
        else if (skillCount >= 6) moveChance = 40;
        else if (skillCount >= 4) moveChance = 20;


        //벽기준 패턴
        if (enemy.WallCount <= 2)
        {
            laserChance = 8;   // 패널티성으로만 등장
            jumpChance = 20;  // 다시 깔 기회는 줌
        }
        if (enemy.WallCount > 12)
        {
            laserChance = Mathf.Max(missileChance, rockChance, jumpChance) + 10;
            jumpChance = 5;   // 더 이상 쌓이지 않게
        }
        if (enemy.WallCount > 14)
        {
            jumpChance = 0;
        }


        if (enemy.WallCount < 2) laserChance = 10;

        if (enemy.WallCount >= 12)
        {
            laserChance += 20;
            jumpChance = 10;
        }

        if (enemy.WallCount > 14) jumpChance = 0;

        int total = missileChance + jumpChance + rockChance + laserChance + moveChance;
        int roll = Random.Range(0, total);

        if (roll < missileChance) return 0;
        roll -= missileChance;

        if (roll < jumpChance) return 1;
        roll -= jumpChance;

        if (roll < rockChance) return 2;
        roll -= rockChance;

        if (roll < laserChance) return 3;
        roll -= laserChance;

        return 4;
    }

    private void laserPatten()
    {
        enemy.Animator.SetTrigger("Laser");
        delay();
    }

    private void missilePatten()
    {
        enemy.Animator.SetTrigger("Missile");
        delay();
    }
    private void wallPatten()
    {
        enemy.Animator.SetTrigger("Jump");
        delay();
    }
    private void rockPatten()
    {
        enemy.Animator.SetTrigger("Rock");
        delay();
    }
    private void movePatten()
    {
        enemy.Animator.SetTrigger("Move");
        skillCount = 0;
        delay();
    }


    private void delay()
    {
        float hpRatio = enemy.Hp / hp;
        if (hpRatio > 0.7f)
        {
            delayTime = Random.Range(3f, 4f);
        }
        else if (hpRatio > 0.35f)
        {
            delayTime = Random.Range(2f, 3.5f);
        }
        else
        {
            delayTime = Random.Range(1.5f, 3f);
        }
        skillCount++;
        lastUesdTime = Mathf.Infinity;
    }

    public void Exit()
    {

    }

}
