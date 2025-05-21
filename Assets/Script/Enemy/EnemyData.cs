using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    EnemyA,
    EnemyB,
}

[CreateAssetMenu(
 menuName = "Game/EnemyData",
 fileName = "NewEnemyData"
)]
public class EnemyData : ScriptableObject
{

    public EnemyType enemyType;
    public string EnemyName;

    public float Hp;
    public float Damage;
    public float Speed;
    public float chaseStopDistance;

}

