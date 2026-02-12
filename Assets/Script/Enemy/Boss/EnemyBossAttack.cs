using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBossAttack : MonoBehaviour
{
    private Enemy enemy;

    private Transform missilePort1;
    private Transform missilePort2;
    private Transform mousePoint;
    private BossLaser bossLaser;
    private BossMove bossMove;

    [SerializeField] private float randomRangeMin = -30f;
    [SerializeField] private float randomRangeMax = 30f;

    [SerializeField] private Vector3 center;
    [SerializeField]private List<Vector3> wallVec = new List<Vector3>();


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // center ±‚¡ÿ
        Vector3 c = center;

        float sizeX = randomRangeMax - randomRangeMin;
        float sizeZ = randomRangeMax - randomRangeMin;

        Vector3 boxSize = new Vector3(sizeX, 0.1f, sizeZ);
        Vector3 boxCenter = new Vector3(
            c.x + (randomRangeMin + randomRangeMax) * 0.5f,
            transform.position.y,
            c.z + (randomRangeMin + randomRangeMax) * 0.5f
        );

        Gizmos.DrawWireCube(boxCenter, boxSize);
    }

    public void SetUp(Enemy _enemy, Transform _port1, Transform _port2, Transform _mousePoint)
    {
        enemy = _enemy;
        missilePort1 = _port1;
        missilePort2 = _port2;
        mousePoint = _mousePoint;
    }
    void Start()
    {
        bossMove = GetComponentInParent<BossMove>();


    }
    private void bossMoving()
    {
        bossMove.startMoving(enemy.transform.position);
    }

    private void bossWall()
    {
        int count = 0;
        int tryCount = 0;
        int maxTry = 100;
        while (count < 4 && tryCount < maxTry)
        {
            tryCount++;

            float randX = Random.Range(randomRangeMin, randomRangeMax);
            float randZ = Random.Range(randomRangeMin, randomRangeMax);

            Vector3 randomPos = new Vector3(randX + center.x, 0, randZ + center.z);
            bool overlapped = false;

            for (int i = 0; i < wallVec.Count; i++)
            {
                if (Mathf.Abs(wallVec[i].x - randomPos.x) < 2.3f &&
                    Mathf.Abs(wallVec[i].z - randomPos.z) < 2.3f)
                {
                    overlapped = true;
                    break;
                }
            }

            if (overlapped)
            {
                continue;
            }

            wallVec.Add(randomPos);
            count++;
        }
        for (int i = 0; i < wallVec.Count; i++)
        {
            GameObject obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.BossWall,
               GameManager.instance.PoolingParents[PoolingManager.ePoolingObject.BossWall.ToString()]);
            obj.GetComponent<BossWall>().SetStart(wallVec[i]);
            enemy.WallCount++;
        }
        wallVec.Clear();
    }

    private void bossMissile(bool _isLeft)
    {
        GameObject obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.MissileBoss,
            GameManager.instance.PoolingParents[PoolingManager.ePoolingObject.MissileBoss.ToString()]);
        Vector3 portVec = _isLeft ? missilePort1.position : missilePort2.position;
        obj.GetComponent<EnemyMissile>().SetBezier(portVec);
        obj.transform.position = portVec;
    }
    private void enemyMissileLeft()
    {
        bossMissile(true);
    }

    private void enemyMissileRight()
    {
        bossMissile(false);
    }

    private void bossRock()
    {
        GameObject obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.BossRock,
                GameManager.instance.PoolingParents[PoolingManager.ePoolingObject.BossRock.ToString()]);
        obj.transform.position = mousePoint.position;
        obj.transform.LookAt(GameManager.instance.GetUnit.transform);

    }

    private void enemyAttackEnd()
    {
        enemy.EnemyBossSkillTime();
    }

    private void enemyLaser()
    {
        GameObject obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.BossLaser,
                GameManager.instance.PoolingParents[PoolingManager.ePoolingObject.BossLaser.ToString()]);
        bossLaser = obj.GetComponent<BossLaser>();
        bossLaser.SetUp(mousePoint);
    }
    private void enemyLaserEnd()
    {
        bossLaser.LaserHitCheck(enemy);
        enemy.EnemyBossSkillTime();
    }

    private void DeathEnd()
    {
        enemy.EnemyDeath();
    }
}
