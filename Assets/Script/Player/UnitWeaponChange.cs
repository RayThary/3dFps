using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class UnitWeaponChange
{
    public event Action<Weapon> OnWeaponSwitched;

    private Dictionary<int, Weapon> weaponSlot;
    private GameObject gunSlot1Obj;
    private GameObject gunSlot2Obj;

    private GameObject meleeSlot1Obj;
    private GameObject meleeSlot2Obj;

    private UnitAttack unitAttack;

    private Dictionary<int, WeaponView> weaponViewSlot = new Dictionary<int, WeaponView>();
    private int currentSlot;
    public int GetCurrentSlot { get { return currentSlot; } }

    private bool isChange = true;
    public bool IsChange { get { return isChange; } }

    private float weaponChangeTimer = 0.0f;
    public float GetWeaponChagneTimer { get { return weaponChangeTimer; } }

    private float weaponChangeTime = 2;

    public float ChangeCooldown;

    public Weapon GetCurrentWeapon()
    {
        return weaponSlot[currentSlot];
    }

    public UnitWeaponChange(Dictionary<int, Weapon> _weaponSlot, GameObject _gunSlot1Obj, GameObject _gunSlot2Obj,
        GameObject _meleeSlot1Obj, GameObject _meleeSlot2Obj, float _weaponChangeTime, UnitAttack _unitAttack, int _defaultSlot = 1)
    {
        this.weaponSlot = _weaponSlot;
        this.gunSlot1Obj = _gunSlot1Obj;
        this.gunSlot2Obj = _gunSlot2Obj;
        this.meleeSlot1Obj = _meleeSlot1Obj;
        this.meleeSlot2Obj = _meleeSlot2Obj;

        currentSlot = _defaultSlot;

        this.weaponChangeTime = _weaponChangeTime;
        this.weaponChangeTimer = 0;

        unitAttack = _unitAttack;
        slotSwitch();
        weaponInstantiate(_gunSlot1Obj, _gunSlot2Obj);
    }


    private void slotSwitch()
    {

        gunSlot1Obj.SetActive(currentSlot == 1);
        meleeSlot1Obj.SetActive(currentSlot == 1);

        gunSlot2Obj.SetActive(currentSlot == 2);
        meleeSlot2Obj.SetActive(currentSlot == 2);
    }

    private void weaponInstantiate(GameObject _slot1, GameObject _slot2)
    {
        var weapon1 = weaponSlot[1];
        var weapon1Obj = GameObject.Instantiate(weapon1.WeaponPrefeb, _slot1.transform);
        var view1 = weapon1Obj.GetComponent<WeaponView>();
        weaponViewSlot[1] = view1;
        view1.Initialize(weapon1);
        view1.WeaponPickup.GetComponent<BoxCollider>().enabled = false;
        view1.OnMeleeHit += unitAttack.HandleMeleeHits;

        var weapon2 = weaponSlot[2];
        var weapon2Obj = GameObject.Instantiate(weapon2.WeaponPrefeb, _slot2.transform);
        var view2 = weapon2Obj.GetComponent<WeaponView>();
        weaponViewSlot[2] = view2;
        view2.Initialize(weapon2);
        view2.WeaponPickup.GetComponent<BoxCollider>().enabled = false;
        view2.OnMeleeHit += unitAttack.HandleMeleeHits;
    }
    public WeaponView GetCurrentWeaponview()
    {
        return weaponViewSlot[currentSlot];
    }


    public void WeaponSwitch(int _slotNum)
    {
        if (!isChange) return;

        if (_slotNum == currentSlot) return;
        if (!weaponSlot.ContainsKey(_slotNum)) return;

        WeaponView oldView = weaponViewSlot[currentSlot];
        if (oldView != null)
            oldView.OnMeleeHit -= unitAttack.HandleMeleeHits;

        isChange = false;
        currentSlot = _slotNum;

        slotSwitch();
        OnWeaponSwitched?.Invoke(GetCurrentWeapon());

        WeaponView newView = weaponViewSlot[currentSlot];
        newView.OnMeleeHit += unitAttack.HandleMeleeHits;

        CameraManager camMger = GameManager.instance.GetCameraManager;
        camMger.weaponSwitched(weaponSlot[currentSlot]);
    }


    public void WeaponChangeCheck(PlayerInput _playerinput)
    {
        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        RaycastHit hit;
        int distanceRay = weaponSlot[currentSlot].IsMelee ? 20 : 10;

        if (Physics.Raycast(ray, out hit, distanceRay, LayerMask.GetMask("WeaponPickup")))
        {
            GameManager.instance.CheckF.SetActive(true);
            if (_playerinput.FCheck)
            {
                WeaponChange(hit.transform.GetComponentInParent<WeaponView>());
            }
        }
        else
        {
            GameManager.instance.CheckF.SetActive(false);
        }
    }
    public void WeaponChange(WeaponView _pickupWeaponView)
    {
        if (!isChange) return;

        isChange = false;
        var nowView = weaponViewSlot[currentSlot];
        if (nowView != null)
        {
            nowView.transform.SetParent(GameManager.instance.GetWeaponParent, true);
            nowView.WeaponPickup.GetComponent<BoxCollider>().enabled = true;
            nowView.OnMeleeHit -= unitAttack.HandleMeleeHits;
        }

        var newWeapon = WeaponFactory.CreateWeapon(_pickupWeaponView.WeaponType);
        weaponSlot[currentSlot] = newWeapon;
        GameObject parentSlot = null;
        Vector3 weaponEuler;
        if (newWeapon.IsMelee)
        {
            parentSlot = (currentSlot == 1) ? meleeSlot1Obj : meleeSlot2Obj;
            weaponEuler = Vector3.zero;
        }
        else
        {
            parentSlot = (currentSlot == 1) ? gunSlot1Obj : gunSlot2Obj;
            weaponEuler = new Vector3(0, -90, 0);
        }

        _pickupWeaponView.transform.SetParent(parentSlot.transform, false);
        _pickupWeaponView.Initialize(newWeapon);

        _pickupWeaponView.MeshObject.localPosition = Vector3.zero;
        _pickupWeaponView.transform.localRotation = Quaternion.Euler(weaponEuler);

        weaponViewSlot[currentSlot] = _pickupWeaponView;

        OnWeaponSwitched?.Invoke(GetCurrentWeapon());

        _pickupWeaponView.OnMeleeHit += unitAttack.HandleMeleeHits;

        CameraManager camMger = GameManager.instance.GetCameraManager;
        camMger.weaponSwitched(weaponSlot[currentSlot]);
    }

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

    //무기소환쪽코드 이건몬스터나 보물상자에서 무기를소환할떄쓰면좋을듯함
    #region
    //    var newWeapon = WeaponFactory.CreateWeapon(_pickupWeapon.WeaponType);

    //        if (newWeapon == null)
    //        {
    //            Debug.LogError($"WeaponFactory에 타입이없습니다.{_pickupWeapon.WeaponType}");
    //            return;
    //        }


    //var nowView = weaponViewSlot[currentSlot];
    //if (nowView != null)
    //{
    //    GameObject.Destroy(nowView.gameObject);
    //}

    //weaponSlot[currentSlot] = newWeapon;

    //GameObject parentSlot = (currentSlot == 1) ? slot1Obj : slot2Obj;

    //GameObject nowWeapon = GameObject.Instantiate(newWeapon.WeaponPrefeb, parentSlot.transform);
    //WeaponView nowWeaponView = nowWeapon.GetComponent<WeaponView>();
    #endregion
}
