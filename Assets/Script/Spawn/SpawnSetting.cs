using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSetting : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnTower = new List<Transform>();

    [SerializeField] private bool spawnStart = false;
    

    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            EnemySpawn eSpawn = transform.GetChild(i).GetComponent<EnemySpawn>();
            if (eSpawn != null)
            {
                eSpawn.SpawnStart = true;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    { 
        
    }
}
