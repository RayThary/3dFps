using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOffest : MonoBehaviour
{
    public Vector3 offset = new Vector3(0f, 0f, -0.2f);

    private Vector3 initialLocalPos;

    void Start()
    {
        // 시작할 때 현재 로컬 위치 저장
        initialLocalPos = transform.localPosition;
    }

    void LateUpdate()
    {
        // 매 프레임 마지막에 초기 위치 + offset 적용
        //transform.localPosition = initialLocalPos + offset;
    }
}
