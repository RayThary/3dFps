using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangerState : IEnemyState
{

    private Enemy enemy;



    //쿨타임 
    private float rangeCooltime = 3;
    private float lastUesdTime = 0;
    public bool CanEnter => Time.time >= lastUesdTime + rangeCooltime;

    //밥먹기전적음 추적은 navmesh를받아서 속도를한순간올려주고 exit나갈때 줄여주는걸로 update로 목표까지갔다면 return또는 시간만큼추적으로고민중
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
