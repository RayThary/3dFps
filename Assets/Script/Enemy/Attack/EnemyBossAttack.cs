using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossAttack : MonoBehaviour
{
    private Enemy enemy;

    private Transform missilePort1;
    private Transform missilePort2;
    private Transform mousePoint;
    [SerializeField]
    private BossLaser bossLaser;
    private BossMove bossMove;

    public void SetUp(Enemy _enemy, Transform _port1, Transform _port2,Transform _mousePoint)
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
        bossMove.SetUp(enemy.transform.position);
    }

    private void bossWall()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.BossWall,
               GameManager.instance.PoolingParents[PoolingManager.ePoolingObject.BossWall.ToString()]);
            obj.GetComponent<BossWall>().SetStart();
            enemy.WallCount++;
        }
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
