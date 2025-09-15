using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bmtest : MonoBehaviour
{
    public bool t;
    public float speed;
    public Vector3 backupPoint;
    public Vector3 targetPoint;

    void Start()
    {
        //backupPoint = transform.position;
        //targetPoint = backupPoint;
        //targetPoint.y -= 5;
    }
    public bool u;

    // Update is called once per frame
    void Update()
    {
        if (t)
        {
            transform.position += Vector3.down * Time.deltaTime * speed;
            if (transform.position.y <= -10)
            {
                t = false;
                u = true;
                targetPoint.y = backupPoint.y;
            }
        }

        if (u)

        {
            transform.position += Vector3.up * Time.deltaTime * (speed / 2);
            if (transform.position.y >= targetPoint.y)
            {
                u = false;
            }
        }
    }
}
