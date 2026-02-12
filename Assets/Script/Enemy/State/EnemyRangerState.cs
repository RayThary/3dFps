using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangerState : IEnemyState
{

    private Enemy enemy;



    //ÄðÅ¸ÀÓ 
    private float rangeCooltime = 3;
    private float lastUesdTime = 0;
    public bool CanEnter  
    { get { return Time.time >= lastUesdTime + rangeCooltime; } set { CanEnter = value; } }

    public EnemyRangerState(Enemy _enemy)
    {
        enemy = _enemy;
        lastUesdTime = -rangeCooltime;
    }

    public void Enter()
    {
        lastUesdTime = Time.time;
        enemy.Animator.SetTrigger("Attack");
    }

    public void Update()
    {

    }

    public void Exit()
    {
        lastUesdTime = Time.time;
        enemy.EnemyStop = false;
    }

}
