using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{

    private Enemy enemy;
    private bool bossDown;
    private bool bossUp;
    [SerializeField] private float moveSpeed;
    private Vector3 backupPoint;
    private Vector3 targetPoint;

    private Transform enemyTrs;
    private Transform nextTrs;
    [SerializeField] private List<Transform> movePoint = new List<Transform>();
    public void SetUp(Vector3 _nowVec)
    {
        bossDown = true;
        backupPoint = _nowVec;
        targetPoint = _nowVec;
        targetPoint.y = -10;
        int num = Random.Range(0, movePoint.Count);
        while (_nowVec == movePoint[num].position)
        {
            num = Random.Range(0, movePoint.Count);
        }
        nextTrs = movePoint[num];
        StartCoroutine(bossMoving());
    }

    private void Awake()
    {
        //movpoints는 어웨이크에서정해주기때문에 스타트에서넣어주기
        enemy = GetComponent<Enemy>();
    }
    void Start()
    {
        BossMovePoints bMovePoints = FindAnyObjectByType<BossMovePoints>();
        movePoint = bMovePoints.GetPoint;

    }

    void Update()
    {
        //if (bossDown)
        //{
        //    transform.position += Vector3.down * Time.deltaTime * moveSpeed;
        //    if (transform.position.y <= -10)
        //    {
        //        bossDown = false;
        //        targetPoint.y = backupPoint.y;
        //    }
        //}

        //if (bossUp)
        //{
        //    transform.position += Vector3.up * Time.deltaTime * (moveSpeed / 2);
        //    if (transform.position.y >= targetPoint.y)
        //    {
        //        bossUp = false;
        //    }
        //}
    }
    private IEnumerator bossMoving()
    {
        while (bossDown)
        {
            transform.position += Vector3.down * Time.deltaTime * moveSpeed;
            yield return null;
            if (transform.position.y <= -10)
            {
                bossDown = false;
                targetPoint.y = backupPoint.y;
                bossUp = true;
                transform.position = new Vector3(nextTrs.position.x, -10, nextTrs.position.z);

            }
        }

        yield return new WaitForSeconds(0.5f);

        Vector3 unitVec = GameManager.instance.GetUnit.transform.position;
        unitVec.y = 0;
        transform.LookAt(unitVec);

        while (bossUp)
        {
            transform.position += Vector3.up * Time.deltaTime * (moveSpeed / 2);
            yield return null;
            if (transform.position.y >= targetPoint.y)
            {
                bossUp = false;
            }
        }
        enemy.EnemyBossSkillTime();
    }
}
