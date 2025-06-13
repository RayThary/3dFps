using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangerAttack : MonoBehaviour
{
    private Enemy enemy;
    [SerializeField] private Transform RangerTransform;
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }


    //애니메이션
    private void attackStart()
    {
        enemy.EnemyRangerAttackStart(PoolingManager.ePoolingObject.Missile, RangerTransform);
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
