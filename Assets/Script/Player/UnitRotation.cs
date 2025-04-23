using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UnitRotation
{

    private float rotationPitch = 0; //���콺�� �����Ӱ�
    private float recoilPitch = 0; //�Ѿ��� �ݵ���
    private float basePitch = 0f; //������

    //�ݵ� ����
    private float recoilRecoverSpeed = 5f;// �ݵ� ���� �ӵ�
    private float maxRecoilAngle = 15;//�ݵ��ִ����
    //���Ʒ� �ּ� �ִ밪
    private float minPitch;
    private float maxPitch;

    private bool mouseMoveAttack;//���콺�� ���ݵ��߿������°�?

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


    //�ݵ��ĵǵ��ư��ºκ�
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
        // �賻�� ���ذ��� ���ذ��� ���� ������ ��ũ
        rotationPitch = current;
        basePitch = current;


        recoilPitch = 0f;

    }

}
