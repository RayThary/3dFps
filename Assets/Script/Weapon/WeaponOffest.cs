using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOffest : MonoBehaviour
{
    public Vector3 offset = new Vector3(0f, 0f, -0.2f);

    private Vector3 initialLocalPos;

    void Start()
    {
        // ������ �� ���� ���� ��ġ ����
        initialLocalPos = transform.localPosition;
    }

    void LateUpdate()
    {
        // �� ������ �������� �ʱ� ��ġ + offset ����
        //transform.localPosition = initialLocalPos + offset;
    }
}
