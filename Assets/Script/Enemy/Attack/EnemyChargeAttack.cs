using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChargeAttack : MonoBehaviour
{
    private Enemy enemy;
    private BoxCollider box;

    [SerializeField]private bool hitCheck = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (enemy.IsDead || hitCheck) return;
            hitCheck = true;
            other.GetComponent<Unit>().TakeDamge(enemy.Damage);
        }
    }

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        box = GetComponent<BoxCollider>();
        box.enabled = false;
    }

    private void attackEnd()
    {
        enemy.EnemyChargeAttackEnd();
        hitCheck = false;
    }

    //애니메이션 이벤트추가용 
    private void DeathEnd()
    {
        enemy.EnemyDeath();
    }
}
