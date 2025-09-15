using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitZoom
{
    private CinemachineVirtualCamera vCamera;
    private bool downKey;
    private bool upKey;

    public void SetUp(PlayerInput _playerInput,CinemachineVirtualCamera _vCamera)
    {
        downKey = _playerInput.ButtonDown[InputAction.Zoom];
        upKey= _playerInput.ButtonUp[InputAction.Zoom];
        vCamera = _vCamera;
    }

    public void UnitWeaponZoom()
    {
        if (downKey)
        {
            vCamera.m_Lens.FieldOfView = 30;
        }
        //또는 현재웨펀이 줌이있는무기가아닌지 추가해줄것 || current 인터페이스로체크해주면됨
        if (upKey)
        {
            vCamera.m_Lens.FieldOfView = 60;
        }
        
    }
}
