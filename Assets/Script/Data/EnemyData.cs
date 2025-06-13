using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    EnemyA,
    EnemyB,
    EnemyC,
    EnemyF,
}

[CreateAssetMenu(menuName = "Game/EnemyData", fileName = "NewEnemyData")]
public class EnemyData : ScriptableObject
{

    public EnemyType enemyType;
    [Tooltip("enum 이름을 표시합니다")]
    public string EnemyName;

    public float Hp;
    public float Damage;
    public float Speed;
    public float chaseStopDistance;
    private void OnValidate()
    {
        EnemyName = enemyType.ToString();
    }

}



