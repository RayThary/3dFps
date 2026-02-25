using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCloud : MonoBehaviour
{
    [SerializeField] private float minSpeed = 1;
    [SerializeField] private float maxSpeed = 2;
    private float speed;

    private float changeTime = 5;
    private float chaneTimer = 0;
    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        chaneTimer += Time.deltaTime;
        if (chaneTimer >= changeTime)
        {
            chaneTimer = 0;
            speed = Random.Range(minSpeed, maxSpeed);
        }
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
