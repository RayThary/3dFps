using System.Collections;
using System.Collections.Generic;
using UnityEngine;




//���ַ����̼� �ӽ�����
#region
//using System.Collections;
//using System.Collections.Generic;
//using System.Threading;
//using UnityEngine;

//public class UnitRotation
//{

//    private float rotationPitch = 0; //���콺�� �����Ӱ�
//    private float recoilPitch = 0; //�Ѿ��� �ݵ���
//    private float basePitch = 0f; //������

//    //�ݵ� ����
//    private float recoilRecoverSpeed = 5f;// �ݵ� ���� �ӵ�
//    private float maxRecoilAngle = 15;//�ݵ��ִ����
//    //���Ʒ� �ּ� �ִ밪
//    private float minPitch;
//    private float maxPitch;

//    private bool mouseMoveAttack;//���콺�� ���ݵ��߿������°�?

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
//        //�¿�ȸ��
//        unit.Rotate(0, _mouseX * _sensitivity, 0f, Space.World);


//        //����ȸ��
//        rotationPitch -= _mouseY * _sensitivity;
//        rotationPitch = Mathf.Clamp(rotationPitch, minPitch, maxPitch);

//        //playerHead.localRotation = Quaternion.Euler(rotationPitch, 0, 0);
//        neck.localRotation = Quaternion.Euler(rotationPitch, 0, 0);

//        //���콺 ������üũ
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


//    //�ݵ��ĵǵ��ư��ºκ�
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

//        // ���� ���ذ��� ���ذ��� ���� ������ ��ũ
//        rotationPitch = current;
//        basePitch = current;


//        //recoilPitch = 0f;

//    }

//}

#endregion
