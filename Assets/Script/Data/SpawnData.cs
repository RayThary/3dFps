using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game/SpawnData", fileName = "NewSpawnData")]
public class SpawnData : ScriptableObject
{
    public List<spawnSetting> SpawnSetting;
}
[System.Serializable]
public class spawnSetting
{
    [Tooltip("스테이지 번호")]
    public int StageNum;
    [Tooltip("몬스터의 총개수")]
    public int StageSpawnCount;

    [Tooltip("근접")] public int CountA;
    [Tooltip("돌진")] public int CountB;
    [Tooltip("미사일")] public int CountC;
    [Tooltip("저격")] public int CountF;
}
