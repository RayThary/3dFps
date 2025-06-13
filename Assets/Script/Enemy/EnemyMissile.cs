using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{

    private Enemy enemy;

    [SerializeField]private float missileSpeed;

    public void SetEnemy(Enemy _enemy)
    {
        enemy = _enemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<Unit>().TakeDamge(enemy.Damage);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * missileSpeed;
    }
}
