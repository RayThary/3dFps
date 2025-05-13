using System.Collections;
using System.Collections.Generic;
using UnityEngine;




//유닛로테이션 임시저장
#region
//using System.Collections;
//using System.Collections.Generic;
//using System.Threading;
//using UnityEngine;

//public class UnitRotation
//{

//    private float rotationPitch = 0; //마우스의 움직임값
//    private float recoilPitch = 0; //총알의 반동량
//    private float basePitch = 0f; //보정용

//    //반동 셋팅
//    private float recoilRecoverSpeed = 5f;// 반동 복귀 속도
//    private float maxRecoilAngle = 15;//반동최대범위
//    //위아래 최소 최대값
//    private float minPitch;
//    private float maxPitch;

//    private bool mouseMoveAttack;//마우스가 공격도중움직였는가?

//    private bool isMelee;

//    private Transform playerHead;
//    private Transform neck;

//    public void SetUnitRotation(Transform _head, Transform _neck, float _minP, float _maxP, float _maxRecoilAngle, float recoverSpd, bool _isMelee)
//    {
//        playerHead = _head;
//        minPitch = _minP;
//        maxPitch = _maxP;
//        maxRecoilAngle = _maxRecoilAngle;
//        recoilRecoverSpeed = recoverSpd;
//        isMelee = _isMelee;
//        neck = _neck;
//    }

//    public void unitMouseLook(Transform unit, float _mouseX, float _mouseY, float _sensitivity)
//    {
//        //좌우회전
//        unit.Rotate(0, _mouseX * _sensitivity, 0f, Space.World);


//        //상하회전
//        rotationPitch -= _mouseY * _sensitivity;
//        rotationPitch = Mathf.Clamp(rotationPitch, minPitch, maxPitch);

//        //playerHead.localRotation = Quaternion.Euler(rotationPitch, 0, 0);
//        neck.localRotation = Quaternion.Euler(rotationPitch, 0, 0);

//        //마우스 움직인체크
//        mouseMoveAttack = Mathf.Abs(_mouseY) > 0.0001f;

//        if (mouseMoveAttack)
//        {
//            basePitch = rotationPitch + recoilPitch;
//        }

//    }

//    public void unitRecoil(float _recoilPower)
//    {

//        basePitch = rotationPitch;

//        recoilPitch += _recoilPower;
//        recoilPitch = Mathf.Clamp(recoilPitch, 0, maxRecoilAngle);
//    }


//    //반동후되돌아가는부분
//    public void ApplyRotation(bool _isRecoil)
//    {

//        //if (mouseMoveAttack)
//        //{
//        //    basePitch = rotationPitch;
//        //}
//        //else if (!_isRecoil)
//        //{
//        //    recoilPitch = Mathf.MoveTowards(recoilPitch, 0f, recoilRecoverSpeed * Time.deltaTime);
//        //}

//        if (!_isRecoil && !mouseMoveAttack)
//        {
//            recoilPitch = Mathf.MoveTowards(recoilPitch, 0f, recoilRecoverSpeed * Time.deltaTime);
//        }



//        float finalPitch = basePitch - recoilPitch;
//        finalPitch = Mathf.Clamp(finalPitch, minPitch, maxPitch);
//        playerHead.localRotation = Quaternion.Euler(finalPitch, 0f, 0f);

//        Debug.Log($"rot:{rotationPitch:F2}" + $"base:{basePitch:F2}" + $"recoil:{recoilPitch:F2} " + $"final:{finalPitch:F2}");

//    }
//    public void ResetMouseRecoil()
//    {

//        float current = playerHead.localEulerAngles.x;
//        if (current > 180f) current -= 360f;

//        // 내부 조준값과 기준값을 실제 각도로 싱크
//        rotationPitch = current;
//        basePitch = current;


//        //recoilPitch = 0f;

//    }

//}

#endregion
