using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class UnitWeaponChange
{
    public event Action<Weapon> OnWeaponSwitched;

    private Unit unit;
    private UnitAttack unitAttack;

    // 무기 관리
    private Dictionary<int, Weapon> weaponSlot;
    private Dictionary<int, WeaponView> weaponViewSlot = new Dictionary<int, WeaponView>();

    private GameObject gunSlot1Obj;
    private GameObject gunSlot2Obj;


    private int currentSlot;
    public int GetCurrentSlot => currentSlot;

    private bool isChange = true;
    public bool IsChange => isChange;


    // 교체 시간
    private float weaponChangeTimer = 0.0f;
    public float GetWeaponChagneTimer => weaponChangeTimer;

    private float weaponChangeTime = 2f;

    public float ChangeCooldown;


    public Weapon GetCurrentWeapon()
    {
        return weaponSlot[currentSlot];
    }

    public WeaponView GetWeaponview()
    {
        return weaponViewSlot[currentSlot];
    }

    public WeaponView GetWeaponview(int slot)
    {
        return weaponViewSlot.ContainsKey(slot) ? weaponViewSlot[slot] : null;
    }

    public UnitWeaponChange(Unit _unit, Dictionary<int, Weapon> _weaponSlot, GameObject _gunSlot1Obj,
        GameObject _gunSlot2Obj, float _weaponChangeTime, UnitAttack _unitAttack, int _defaultSlot = 1)
    {
        unit = _unit;
        weaponSlot = _weaponSlot;
        gunSlot1Obj = _gunSlot1Obj;
        gunSlot2Obj = _gunSlot2Obj;

        currentSlot = _defaultSlot;

        weaponChangeTime = _weaponChangeTime;
        weaponChangeTimer = 0;

        unitAttack = _unitAttack;

        slotSwitch();
        weaponInstantiate(_gunSlot1Obj, _gunSlot2Obj);
    }

    //슬롯 활성화 처리
    private void slotSwitch()
    {
        gunSlot1Obj.SetActive(currentSlot == 1);
        gunSlot2Obj.SetActive(currentSlot == 2);
    }


    //초기화
    private void weaponInstantiate(GameObject _slot1, GameObject _slot2)
    {
        // --- 슬롯 1 ---
        var weapon1 = weaponSlot[1];
        var weapon1Obj = GameObject.Instantiate(weapon1.WeaponPrefeb, _slot1.transform);
        var view1 = weapon1Obj.GetComponent<WeaponView>();
        weaponViewSlot[1] = view1;

        view1.Initialize(weapon1);
        view1.WeaponPicupLayer(true);
        view1.WeaponPickup.GetComponent<BoxCollider>().enabled = false;

        // --- 슬롯 2 ---
        var weapon2 = weaponSlot[2];
        var weapon2Obj = GameObject.Instantiate(weapon2.WeaponPrefeb, _slot2.transform);
        var view2 = weapon2Obj.GetComponent<WeaponView>();
        weaponViewSlot[2] = view2;

        view2.Initialize(weapon2);
        view2.WeaponPicupLayer(true);
        view2.WeaponPickup.GetComponent<BoxCollider>().enabled = false;
    }


    //슬롯 변경
    public void WeaponSwitch(int _slotNum)
    {
        if (!isChange) return;
        if (_slotNum == currentSlot) return;
        if (!weaponSlot.ContainsKey(_slotNum)) return;

        WeaponView oldView = weaponViewSlot[currentSlot];

        isChange = false;
        currentSlot = _slotNum;

        slotSwitch();
        OnWeaponSwitched?.Invoke(GetCurrentWeapon());

        WeaponView newView = weaponViewSlot[currentSlot];
    }


    //무기 교환
    public void WeaponChangeCheck(PlayerInput _playerinput)
    {
        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        RaycastHit hit;
        int distanceRay = 10;

        if (Physics.Raycast(ray, out hit, distanceRay, LayerMask.GetMask("WeaponPickup")))
        {
            GameManager.instance.CenterTextObj.SetActive(true);
            GameManager.instance.CenterText.text = "[F] 교체";

            if (_playerinput.ButtonDown[InputAction.FCheck])
            {
                WeaponChange(hit.transform.GetComponentInParent<WeaponView>());
            }
        }
        else
        {
            if (GameManager.instance.CenterText.text == "[F] 교체")
            {
                GameManager.instance.CenterTextObj.SetActive(false);
            }
        }
    }


    private void WeaponChange(WeaponView _pickupWeaponView)
    {
        if (!isChange) return;

        isChange = false;

        Vector3 weaponEuler = new Vector3(0, -90, 0);

        var nowView = weaponViewSlot[currentSlot];
        if (nowView != null)
        {
            nowView.transform.SetParent(GameManager.instance.GetWorldParent, true);
            nowView.WeaponPickup.GetComponent<BoxCollider>().enabled = true;
            nowView.WeaponPicupLayer(false);
            nowView.Anim.enabled = false;
            Transform parent = _pickupWeaponView.transform.parent;
            nowView.transform.SetParent(parent);
            nowView.transform.localPosition = Vector3.zero;
            nowView.transform.localRotation = Quaternion.Euler(weaponEuler);

        }

        var newWeapon = WeaponFactory.CreateWeapon(_pickupWeaponView.WeaponType);
        weaponSlot[currentSlot] = newWeapon;

        GameObject parentSlot = (currentSlot == 1) ? gunSlot1Obj : gunSlot2Obj;

        _pickupWeaponView.transform.SetParent(parentSlot.transform, false);
        _pickupWeaponView.Initialize(newWeapon);
        _pickupWeaponView.WeaponPicupLayer(true);

        _pickupWeaponView.MeshObject.localPosition = Vector3.zero;
        _pickupWeaponView.transform.localRotation = Quaternion.Euler(weaponEuler);

        weaponViewSlot[currentSlot] = _pickupWeaponView;

        OnWeaponSwitched?.Invoke(GetCurrentWeapon());
        unit.UnitWeapon = weaponSlot[currentSlot];
    }

    //무기 교체 쿨다운
    public void WeaponChangeCool()
    {
        if (!isChange)
        {
            weaponChangeTimer += Time.deltaTime;
            ChangeCooldown = 1f - (weaponChangeTimer / weaponChangeTime);

            if (weaponChangeTimer >= weaponChangeTime)
            {
                isChange = true;
                weaponChangeTimer = 0;
                ChangeCooldown = 0;
            }
        }
        else
        {
            ChangeCooldown = 0;
        }
    }
}
