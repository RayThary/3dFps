using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitClone : MonoBehaviour
{

    //임시
    private Unit unit;

    public event Action<float> OnUnitChangeHp;

    [SerializeField] private UnitData unitData;
    //클래스
    private PlayerInput playerInput;
    private UnitMovement unitMovement;
    private UnitRotation unitRotation;
    public UnitRotation CurrentRotation { set { unitRotation = value; } }
    private UnitAttack unitAttack;
    private UnitDodge unitDodge;
    private Weapon weapon;
    public Weapon UnitWeapon { get { return weapon; } set { weapon = value; } }
    private UnitHandMotion unitHandMotion;
    private UnitWeaponChange unitWeaponChange;
    public UnitWeaponChange GetUnitWeaponChange { get { return unitWeaponChange; } }

    private UnitZoom unitZoom;


    //대시
    private bool isDodge = false;
    public bool IsDodge { get { return isDodge; } set { isDodge = value; } }
    private Vector3 dodgeVec;
    public Vector3 DodgeVec { get { return dodgeVec; } set { dodgeVec = value; } }




    //플레이어 오브젝트
    [SerializeField] private Transform unitHead;
    public Transform GetUnitHead { get { return unitHead; } }
    [SerializeField] private Transform unitHandSlot;
    [SerializeField] private Transform unitMeleeSlot;



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

    private bool zoomCheck = false;


    //유닛 설정 
    private bool statChange = false;
    public bool SetStatChange { set { statChange = value; } }


    public class UnitStat
    {
        //마우스감도 이건 유닛데이터가아닌다른곳에 넣어줄필요있음
        public float sensitivity = 0.8f;

        public float unitMaxHp;

        // 이동
        public float unitSpeed;
        public float unitJumpPower;
        // 무기 교체 시간
        public float weaponChangeTime = 2f;

        // 총 반동 관련
        public float recoilRecoverSpeed = 5f;
        public float maxRecoilAngle = 15f;
        public float minPitch = -45f;
        public float maxPitch = 45f;

        // 크리티컬 관련
        public float criticalChance;
        public float criticalDamage;
        public UnitStat Clone()
        {
            return (UnitStat)MemberwiseClone();
        }

        public void setUnitStat(UnitData unitData)
        {
            sensitivity = unitData.Sensitivity;//임시

            unitMaxHp = unitData.UnitMaxHp;
            unitSpeed = unitData.UnitSpeed;
            unitJumpPower = unitData.UnitJumpPower;
            weaponChangeTime = unitData.WeaponChangeTime;
            //recoilRecoverSpeed = unitData.RecoilRecoverSpeed;
            maxRecoilAngle = unitData.MaxRecoilAngle;
            maxPitch = unitData.MaxPitch;
            minPitch = unitData.MinPitch;

            criticalChance = unitData.CriticalChance;
            criticalDamage = unitData.CriticalDamage;

        }
    }
    private UnitStat unitStat = new UnitStat();
    public UnitStat CurrentStat { get { return unitStat; } }
    private UnitStat unitStatBasic;


    private float unitCurrentHp;

    //이동
    private float unitSpeed;
    public float SetSpeed { get { return unitSpeed; } set { unitSpeed = value; } }




    [SerializeField] private Transform neck;
    public int CurrentAmmo { get { return weapon.GetCurrentAmmo; } }
    public int ReserveAmmo { get { return weapon.GetReserveAmmo; } }

    private Animator anim;
    private Rigidbody rigid;

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
        unitHandMotion = GetComponent<UnitHandMotion>();
        unitDodge = GetComponent<UnitDodge>();

        unitRotation = new UnitRotation();
        playerInput = new PlayerInput();
        unitMovement = new UnitMovement();
        unitMovement.SetUp(transform, anim, rigid, playerInput);


        unitAttack = GetComponent<UnitAttack>();
        unitAttack.SetUnitAttack(unitRotation);

        addWeapon();
        unitZoom = new UnitZoom();
        unitZoom.SetUp(playerInput, povCamera);

        unitRotation.SetUnitRotation(unitHead, neck, unitStat.minPitch, unitStat.maxPitch, unitStat.maxRecoilAngle, unitStat.recoilRecoverSpeed);
       
    }


    private void addWeapon()
    {

        unitSlot.weaponSlot[1] = WeaponFactory.CreateWeapon(eWeaponType.HandGun);
        unitSlot.weaponSlot[2] = WeaponFactory.CreateWeapon(eWeaponType.SubMachineGun);

        unitSlot.unitSlot1 = unitHandSlot.GetChild(0);
        unitSlot.unitSlot2 = unitHandSlot.GetChild(1);
        unitSlot.unitMeleeSlot1 = unitMeleeSlot.GetChild(0);
        unitSlot.unitMeleeSlot2 = unitMeleeSlot.GetChild(1);

        unitWeaponChange = new UnitWeaponChange(unit, unitSlot.weaponSlot, unitSlot.unitSlot1.gameObject, unitSlot.unitSlot2.gameObject,
            unitSlot.unitMeleeSlot1.gameObject, unitSlot.unitMeleeSlot2.gameObject, unitStat.weaponChangeTime, unitAttack);
        weapon = unitWeaponChange.GetCurrentWeapon();
    }

    void Update()
    {
        playerInput.ReadInput();
        unitMovement.UnitMove(unitSpeed, isDodge, dodgeVec);
        unitMovement.jump(unitStat.unitJumpPower, playerInput);
        unitRotation.unitMouseLook(transform, playerInput.GetAxis[InputAction.MouseX],
            playerInput.GetAxis[InputAction.MouseY], unitStat.sensitivity);
        unitDodge.dodge(playerInput, unit, unitMovement, unitSpeed, unitMovement.GetMoveVec);
        unitWeaponChange.WeaponChangeCheck(playerInput);
        unitWeaponChange.WeaponChangeCool();
        attack();
        weaponChange();
        changeUnitStat();
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log($"{unitWeaponChange.GetCurrentWeapon().GetRecoilRecoverSpeed}");
        }


        //나중에모아줄것
        if (playerInput.ButtonDown[InputAction.Zoom])
        {
            unitWeaponChange.GetCurrentWeapon().Zoomable(povCamera, true);
            zoomCheck = true;
        }
        if (playerInput.ButtonUp[InputAction.Zoom])
        {
            unitWeaponChange.GetCurrentWeapon().Zoomable(povCamera, false);
            zoomCheck = false;
        }
        //테스트
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (weapon.IsMelee)
            {
                weapon.Reload(anim);
            }
            else
            {
                weapon.Reload(unitWeaponChange.GetCurrentWeaponview());
            }
        }
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


    private void attack()
    {
        if (weapon.IsMelee)
        {
            if (playerInput.ButtonDown[InputAction.Fire])
            {

                unitAttack.Attack_Melee(weapon, unitWeaponChange.GetCurrentWeaponview(), anim);
            }
        }
        else
        {
            if (!weapon.Automatic)
            {
                if (playerInput.ButtonDown[InputAction.Fire])
                {
                    unitAttack.Attack_Single(weapon, unitWeaponChange.GetCurrentWeaponview(), zoomCheck);
                }
            }
            else
            {
                if (playerInput.ButtonHold[InputAction.Fire])
                {
                    unitAttack.Attack_Auto(weapon, playerInput, unitWeaponChange.GetCurrentWeaponview());
                }
            }
        }
    }


    private void weaponChange()
    {
        if (playerInput.ButtonDown[InputAction.Weapon1])
        {
            unitHandMotion.handMotion(unitWeaponChange, 1);

        }
        else if (playerInput.ButtonDown[InputAction.Weapon2])
        {
            unitHandMotion.handMotion(unitWeaponChange, 2);
        }

    }

    //
    private void changeUnitStat()
    {
        if (statChange)
        {
            if (!Mathf.Approximately(unitStatBasic.criticalChance, unitStat.criticalChance) ||
                !Mathf.Approximately(unitStatBasic.criticalDamage, unitStat.criticalDamage))
            {
                unitStatBasic.criticalChance = unitStat.criticalChance;
                unitStatBasic.criticalDamage = unitStat.criticalDamage;
            }

            unitStatBasic = unitStat.Clone();
        }

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
        unitWeaponChange.GetCurrentWeaponview().MeleeEnd();
    }
}
