using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    private Transform playerTrs;
    [SerializeField]
    private float hp;
    private float speed;
    private float damage;
    public float Damage { get { return damage; } }
    private float stopDistance;
    private bool enemyStop = false;
    public bool EnemyStop { get { return enemyStop; } set { enemyStop = value; } }

    private EnemyStateMachine stateMachine;
    public EnemyStateMachine StateMachine { get { return stateMachine; } }

    private IEnemyState enemyChaseState;
    public IEnemyState EnemyChaseState { get { return enemyChaseState; } }

    private IEnemyState enemyAttackState;
    public IEnemyState EnemyAttackState { get { return enemyAttackState; } }

    private Rigidbody rigid;

    private NavMeshAgent navMesh;
    public NavMeshAgent NavMesh { get { return navMesh; } }

    private Animator animator;
    public Animator Animator { get { return animator; } }
    [SerializeField]
    private BoxCollider box;



    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        navMesh = GetComponent<NavMeshAgent>();

        playerTrs = GameManager.instance.GetUnit.GetComponent<Transform>();

        hp = enemyData.Hp;
        speed = enemyData.Speed;
        damage = enemyData.Damage;
        stopDistance = enemyData.chaseStopDistance;

        stateMachine = new EnemyStateMachine();
        enemyChaseState = new EnemyChaseState(this, playerTrs, transform, speed, stopDistance);
        enemyAttackState = new EnemyAttackState(this, speed);

        stateMachine.ChangeState(enemyChaseState);

    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();


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

    public void EnemyAttackEnd(BoxCollider _box)
    {
        //animator.SetBool("Attack", false);
        _box.enabled = false;
        StateMachine.ChangeState(enemyChaseState);
    }

    public void EnemyAttackStart(BoxCollider _box)
    {
        _box.enabled = true;
    }
}
