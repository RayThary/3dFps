using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public bool spawn = false;
    public GameObject test;
    void Start()
    {
        GameManager.instance.EnemyMaxCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawn)
        {
            GameManager.instance.AddKillCount();
            spawn = false;
        }

    }
}
