using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    public enum eEnemyCategory
    {
        Melee,
        Charger,
        Ranger,
        Sniper,
        Boom,
        Boss,
        BossBoom,
    }
    [SerializeField] private eEnemyCategory enemyCategory;
    public eEnemyCategory Type { get { return enemyCategory; } set { enemyCategory = value; } }

    [SerializeField] private EnemyData enemyData;

    [SerializeField] private float roamRadius;
    [SerializeField] private LayerMask obstacleMask;

    private Transform playerTrs;

    [SerializeField] private float hp;
    public float Hp { get { return hp; } }
    [SerializeField] private float speed;
    private float damage;
    public float Damage { get { return damage; } }
    //인터페이스용 Stop
    private bool enemyStop = false;
    public bool EnemyStop { get { return enemyStop; } set { enemyStop = value; } }

    private bool isDead = false;
    public bool IsDead { get { return isDead; } }
    private bool deathCleanupDone = false;
    private Coroutine deathFailSafeCoroutine;
    private const float deathFailSafeDelay = 3f;

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
    public BoxCollider BoxCollider { get { return box; } set { box = value; } }

    [HideInInspector][SerializeField] private BoxCollider unitHitBox;
    public BoxCollider UnitHitBox { get { return unitHitBox; } set { unitHitBox = value; } }

    private LineRenderer lineR;

    [HideInInspector][SerializeField] private Transform missilePort1;
    public Transform MissilePort1 { get { return missilePort1; } set { missilePort1 = value; } }
    [HideInInspector][SerializeField] private Transform missilePort2;
    public Transform MissilePort2 { get { return missilePort2; } set { missilePort2 = value; } }

    [HideInInspector][SerializeField] private Transform rockPoint;
    public Transform RockPoint { get { return rockPoint; } set { rockPoint = value; } }
    private int currentWallCount;
    public int WallCount { get { return currentWallCount; } set { currentWallCount = value; } }

    private bool hitCheck = false;
    public bool HitCheck { get { return hitCheck; } set { hitCheck = value; } }

    private void OnDrawGizmosSelected()
    {
        if (enemyData == null) return;

        // 인식 범위(추격 시작 거리)
        float chase = enemyData.chaseDistance;

        Gizmos.DrawWireSphere(transform.position, chase);
    }

    private void OnEnable()
    {
        deathCleanupDone = false;
        isDead = false;
        isStarted = false;
        enemyStop = false;
        hitCheck = false;
        if (lineR != null) lineR.enabled = false;

        if (enemyCategory == eEnemyCategory.Boss)
        {
            EnemyBossAttack bossAttack = GetComponentInChildren<EnemyBossAttack>();
            bossAttack.SetUp(this, missilePort1, missilePort2, rockPoint);
        }
    }
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        box = GetComponent<BoxCollider>();
        playerTrs = GameManager.instance.GetUnit.GetComponent<Transform>();

        SetUpStat();
        stateMachine = new EnemyStateMachine();
    }
    public void SetUpStat()
    {
        int stage = GameManager.instance.GetStageNum;
        if (stage == 0) stage = 1;

        damage = enemyData.Damage;

        float hpMultiplier = 1f + (stage - 1) * 0.12f;
        hp = enemyData.Hp * hpMultiplier;

        float value = Random.Range(0.95f, 1.1f);
        speed = enemyData.Speed * value;
    }
    private void SetupEnemyState()
    {
        switch (enemyCategory)
        {
            case eEnemyCategory.Melee:
                enemyChaseState = new EnemyMeleeChaseState(this, playerTrs, transform, obstacleMask, roamRadius, speed, enemyData);
                enemyAttackState = new EnemyMeleeState(this);
                return;

            case eEnemyCategory.Charger:
                enemyChaseState = new EnemyChargerChaseState(this, playerTrs, transform, obstacleMask, speed, roamRadius, enemyData);
                enemyAttackState = new EnemyChargerState(this, transform, playerTrs, unitHitBox, speed);

                return;

            case eEnemyCategory.Ranger:
                enemyChaseState = new EnemyRangerChaseState(this, playerTrs, transform, obstacleMask, roamRadius, speed, enemyData);
                enemyAttackState = new EnemyRangerState(this);
                return;

            case eEnemyCategory.Sniper:
                lineR = GetComponentInChildren<LineRenderer>();
                lineR.enabled = false;

                enemyChaseState = new EnemyRangerChaseState(this, playerTrs, transform, obstacleMask, roamRadius, speed, enemyData);
                enemyAttackState = new EnemySniperState(this, lineR.transform, playerTrs, lineR, damage, obstacleMask);
                return;

            case eEnemyCategory.BossBoom:
            case eEnemyCategory.Boom:
                enemyChaseState = new EnemyBoomChaseState(this, playerTrs, transform, speed, obstacleMask, enemyData);
                enemyAttackState = new EnemyBoomState(this);
                return;

            case eEnemyCategory.Boss:
                enemyChaseState = new EnemyBossState(this, missilePort1, missilePort2);
                return;

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!isStarted && GameManager.instance.IsStageStarted)
        {
            isStarted = true;
            SetupEnemyState();
            stateMachine.ChangeState(enemyChaseState);
        }

        if (!isDead && isStarted)
        {
            stateMachine.Update();
        }
    }


    public void HitEnemy(float _damage, float _criticalDamage, bool _hitDamage)
    {
        if (isDead)
        {
            return;
        }

        hitCheck = true;
        if (_hitDamage)
        {
            hp -= _damage * _criticalDamage;
        }
        else
        {
            hp -= _damage;
        }

        SoundManager.instance.HitSFXCreate(SoundManager.Clips.CriticalHit, 1, GameManager.instance.WeaponSoundParent, _hitDamage);

        if (hp <= 0)
        {
            animator.SetTrigger("Death");
            animator.speed = 1;
            if (enemyCategory != eEnemyCategory.BossBoom && enemyCategory != eEnemyCategory.Boss)
            {
                GameManager.instance.AddKillCount();
            }
            box.enabled = false;
            isDead = true;
            isStarted = false;
            hitCheck = false;
            enemyStop = false;
            if (unitHitBox != null) unitHitBox.enabled = false;
            if (lineR != null) lineR.enabled = false;
            StopAllCoroutines();
            StartDeathFailSafe();

            if (enemyCategory == eEnemyCategory.Boss)
            {
                GameManager.instance.Portal.SetActive(true);
            }
        }
    }

    private void StartDeathFailSafe()
    {
        if (deathFailSafeCoroutine != null)
        {
            StopCoroutine(deathFailSafeCoroutine);
        }
        deathFailSafeCoroutine = StartCoroutine(DeathFailSafe());
    }

    private IEnumerator DeathFailSafe()
    {
        yield return new WaitForSeconds(deathFailSafeDelay);

        if (isDead && !deathCleanupDone && gameObject.activeInHierarchy)
        {
            EnemyDeath();
        }
    }

    /// <summary>
    /// 슬로우
    /// </summary>
    /// <param name="_slowSpeed">속도감소 배율</param>
    /// <param name="_slowTime">속도감속 시간</param>
    public void SlowEenemy(float _slowSpeed, float _slowTime)
    {
        StartCoroutine(slowEnemy(_slowSpeed, _slowTime));
    }

    private IEnumerator slowEnemy(float _slowSpeed, float _slowTime)
    {
        float basicSpeed = speed;
        speed *= _slowSpeed;
        yield return new WaitForSeconds(_slowTime);
        speed = basicSpeed;
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
        obj.GetComponent<EnemyMissile>().SetDamage(damage);
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

    public void EnemyBoomAttack(PoolingManager.ePoolingObject _poolingObject)
    {
        GameObject obj = PoolingManager.Instance.CreateObject(_poolingObject, GameManager.instance.PoolingParents[_poolingObject.ToString()]);
        obj.transform.position = transform.position;
        hp = enemyData.Hp;
        stateMachine.ChangeState(enemyChaseState);
        box.enabled = true;
        PoolingManager.Instance.RemovePoolingObject(gameObject);
    }

    public void EnemyBossSkillTime()
    {
        enemyChaseState.CanEnter = true;
    }



    public void EnemyDeath()
    {
        if (deathCleanupDone)
        {
            return;
        }
        deathCleanupDone = true;
        if (deathFailSafeCoroutine != null)
        {
            StopCoroutine(deathFailSafeCoroutine);
            deathFailSafeCoroutine = null;
        }
        StopAllCoroutines();
        if (lineR != null) lineR.enabled = false;

        animator.SetTrigger("Reset");

        box.enabled = true;
        enemyStop = false;
        hp = enemyData.Hp;
        isDead = false;
        stateMachine.ChangeState(enemyChaseState);

        itemDrop();

        if (unitHitBox != null) unitHitBox.enabled = false;
        PoolingManager.Instance.RemovePoolingObject(gameObject);
    }

    private void itemDrop()
    {
        if (enemyCategory == eEnemyCategory.BossBoom)
        {
            item(PoolingManager.ePoolingObject.ItemAmmo, 0);
            return;
        }

        int goldDropChance = Random.Range(0, 10);
        if (goldDropChance < 6)
            item(PoolingManager.ePoolingObject.ItemCoin, 0);


        int ammoDropChance = Random.Range(0, 10);
        if (ammoDropChance < 6)
            item(PoolingManager.ePoolingObject.ItemAmmo, 0);



        int hpDropChance = Random.Range(0, 100);
        if (hpDropChance < 5)
            item(PoolingManager.ePoolingObject.ItemHp, 1);

    }

    private void item(PoolingManager.ePoolingObject _item, int dropCount)
    {
        if (dropCount == 0)
            dropCount = Random.Range(1, 4);

        float randY = Random.Range(3, 3.5f);
        for (int i = 0; i < dropCount; i++)
        {
            GameObject obj = PoolingManager.Instance.CreateObject(_item, GameManager.instance.PoolingParents[_item.ToString()]);
            Vector3 objTargerVector = transform.position;
            objTargerVector.y += randY;
            obj.transform.position = objTargerVector;
        }
    }



}
