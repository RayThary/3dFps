using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class UnitAttackModule
{
    private Unit unit;
    private UnitAttack unitAttack;
    public UnitAttack CurrentUnitAttack { get { return unitAttack; } }
    private UnitHandMotion unitHandMotion;
    private UnitZoom unitZoom;
    private UnitWeaponChange unitWeaponChange;
    public UnitWeaponChange GetUnitWeaponChange { get { return unitWeaponChange; } }

    private Animator anim;
    private CinemachineVirtualCamera povCamera;
    private bool zoomCheck = false;


    public void SetUp(Unit _unit, Animator _anim, UnitRotation _unitRotation, CinemachineVirtualCamera _povCamera, PlayerInput _playerInput)
    {
        unit = _unit;
        anim = _anim;
        povCamera = _povCamera;
        unitAttack = unit.GetComponent<UnitAttack>();
        unitAttack.SetUnitAttack(_unitRotation);

        //무기 셋팅
        unitWeaponChange = new UnitWeaponChange(unit, unit.CurrentSlot.weaponSlot, unit.CurrentSlot.unitSlot1.gameObject, unit.CurrentSlot.unitSlot2.gameObject,
            unit.CurrentSlot.unitMeleeSlot1.gameObject, unit.CurrentSlot.unitMeleeSlot2.gameObject, unit.CurrentStat.weaponChangeTime, unitAttack);
        unit.UnitWeapon = unitWeaponChange.GetCurrentWeapon();


        unitZoom = new UnitZoom();
        unitZoom.SetUp(_playerInput, povCamera);

        unitHandMotion = unit.GetComponent<UnitHandMotion>();
    }

    public void UpdateAttack(PlayerInput _playerInput)
    {

        unitWeaponChange.WeaponChangeCheck(_playerInput);
        unitWeaponChange.WeaponChangeCool();
        zoom(_playerInput);
        attack(_playerInput);
        weaponReroad(_playerInput);
        weaponChange(_playerInput);
    }

    private void attack(PlayerInput _playerInput)
    {
        if (unit.UnitWeapon.IsMelee)
        {
            if (_playerInput.ButtonDown[InputAction.Fire])
            {

                unitAttack.Attack_Melee(unit.UnitWeapon, unitWeaponChange.GetCurrentWeaponview(), anim);
            }
        }
        else
        {

            if (!unit.UnitWeapon.Automatic)
            {
                if (_playerInput.ButtonDown[InputAction.Fire])
                {
                    switch (unit.UnitWeapon.WeaponType)
                    {
                        case eWeaponType.HandGun:
                            unitAttack.Attack_Single(unit.UnitWeapon, unitWeaponChange.GetCurrentWeaponview(), zoomCheck);
                            break;

                        case eWeaponType.ShotGun:
                            unitAttack.Attack_ShotGun(unit.UnitWeapon, unitWeaponChange.GetCurrentWeaponview());
                            break;
                        case eWeaponType.Sniper:
                            unitAttack.Attack_Single(unit.UnitWeapon, unitWeaponChange.GetCurrentWeaponview(), zoomCheck);
                            break;
                    }
                }
            }
            else
            {
                if (_playerInput.ButtonHold[InputAction.Fire])
                {
                    switch (unit.UnitWeapon.WeaponType)
                    {
                        case eWeaponType.SubMachineGun:
                            unitAttack.Attack_Auto(unit.UnitWeapon, _playerInput, unitWeaponChange.GetCurrentWeaponview());
                            break;
                    }
                }
            }
        }
    }

    private void zoom(PlayerInput _playerInput)
    {
        if (!unit.UnitWeapon.ZoomWeapon)
        {
            return;
        }

        if (_playerInput.ButtonDown[InputAction.Zoom])
        {
            unitWeaponChange.GetCurrentWeapon().Zoomable(povCamera, true);
            zoomCheck = true;
        }
        if (_playerInput.ButtonUp[InputAction.Zoom])
        {
            unitWeaponChange.GetCurrentWeapon().Zoomable(povCamera, false);
            zoomCheck = false;
        }
    }

    private void weaponChange(PlayerInput _playerInput)
    {
        if (_playerInput.ButtonDown[InputAction.Weapon1])
        {
            unitHandMotion.handMotion(unitWeaponChange, 1);

        }
        else if (_playerInput.ButtonDown[InputAction.Weapon2])
        {
            unitHandMotion.handMotion(unitWeaponChange, 2);
        }

    }

    private void weaponReroad(PlayerInput _playerInput)
    {
        if (_playerInput.ButtonDown[InputAction.Reload])
        {

            if (unit.UnitWeapon.IsMelee)
            {
                unit.UnitWeapon.Reload(anim);
            }
            else
            {
                unit.UnitWeapon.Reload(unitWeaponChange.GetCurrentWeaponview());
            }
        }
    }
    private void changeUnitStat()
    {




    }
}