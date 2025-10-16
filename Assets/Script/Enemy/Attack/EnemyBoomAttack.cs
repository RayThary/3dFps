using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoomAttack : MonoBehaviour
{
    private Enemy enemy;
    [SerializeField]private BoxCollider box;
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
        box = GetComponent<BoxCollider>();
        enemy = GetComponentInParent<Enemy>();

        box.enabled = false;
    }

    void Update()
    {

    }
    private void attackStart()
    {
        box.enabled = true;
        Invoke("attackBoom", 0.1f);
        
    }
    private void attackBoom()
    {
        enemy.EnemyBoomAttack(PoolingManager.ePoolingObject.EnemyBoom);
    }

    private void enemyDeath()
    {
        enemy.EnemyDeath();
        box.enabled = false;
    }
}
