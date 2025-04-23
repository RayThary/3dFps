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

    private bool mouseMoveAttack;//마우스가 공격도중움직였는가?

    private Transform playerHead;

    public void SetUnitRotation(Transform _head, float _minP, float _maxP,float _maxRecoilAngle , float recoverSpd)
    {
        playerHead = _head;
        minPitch = _minP;
        maxPitch = _maxP;
        maxRecoilAngle = _maxRecoilAngle;
        recoilRecoverSpeed = recoverSpd;
    }

    public void unitMouseLook(Transform unit, float _mouseX, float _mouseY, float _sensitivity)
    {
        unit.Rotate(0, _mouseX * _sensitivity, 0f, Space.World);

        rotationPitch -= _mouseY * _sensitivity;
        rotationPitch = Mathf.Clamp(rotationPitch, minPitch, maxPitch);

        mouseMoveAttack = Mathf.Abs(_mouseY) > 0;

        if (mouseMoveAttack)
        {
            basePitch = rotationPitch;
        }

    }

    public void unitRecoil(float _recoilPower)
    {
        basePitch = rotationPitch;

        recoilPitch += _recoilPower;
        recoilPitch = Mathf.Clamp(recoilPitch, 0, maxRecoilAngle);
    }


    //반동후되돌아가는부분
    public void ApplyRotation(bool _isRecoil)
    {

        if (!_isRecoil && !mouseMoveAttack)
        {
            recoilPitch = Mathf.MoveTowards(recoilPitch, 0f, recoilRecoverSpeed * Time.deltaTime);
        }

        float finalPitch = basePitch - recoilPitch;
        finalPitch = Mathf.Clamp(finalPitch, minPitch, maxPitch);
        playerHead.localRotation = Quaternion.Euler(finalPitch, 0f, 0f);

        mouseMoveAttack = false;
    }
    public void ResetMouseRecoil()
    {

        float current = playerHead.localEulerAngles.x;
        if (current > 180f) current -= 360f;
        // ②내부 조준값과 기준값을 실제 각도로 싱크
        rotationPitch = current;
        basePitch = current;


        recoilPitch = 0f;

    }

}
