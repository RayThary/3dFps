using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testItem : MonoBehaviour
{
    public bool a = false;
    void Start()
    {

    }

    int count = 0;
    public Transform a1;
    // Update is called once per frame
    void Update()
    {
        if (a)
        {
            Debug.Log(Time.timeScale);
            Time.timeScale = 1;
            a = false;
        }
    }
}
