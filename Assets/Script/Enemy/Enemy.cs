using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    public enum eEnemyType
    {
        Melee,
        Charger,
        Ranger,
        Boss,
    }
    [SerializeField] private eEnemyType enemyType;
    [SerializeField] private EnemyData enemyData;


    private Transform playerTrs;

    private float hp;
    private float speed;
    private float damage;
    public float Damage { get { return damage; } }
    private float stopDistance;
    private bool enemyStop = false;
    public bool EnemyStop { get { return enemyStop; } set { enemyStop = value; } }

    private bool isDead = false;
    public bool IsDead { get { return isDead; } }

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

    private BoxCollider box;

    [SerializeField] private BoxCollider unitHitBox;


    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        box = GetComponent<BoxCollider>();
        playerTrs = GameManager.instance.GetUnit.GetComponent<Transform>();

        hp = enemyData.Hp;
        speed = enemyData.Speed;
        damage = enemyData.Damage;
        stopDistance = enemyData.chaseStopDistance;

        stateMachine = new EnemyStateMachine();
        enemyChaseState = new EnemyChaseState(this, playerTrs, transform, speed, stopDistance);

        SetupAttackState();



        stateMachine.ChangeState(enemyChaseState);

    }
    private void SetupAttackState()
    {
        switch (enemyType)
        {
            case eEnemyType.Melee:
                enemyAttackState = new EnemyMeleeState(this, speed);
                return;

            case eEnemyType.Charger:
                enemyAttackState = new EnemyChargerState(this, transform, playerTrs, speed);

                return;

            case eEnemyType.Ranger:

                return;

        }

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"{enemyAttackState.CanEnter} , {enemyStop}");
        if (!isDead)
        {
            stateMachine.Update();
        }
        

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

        if (hp <= 0)
        {
            animator.SetTrigger("Death");
            box.enabled = false;
            isDead = true;
        }
    }

    //애니메이션 부분
    public void EnemyAttackEnd()
    {
        unitHitBox.enabled = false;
        StateMachine.ChangeState(enemyChaseState);
    }

    public void EnemyAttackStart()
    {
        unitHitBox.enabled = true;
    }

    public void EnemyDeath()
    {
        PoolingManager.Instance.RemovePoolingObject(gameObject);
        animator.SetTrigger("Reset");
        unitHitBox.enabled = false;
        box.enabled = true;
        enemyStop = false;
        hp = enemyData.Hp;
        isDead = false;
    }
}
