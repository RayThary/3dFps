using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private LayerMask firstPersonMask;
    [SerializeField] private LayerMask thirdPersonMask;

    void Start()
    {
        Camera.main.cullingMask = firstPersonMask;

    }

    void Update()
    {
    }
}
