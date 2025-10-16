using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoomState : IEnemyState
{
    private Enemy enemy;

    public bool CanEnter { get; set; } = true;

    public EnemyBoomState(Enemy _enemy)
    {
        enemy = _enemy;
    }
    public void Enter()
    {
        enemy.Animator.SetTrigger("Attack");
    }

    public void Update()
    {
    }


    public void Exit()
    {

    }


}
