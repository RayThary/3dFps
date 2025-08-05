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
        Sniper,
        Boss,
    }
    [SerializeField] private eEnemyType enemyType;
    [SerializeField] private EnemyData enemyData;

    [SerializeField] private float roamRadius;
    [SerializeField] private LayerMask obstacleMask;

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

    private bool isStarted = false;

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
    [Tooltip("근거리만 필요함")][SerializeField] private BoxCollider unitHitBox;
    private LineRenderer lineR;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        box = GetComponent<BoxCollider>();
        playerTrs = GameManager.instance.GetUnit.GetComponent<Transform>();

        SetUpStat();

        speed = enemyData.Speed;
        stopDistance = enemyData.AttackStopRange;

        stateMachine = new EnemyStateMachine();


    }
    public void SetUpStat()
    {
        int stage = GameManager.instance.GetStageNum;
        if (stage == 0) stage = 1;

        float damageMultiplier = 1 + (stage - 1) * 0.4f;
        float hpMultiplier = 1 + (stage - 1) * 0.25f;
        damage = enemyData.Damage + damageMultiplier;
        hp = enemyData.Hp + hpMultiplier;
    }
    private void SetupState()
    {
        switch (enemyType)
        {
            case eEnemyType.Melee:
                enemyChaseState = new EnemyMeleeChaseState(this, playerTrs, transform, obstacleMask, roamRadius, enemyData);
                enemyAttackState = new EnemyMeleeState(this, speed);
                return;

            case eEnemyType.Charger:
                enemyChaseState = new EnemyMeleeChaseState(this, playerTrs, transform, obstacleMask, roamRadius, enemyData);
                enemyAttackState = new EnemyChargerState(this, transform, playerTrs, unitHitBox, speed);

                return;

            case eEnemyType.Ranger:
                enemyChaseState = new EnemyRangerChaseState(this, playerTrs, transform, obstacleMask, roamRadius, enemyData);
                enemyAttackState = new EnemyRangerState(this);
                return;
            case eEnemyType.Sniper:
                lineR = GetComponentInChildren<LineRenderer>();
                lineR.enabled = false;

                enemyChaseState = new EnemyRangerChaseState(this, playerTrs, transform, obstacleMask, roamRadius, enemyData);
                enemyAttackState = new EnemySniperState(this, lineR.transform, playerTrs, lineR, damage);
                return;

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!isStarted && GameManager.instance.IsStageStarted)
        {
            isStarted = true;
            SetupState();
            stateMachine.ChangeState(enemyChaseState);
        }

        if (!isDead && isStarted)
        {
            stateMachine.Update();
        }
    }


    public void HitEnemy(float _damage, float _criticalDamage, bool _hitDamage)
    {
        if (_hitDamage)
        {
            hp -= _damage * _criticalDamage;
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
            isStarted = false;
        }
    }

    //애니메이션 부분
    public void EnemyMeleeAttackStart()
    {
        unitHitBox.enabled = true;
    }

    public void EnemyMeleeAttackEnd()
    {
        unitHitBox.enabled = false;
        StateMachine.ChangeState(enemyChaseState);
    }
    public void EnemyChargeAttackEnd()
    {
        unitHitBox.enabled = false;
        stateMachine.ChangeState(enemyChaseState);
    }

    public void EnemyRangerAttackStart(PoolingManager.ePoolingObject _poolingObject, Transform _RangerTrs)
    {
        GameObject obj = PoolingManager.Instance.CreateObject(_poolingObject, GameManager.instance.PoolingParents[_poolingObject.ToString()]);
        obj.transform.localRotation = transform.rotation;
        obj.transform.position = _RangerTrs.position;
        obj.GetComponent<EnemyMissile>().SetEnemy(this);
    }

    public void EnemyRangerAttackEnd()
    {
        StateMachine.ChangeState(enemyChaseState);
    }

    public void EnemySniperAttack()
    {
        if (enemyAttackState is IEnemySniperState sniper)
        {
            sniper.SniperShot = true;
            animator.speed = 0;
        }
    }


    public void EnemyDeath()
    {
        animator.SetTrigger("Reset");

        box.enabled = true;
        enemyStop = false;
        hp = enemyData.Hp;
        isDead = false;
        stateMachine.ChangeState(enemyChaseState);

        if (unitHitBox != null) unitHitBox.enabled = false;
        PoolingManager.Instance.RemovePoolingObject(gameObject);
    }
}
