using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeState : IEnemyState
{

    private Enemy enemy;

    public bool CanEnter { get; set; } = true;

    public EnemyMeleeState(Enemy _enemy)
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
        enemy.EnemyStop = false;
    }

}
