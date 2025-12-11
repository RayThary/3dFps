using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private LayerMask firstPersonMask;
    [SerializeField] private LayerMask thirdPersonMask;

    private CinemachineVirtualCamera POVCam;
    
    [SerializeField] private CinemachineVirtualCamera ChangeCam;
    [SerializeField] private CinemachineVirtualCamera V3Cam;

    private CinemachineBrain mainCamera;

    public void SetPovCam(CinemachineVirtualCamera _povCam) 
    {
        POVCam = _povCam;
        POVCam.Priority = 11;
    }
    void Start()
    {
        mainCamera = Camera.main.GetComponent<CinemachineBrain>();


        ChangeCam.Priority = 5;
        Camera.main.cullingMask = firstPersonMask.value;


    }

    public void InitializeCamera(Unit _player,Transform _playerTrs)
    {
        Transform head = _player.GetUnitHead;
        POVCam.Follow = head;

        ChangeCam.Follow = _playerTrs;
        ChangeCam.LookAt = head;

        V3Cam.Follow = _playerTrs;
        V3Cam.LookAt = head;
    }


}
