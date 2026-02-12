using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UnitRotation
{


    private float rotationPitch = 0; //마우스의 움직임값
    private float recoilPitch = 0; //총알의 반동량
    private float basePitch = 0f; //보정용

    //반동 셋팅
    private float recoilRecoverSpeed = 5f;// 반동 복귀 속도
    private float maxRecoilAngle = 15;//반동최대범위
    //위아래 최소 최대값
    private float minPitch;
    private float maxPitch;

    private float minMeleePitch;
    private float maxMeleePitch;


    private bool attackCheck = false;
    private bool mouseMoveAttack;

    private Transform playerHead;
    private Transform playerNeck;

    private UnitAttack unitAttack;

    public void SetUnitRotation(Transform _head, Transform _neck, float _minP, float _maxP, float _maxRecoilAngle, float recoverSpd)
    {
        playerHead = _head;
        minPitch = _minP;
        maxPitch = _maxP;
        maxRecoilAngle = _maxRecoilAngle;
        recoilRecoverSpeed = recoverSpd;
        playerNeck = _neck;

        minMeleePitch = minMeleePitch / 2;
        maxMeleePitch = maxPitch * 2;
    }
    //어택이늦게 선언되기때문에 따로 참조
    public void SetUnitAttack(UnitAttack _unitAttack)
    {
        unitAttack = _unitAttack;
    }

    public void unitMouseLook(Transform unit, float _mouseX, float _mouseY, float _sensitivity)
    {
        unit.Rotate(0, _mouseX * _sensitivity, 0f, Space.World);

        rotationPitch -= _mouseY * _sensitivity;
        rotationPitch = Mathf.Clamp(rotationPitch, minPitch, maxPitch);
    }

    public void unitRecoil(float _recoilPower, float _recoilRecoverSpeed)
    {
        basePitch = rotationPitch;

        recoilPitch += _recoilPower;
        recoilPitch = Mathf.Clamp(recoilPitch, 0, maxRecoilAngle);
        recoilRecoverSpeed = _recoilRecoverSpeed;
        attackCheck = true;
    }

    private float tempMouseY;
    private float delta;
    //반동후되돌아가는부분
    public void ApplyRotation(PlayerInput _playerInput)
    {

        delta = Mathf.Abs(_playerInput.GetAxis[InputAction.MouseY] - tempMouseY);
        mouseMoveAttack = delta > 0.01f;

        if (mouseMoveAttack)
        {
            basePitch = rotationPitch;
        }


        if (!unitAttack.GetIsRecoil && attackCheck)
        {
            recoilPitch = Mathf.MoveTowards(recoilPitch, 0f, recoilRecoverSpeed * Time.deltaTime);
            if (Mathf.Abs(recoilPitch) <= 0.01f)
            {
                recoilPitch = 0;
                attackCheck = false;
            }
        }

        float finalPitch = basePitch - recoilPitch;



        finalPitch = Mathf.Clamp(finalPitch, minPitch, maxPitch);
        playerNeck.localRotation = Quaternion.Euler(finalPitch, 0f, 0f);


        playerHead.localRotation = Quaternion.Euler(finalPitch, 0f, 0f);

        tempMouseY = _playerInput.GetAxis[InputAction.MouseY];
        mouseMoveAttack = false;
    }
    public void ResetMouseRecoil()
    {

        float current = playerHead.localEulerAngles.x;
        if (current > 180f) current -= 360f;

        rotationPitch = current;
        basePitch = current;
        recoilPitch = 0f;
    }
}


