using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    private Enemy enemy;
    private BoxCollider box;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (enemy.IsDead) return;
            other.GetComponent<Unit>().TakeDamge(enemy.Damage);
        }
    }

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        box = GetComponent<BoxCollider>();
        box.enabled = false;
    }


    //애니메이션 이벤트추가용 

    private void AttackStart()
    {
        enemy.EnemyAttackStart();
    }
    private void AttackEnd()
    {
        enemy.EnemyAttackEnd();
    }

    private void DeathEnd()
    {
        enemy.EnemyDeath();
    }

}
