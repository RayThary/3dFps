using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    public enum eMissileType
    {
        Straight,
        Curve,
        BossRock,
    }
    [SerializeField] private eMissileType missileType;

    private float enemyDamage;
    [SerializeField] private float missileSpeed;

    [SerializeField] private LayerMask RemoveLayer;

    private iMissile missile;
    private MissileCurve curve;

    private Vector3 p0, p1, p2;

    public void SetDamage(float _enemyDamage)
    {
        enemyDamage = _enemyDamage;
    }

    public void SetBezier(Vector3 _p0)
    {
        curve.SetP0(_p0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<Unit>().TakeDamge(enemyDamage);
        }

        if (((1 << other.gameObject.layer) & RemoveLayer.value) != 0)
        {
            if (missileType == eMissileType.BossRock)
            {
                BossWall wall = other.GetComponent<BossWall>();
                if (wall != null)
                {
                    wall.RemoveWall();
                }
            }
            missile.OnHit(gameObject);
        }
    }



    private void Awake()
    {
        switch (missileType)
        {
            case eMissileType.Straight:
                missile = new MissileStright(transform, missileSpeed);
                break;
            case eMissileType.Curve:
                curve = new MissileCurve(transform, missileSpeed);
                missile = curve;
                break;
            case eMissileType.BossRock:
                missile = new MissileRock(transform, missileSpeed);
                break;
        }
    }


    void Update()
    {
        missile.Update();
    }


}
