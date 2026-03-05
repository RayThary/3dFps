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
    private bool fireLock = false;
    private bool isFire = false;
    public bool IsFire { set { isFire = value; } }

    public void SetUp(Unit _unit, Animator _anim, UnitRotation _unitRotation, CinemachineVirtualCamera _povCamera, PlayerInput _playerInput)
    {
        unit = _unit;
        anim = _anim;
        povCamera = _povCamera;
        unitAttack = unit.GetComponent<UnitAttack>();
        unitAttack.SetUnitAttack(_unitRotation);

        //무기 셋팅
        unitWeaponChange = new UnitWeaponChange(unit, unit.CurrentSlot.weaponSlot, unit.CurrentSlot.unitSlot1.gameObject,
            unit.CurrentSlot.unitSlot2.gameObject, unit.CurrentStat.weaponChangeTime, unitAttack);
        unit.UnitWeapon = unitWeaponChange.GetCurrentWeapon();


        unitZoom = new UnitZoom();
        unitZoom.SetUp(_playerInput, povCamera);

        unitHandMotion = unit.GetComponent<UnitHandMotion>();
    }

    public void UpdateAttack(PlayerInput _playerInput)
    {

        unitWeaponChange.WeaponChangeCheck(_playerInput);
        unitWeaponChange.WeaponChangeCool();
        attack(_playerInput);
        weaponChange(_playerInput);
        zoom(_playerInput);
        weaponReroad(_playerInput);
    }

    private void attack(PlayerInput _playerInput)
    {

        if (fireLock)
        {
            if (!_playerInput.ButtonHold[InputAction.Fire])
            {
                fireLock = false;
                isFire = false;
            }
            return;
        }

        if (_playerInput.ButtonDown[InputAction.Fire])
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                return;
            }
            IsFire = true;
        }

        if (unitHandMotion.IsSwitching)
        {
            return;
        }


        if (!unit.UnitWeapon.Automatic)
        {
            if (isFire)
            {
                switch (unit.UnitWeapon.WeaponType)
                {
                    case eWeaponType.HandGun:
                        unitAttack.Attack_Single(unit.UnitWeapon, unitWeaponChange.GetWeaponview(), zoomCheck, unit.UnitWeapon.SpreadRange);
                        break;

                    case eWeaponType.ShotGun:
                        unitAttack.Attack_ShotGun(unit.UnitWeapon, unitWeaponChange.GetWeaponview(), unit.UnitWeapon.SpreadRange);
                        break;
                    case eWeaponType.Sniper:
                        unitAttack.Attack_Single(unit.UnitWeapon, unitWeaponChange.GetWeaponview(), zoomCheck, unit.UnitWeapon.SpreadRange);
                        break;
                }
                isFire = false;
            }
        }
        else
        {

            if (_playerInput.ButtonHold[InputAction.Fire] && isFire)
            {
                switch (unit.UnitWeapon.WeaponType)
                {
                    case eWeaponType.SubMachineGun:
                        unitAttack.Attack_Auto(unit.UnitWeapon, _playerInput, unitWeaponChange.GetWeaponview(), this,
                            unit.UnitWeapon.FireDelay, unit.UnitWeapon.SpreadRange);
                        break;
                    case eWeaponType.Rifle:
                        unitAttack.Attack_Auto(unit.UnitWeapon, _playerInput, unitWeaponChange.GetWeaponview(), this,
                            unit.UnitWeapon.FireDelay, unit.UnitWeapon.SpreadRange);
                        break;
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
            unitWeaponChange.GetWeaponview().WeaponZoom(true);
            zoomCheck = true;
        }
        if (_playerInput.ButtonUp[InputAction.Zoom])
        {
            unitWeaponChange.GetCurrentWeapon().Zoomable(povCamera, false);
            unitWeaponChange.GetWeaponview().WeaponZoom(false);
            zoomCheck = false;
        }
    }

    private void weaponChange(PlayerInput _playerInput)
    {
        if (_playerInput.ButtonDown[InputAction.Weapon1])
        {
            fireLockCheck(1);
            unitHandMotion.handMotion(unitWeaponChange, 1);
            fireLock = true;
        }
        else if (_playerInput.ButtonDown[InputAction.Weapon2])
        {
            fireLockCheck(2);
            unitHandMotion.handMotion(unitWeaponChange, 2);
            fireLock = true;
        }

    }

    private void fireLockCheck(int _slot)
    {
        if (unitWeaponChange.GetCurrentSlot != _slot)
        {
            fireLock = true;
        }
    }

    private void weaponReroad(PlayerInput _playerInput)
    {
        if (_playerInput.ButtonDown[InputAction.Reload])
        {


            unit.UnitWeapon.Reload(unitWeaponChange.GetWeaponview());

        }
    }
    private void changeUnitStat()
    {




    }
}