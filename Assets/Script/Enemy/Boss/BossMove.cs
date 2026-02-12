using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{

    private Enemy enemy;
    private bool bossDown;
    private bool bossUp;
    [SerializeField] private float moveSpeed;

    private Transform nextTrs;
    [SerializeField] private List<Transform> movePoint = new List<Transform>();

    private Vector3 center = new Vector3(0, 0, -55);
    public void startMoving(Vector3 _nowVec)
    {
        bossDown = true;
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

    private IEnumerator bossMoving()
    {
        while (bossDown)
        {
            transform.position += Vector3.down * Time.deltaTime * moveSpeed;
            yield return null;
            if (transform.position.y <= -10)
            {
                bossDown = false;
                bossUp = true;
                transform.position = new Vector3(nextTrs.position.x, -10, nextTrs.position.z);

            }
        }
        spawnBossBoom(5);
        yield return new WaitForSeconds(0.5f);

        Vector3 unitVec = GameManager.instance.GetUnit.transform.position;
        unitVec.y = 0;
        transform.LookAt(unitVec);

        while (bossUp)
        {
            transform.position += Vector3.up * Time.deltaTime * (moveSpeed / 2);
            yield return null;
            if (transform.position.y >= nextTrs.position.y)
            {
                bossUp = false;
            }
        }
        enemy.EnemyBossSkillTime();
    }

    private void spawnBossBoom(int count)
    {
        Vector3 dir = (center - nextTrs.position).normalized;
        Vector3 spawnPoint = nextTrs.position + dir * 0.2f;
        for (int i = 0; i < count; i++)
        {

            float angle = i * (360f / count);

            float radius = Random.Range(0.8f, 1.2f);

            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad) * radius, 0f, Mathf.Sin(rad) * radius);

            Vector3 spawnPos = spawnPoint + offset;

            GameObject obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.EnemyBossBoom, GameManager.instance.PoolingParents["EnemyBossBoom"]);
            obj.transform.position = spawnPos;
        }

    }

}
