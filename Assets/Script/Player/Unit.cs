using Cinemachine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private Transform unitMeleeSlot;
    [SerializeField] private Transform neck;

    ///플레이어 상태값
    private float unitCurrentHp;
    private float unitSpeed;
    public float SetSpeed { get { return unitSpeed; } set { unitSpeed = value; } }

    private bool isDodge = false;
    public bool IsDodge { get { return isDodge; } set { isDodge = value; } }

    private Vector3 dodgeVec;
    public Vector3 DodgeVec { get { return dodgeVec; } set { dodgeVec = value; } }

    private bool statChange = false;
    public bool SetStatChange { set { statChange = value; } }

    private UnitSlot unitSlot = new UnitSlot();
    public UnitSlot CurrentSlot { get { return unitSlot; } set { unitSlot = value; } }

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



    //  능력치 (Unit 고유값)
    private UnitStat unitStat = new UnitStat();
    public UnitStat CurrentStat { get { return unitStat; } set { unitStat = value; } }
    
    private UnitStat unitStatBasic;

    public class UnitStat
    {
        public float sensitivity = 0.8f;
        public float unitMaxHp;
        public float unitSpeed;
        public float unitJumpPower;
        public float weaponChangeTime = 2f;
        public float maxRecoilAngle = 15f;
        public float minPitch = -45f;
        public float maxPitch = 45f;

        // 크리티컬 관련
        public float criticalChance;
        public float criticalDamage;

        public UnitStat Clone() => (UnitStat)MemberwiseClone();

        public void setUnitStat(UnitData unitData)
        {
            sensitivity = unitData.Sensitivity; // 임시
            unitMaxHp = unitData.UnitMaxHp;
            unitSpeed = unitData.UnitSpeed;
            unitJumpPower = unitData.UnitJumpPower;
            weaponChangeTime = unitData.WeaponChangeTime;
            criticalChance = unitData.CriticalChance;
            criticalDamage = unitData.CriticalDamage;
            maxRecoilAngle = unitData.MaxRecoilAngle;
            maxPitch = unitData.MaxPitch;
            minPitch = unitData.MinPitch;
        }
    }

    [SerializeField] private CinemachineVirtualCamera povCamera;


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
            Debug.Log("없음");
        }
        else
        {
            if (GameManager.instance.GetUnit != this)
            {
                Destroy(gameObject);
            }
                Debug.Log("있음");
        }
    }
    private void addWeapon()
    {

        unitSlot.weaponSlot[1] = WeaponFactory.CreateWeapon(eWeaponType.HandGun);
        unitSlot.weaponSlot[2] = WeaponFactory.CreateWeapon(eWeaponType.SubMachineGun);

        unitSlot.unitSlot1 = unitHandSlot.GetChild(0);
        unitSlot.unitSlot2 = unitHandSlot.GetChild(1);
        unitSlot.unitMeleeSlot1 = unitMeleeSlot.GetChild(0);
        unitSlot.unitMeleeSlot2 = unitMeleeSlot.GetChild(1);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log($"{UnitWeapon.GetName}");
        }
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
        unitRotation.ApplyRotation( weapon.IsMelee, playerInput);
    }





    //모듈빼줄것
    public void changeUnitStat()
    {
        unitStatBasic = unitStat;
    }

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

    public void UnitMeleeEnd()
    {
        //unitWeaponChange.GetCurrentWeaponview().MeleeEnd();
    }
}
