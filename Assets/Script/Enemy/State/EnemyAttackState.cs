using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{

    private Enemy enemy;
    private float speed;

    public EnemyAttackState(Enemy _enemy, float _speed)
    {
        enemy = _enemy;
        speed = _speed;
    }
    public void Enter()
    {
        //enemy.Animator.SetBool("Attack", true);
        enemy.Animator.SetTrigger("Att");
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        enemy.EnemyStop = false;
    }

}
