using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeState : IEnemyState
{

    private Enemy enemy;
    private float speed;

    public bool CanEnter => true;

    public EnemyMeleeState(Enemy _enemy, float _speed)
    {
        enemy = _enemy;
        speed = _speed;
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
