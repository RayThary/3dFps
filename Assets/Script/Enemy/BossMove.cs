using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    private bool bossDown;
    private bool bossUp;
    [SerializeField] private float moveSpeed;
    private Vector3 backupPoint;
    private Vector3 targetPoint;

    private Transform enemyTrs;
    private Transform nextTrs;
    [SerializeField]private List<Transform> movePoint = new List<Transform>();
    public void SetUp(Vector3 _nowVec)
    {
        bossDown = true;
        backupPoint = _nowVec;
        targetPoint = _nowVec;
        targetPoint.y = -10;
        int num = Random.Range(0, movePoint.Count);
        nextTrs = movePoint[num];
        StartCoroutine(bossMoving());
    }
    void Start()
    {
        BossMovePoints bMovePoints = FindAnyObjectByType<BossMovePoints>();
        movePoint = bMovePoints.GetPoint;
        Debug.Log(movePoint.Count);

    }

    void Update()
    {
        if (bossDown)
        {
            transform.position += Vector3.down * Time.deltaTime * moveSpeed;
            if (transform.position.y <= -10)
            {
                bossDown = false;
                targetPoint.y = backupPoint.y;
            }
        }

        if (bossUp)
        {
            transform.position += Vector3.up * Time.deltaTime * (moveSpeed / 2);
            if (transform.position.y >= targetPoint.y)
            {
                bossUp = false;
            }
        }
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
                transform.position =new Vector3(nextTrs.position.x, -10, nextTrs.position.z);
                
            }
        }

        yield return new WaitForSeconds(0.5f);
        
        transform.LookAt(GameManager.instance.GetUnit.transform);
        Vector3 rot = new Vector3(0, transform.eulerAngles.y, 0);
        transform.eulerAngles = rot;

        while (bossUp)
        {
            transform.position += Vector3.up * Time.deltaTime * (moveSpeed / 2);
            yield return null;
            if (transform.position.y >= targetPoint.y)
            {
                bossUp = false;
            }
        }
    }
}
