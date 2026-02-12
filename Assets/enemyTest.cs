using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemyTest : MonoBehaviour
{
    public bool start = false;
    public GameObject target;

    public NavMeshAgent na;

    void Start()
    {
        na = GetComponent<NavMeshAgent>();
        target = GameManager.instance.GetUnit.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if (start)
        {
            na.SetDestination(target.transform.position);
        }

    }
}
