using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    [SerializeField]
    private float hp;
    private float speed;
    private float damage;

    private void Awake()
    {
        hp = enemyData.Hp;
        speed = enemyData.Speed;
        damage = enemyData.Damage;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void HitEnemy(float _damage, bool _hitDamage)
    {
        if (_hitDamage)
        {
            hp -= _damage * 1.5f;
        }
        else
        {
            hp -= _damage;
        }
    }
}
