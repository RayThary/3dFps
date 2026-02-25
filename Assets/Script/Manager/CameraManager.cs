using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private LayerMask firstPersonMask;

    private CinemachineVirtualCamera POVCam;


    private CinemachineBrain mainCamera;

    public void SetPovCam(CinemachineVirtualCamera _povCam) 
    {
        POVCam = _povCam;
        POVCam.Priority = 11;
    }
    void Start()
    {
        mainCamera = Camera.main.GetComponent<CinemachineBrain>();

        Camera.main.cullingMask = firstPersonMask.value;
    }

    public void InitializeCamera(Unit _player,Transform _playerTrs)
    {
        Transform head = _player.GetUnitHead;
        POVCam.Follow = head;
    }


}
