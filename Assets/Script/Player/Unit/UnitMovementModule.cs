using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class UnitMovementModule
{
    private Unit unit;

    private UnitMovement unitMovement;
    private UnitRotation unitRotation;
    private UnitDodge unitDodge;
    private PlayerInput playerInput;

    private Transform unitTrs;

    public void SetUp(Unit _unit, UnitData _unitData, Animator _anim, Rigidbody _rigid, Transform _unitTrs, Transform _head, Transform _neck, PlayerInput _playerInput)
    {
        unit = _unit;
        unitTrs = _unitTrs;

        playerInput = _playerInput;

        unitMovement = new UnitMovement();
        unitMovement.SetUp(unitTrs, _anim, _rigid, _playerInput);

        unitRotation = new UnitRotation();
        unitRotation.SetUnitRotation(_head, _neck, _unitData.MinPitch, _unitData.MaxPitch, _unitData.MaxRecoilAngle, unit.CurrentStat.sensitivity);
        unit.CurrentRotation = unitRotation;

        unitDodge = unit.GetComponent<UnitDodge>();
    }

    public void UpdateMovement()
    {
        unitMovement.UnitMove(unit.CurrentStat.unitSpeed, unit.IsDodge, unit.DodgeVec);
        unitMovement.jump(unit.CurrentStat.unitJumpPower, playerInput);
        unitRotation.unitMouseLook(unitTrs, playerInput.GetAxis[InputAction.MouseX],
            playerInput.GetAxis[InputAction.MouseY], unit.CurrentStat.sensitivity);
        unitDodge.dodge(playerInput, unit, unitMovement, unit.CurrentStat.unitSpeed, unitMovement.GetMoveVec);
    }

}
