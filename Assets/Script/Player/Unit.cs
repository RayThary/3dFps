using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Unit;

public class Unit : MonoBehaviour
{
    public event Action<float> OnUnitChangeHp;

    //기본 데이터/컴포넌트
    [SerializeField] private UnitData unitData;
    private PlayerInput playerInput;
    private UnitRotation unitRotation;
    public UnitRotation CurrentRotation { set { unitRotation = value; } }
    private UnitMovementModule movementModule;
    public UnitMovementModule UnitMovementModule { get { return movementModule; } }

    private UnitAttackModule attackModule;
    public UnitAttackModule UnitAttackModule { get { return attackModule; } }
    

    private Animator anim;
    private Rigidbody rigid;

    private Weapon weapon;
    public Weapon UnitWeapon { get { return weapon; } set { weapon = value; } }

    public int CurrentAmmo { get { return weapon.GetCurrentAmmo; } }
    public int ReserveAmmo { get { return weapon.GetReserveAmmo; } }

    //플레이어 Transform (본체 뼈대)
    [SerializeField] private Transform unitHead;
    public Transform GetUnitHead { get { return unitHead; } }

    [SerializeField] private Transform unitHandSlot;
    [SerializeField] private Transform neck;

    ///플레이어 상태값
    private float unitCurrentHp;
    public float UnitHp { get { return unitCurrentHp; }set { unitCurrentHp = value; } }

    private float unitSpeed;
    public float UnitSpeed { get { return unitSpeed; } set { unitSpeed = value; } }

    private bool isDodge = false;
    public bool IsDodge { get { return isDodge; } set { isDodge = value; } }

    private Vector3 dodgeVec;
    public Vector3 DodgeVec { get { return dodgeVec; } set { dodgeVec = value; } }


    private UnitSlot unitSlot = new UnitSlot();
    public UnitSlot CurrentSlot { get { return unitSlot; } set { unitSlot = value; } }

    [SerializeField] private float sensitivity; // 런타임용, 에디터 조정 X
    public float Sensitivity { get { return sensitivity; } set { sensitivity = value; } }

    // 플레이어슬롯
    public class UnitSlot
    {
        public Transform unitMeleeSlot1;
        public Transform unitMeleeSlot2;
        public Transform unitSlot1;
        public Transform unitSlot2;

        //무기 슬롯
        public Dictionary<int, Weapon> weaponSlot = new Dictionary<int, Weapon>();
    }


    //골드 임시테스트용 직렬화
    [SerializeField]
    private int currentGold = 0;
    public int Gold { get { return currentGold; } set { currentGold = value; } }

    //  능력치 (Unit 고유값)
    private UnitStat unitStat = new UnitStat();
    public UnitStat CurrentStat { get { return unitStat; } set { unitStat = value; } }

    private UnitStat unitStatBasic;

    public class UnitStat
    {
        public float unitMaxHp;
        public float unitSpeed;
        public float unitJumpPower;
        public float weaponChangeTime = 2f;
        public float maxRecoilAngle = 15f;
        public float minPitch = -45f;
        public float maxPitch = 45f;

        public UnitStat Clone() => (UnitStat)MemberwiseClone();

        public void setUnitStat(UnitData unitData)
        {
            unitMaxHp = unitData.UnitMaxHp;
            unitSpeed = unitData.UnitSpeed;
            unitJumpPower = unitData.UnitJumpPower;
            weaponChangeTime = unitData.WeaponChangeTime;
            maxRecoilAngle = unitData.MaxRecoilAngle;
            maxPitch = unitData.MaxPitch;
            minPitch = unitData.MinPitch;
        }
    }

    [SerializeField] private CinemachineVirtualCamera povCamera;
    public CinemachineVirtualCamera PovCamera { get { return povCamera; } }



    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        unitStat.setUnitStat(unitData);
        unitCurrentHp = unitStat.unitMaxHp;
        unitSpeed = unitStat.unitSpeed;

        unitStatBasic = unitStat.Clone();

        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        playerInput = new PlayerInput();

        addWeapon();
        movementModule = new UnitMovementModule();
        movementModule.SetUp(this, unitData, anim, rigid, transform, unitHead, neck, playerInput);
        attackModule = new UnitAttackModule();
        attackModule.SetUp(this, anim, unitRotation, povCamera, playerInput);

        unitRotation.SetUnitAttack(attackModule.CurrentUnitAttack);

        if (GameManager.instance.GetUnit == null)
        {
            GameManager.instance.SetUnit = this;
        }
        else
        {
            if (GameManager.instance.GetUnit != this)
            {
                Destroy(gameObject);
            }
        }
    }
    private void addWeapon()
    {

        unitSlot.weaponSlot[1] = WeaponFactory.CreateWeapon(eWeaponType.HandGun);
        unitSlot.weaponSlot[2] = WeaponFactory.CreateWeapon(eWeaponType.SubMachineGun);

        unitSlot.unitSlot1 = unitHandSlot.GetChild(0);
        unitSlot.unitSlot2 = unitHandSlot.GetChild(1);


    }

    void Update()
    {
        if (GameManager.instance.IsPaused || GameManager.instance.UnitStop) return;

        playerInput.ReadInput();
        movementModule.UpdateMovement();
        attackModule.UpdateAttack(playerInput);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

    }



    void LateUpdate()
    {
        unitRotation.ApplyRotation(playerInput);
    }





    //모듈빼줄것 아마도?


    public void TakeDamge(float _damage)
    {
        unitCurrentHp -= _damage;
        Debug.Log("hit Player");
        OnUnitChangeHp?.Invoke(unitCurrentHp);

        if (unitCurrentHp <= 0)
        {
            death();
        }
    }

    private void death()
    {
        //아직안만듬
        anim.SetTrigger("Death");
    }

    public void ReloadEnd()
    {
        weapon.ReloadAmmo();
        weapon.IsReloading = false;
    }


}
