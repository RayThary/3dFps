using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Enemy enemy;
    private BoxCollider box;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<Unit>().TakeDamge(enemy.Damage);
        }
    }

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        box = GetComponent<BoxCollider>();
        box.enabled = false;
    }

    private void AttackStart()
    {
        enemy.EnemyAttackStart(box);
    }
    private void AttackEnd()
    {
        enemy.EnemyAttackEnd(box);
    }


}
