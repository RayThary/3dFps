using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySniperAttack : MonoBehaviour
{
    private Enemy enemy;
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }


    //애니메이션
    private void attackStart()
    {
        enemy.EnemySniperAttack();
    }

    private void attackEnd()
    {
        enemy.EnemyRangerAttackEnd();
    }
    private void enemyDeath()
    {
        enemy.EnemyDeath();
    }
}
