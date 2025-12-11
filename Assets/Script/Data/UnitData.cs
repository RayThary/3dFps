using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/UnitData", fileName = "NewUnitData")]
public class UnitData : ScriptableObject
{
    //체력
    public float UnitMaxHp;
    public float UnitSpeed;
    public float UnitJumpPower;

    public float WeaponChangeTime;
    //마우스감도
    //public float Sensitivity;
    //총반동관련
    
    //최대 누적반동
    public float MaxRecoilAngle;
    //위아래 최소 최대값
    public float MaxPitch;
    public float MinPitch;

}
