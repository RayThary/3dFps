using Cinemachine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public event Action<float> OnUnitChangeHp;

    //클래스
    private PlayerInput playerInput;
    private UnitMovement unitMovement;
    private UnitRotation unitRotation;
    private UnitAttack unitAttack;
    private UnitDodge unitDodge;
    private Weapon weapon;
    private UnitHandMotion unitHandMotion;
    private UnitWeaponChange unitWeaponChange;
    public UnitWeaponChange GetUnitWeaponChange { get { return unitWeaponChange; } }

    public Weapon GetWeapon { get { return weapon; } }

    //대시
    private bool isDodge = false;
    public bool IsDodge { get { return isDodge; } set { isDodge = value; } }
    private Vector3 dodgeVec;
    public Vector3 DodgeVec { set { dodgeVec = value; } }

    [SerializeField] private float unitMaxHp;
    private float unitCurrentHp;

    //이동
    [SerializeField] private float unitSpeed;
    public float SetSpeed { get { return unitSpeed; } set { unitSpeed = value; } }
    [SerializeField] private float unitJumpPower;


    //플레이어 오브젝트
    [SerializeField] private Transform unitHead;
    [SerializeField] private Transform unitHandSlot;
    [SerializeField] private Transform unitMeleeSlot;

    private Transform unitMeleeSlot1;
    private Transform unitMeleeSlot2;
    //기본무기 
    private Transform unitSlot1;
    private Transform unitSlot2;
    private Dictionary<int, Weapon> weaponSlot = new Dictionary<int, Weapon>();

    [SerializeField] private float weaponChangeTime = 2;

    //마우스감도
    [SerializeField] private float sensitivity = 0.8f;
    //총반동관련
    // 반동 복귀 속도
    [SerializeField] private float recoilRecoverSpeed = 5f;
    //최대 누적반동
    [SerializeField] private float maxRecoilAngle = 15f;
    //위아래 최소 최대값
    [SerializeField] private float minPitch = -45f;
    [SerializeField] private float maxPitch = 45f;

    [SerializeField] private Transform neck;
    public int CurrentAmmo { get { return weapon.GetCurrentAmmo; } }
    public int ReserveAmmo { get { return weapon.GetReserveAmmo; } }

    private Animator anim;
    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        unitHandMotion = GetComponent<UnitHandMotion>();
        unitDodge = GetComponent<UnitDodge>();

        unitRotation = new UnitRotation();
        playerInput = new PlayerInput();
        unitMovement = new UnitMovement();
        unitMovement.SetUp(transform, anim, rigid, playerInput);

        unitCurrentHp = unitMaxHp;

        unitAttack = GetComponent<UnitAttack>();
        unitAttack.SetUnitRot(unitRotation);


        addWeapon();

        unitRotation.SetUnitRotation(unitHead, neck, minPitch, maxPitch, maxRecoilAngle, recoilRecoverSpeed);
    }

    private void addWeapon()
    {

        weaponSlot[1] = WeaponFactory.CreateWeapon(eWeaponType.HandGun);
        weaponSlot[2] = WeaponFactory.CreateWeapon(eWeaponType.SubMachineGun);

        unitSlot1 = unitHandSlot.GetChild(0);
        unitSlot2 = unitHandSlot.GetChild(1);
        unitMeleeSlot1 = unitMeleeSlot.GetChild(0);
        unitMeleeSlot2 = unitMeleeSlot.GetChild(1);

        unitWeaponChange = new UnitWeaponChange(weaponSlot, unitSlot1.gameObject, unitSlot2.gameObject, unitMeleeSlot1.gameObject, unitMeleeSlot2.gameObject, weaponChangeTime, unitAttack);
        weapon = unitWeaponChange.GetCurrentWeapon();
    }

    void Update()
    {
        playerInput.ReadInput();
        unitMovement.UnitMove(unitSpeed, isDodge, dodgeVec);
        unitMovement.jump(unitJumpPower, playerInput);
        unitRotation.unitMouseLook(transform, playerInput.GetMouseX, playerInput.GetMouseY, sensitivity);
        unitDodge.dodge(playerInput, this, unitMovement, unitSpeed, unitMovement.GetMoveVec);
        unitWeaponChange.WeaponChangeCheck(playerInput);
        unitWeaponChange.WeaponChangeCool();
        attack();
        weaponChange();


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
        unitRotation.ApplyRotation(unitAttack.GetIsRecoil, weapon.IsMelee);
    }



    private void attack()
    {
        if (weapon.IsMelee)
        {
            if (playerInput.GetFireDown)
            {
                unitAttack.Attack(weapon, unitWeaponChange.GetCurrentWeaponview(), anim);
            }
        }
        else
        {
            if (!weapon.Automatic)
            {
                if (playerInput.GetFireDown)
                {
                    unitAttack.Attack(weapon, unitWeaponChange.GetCurrentWeaponview());
                }
            }
            else
            {
                if (playerInput.GetFireHold)
                {
                    unitAttack.Attack(weapon, playerInput, unitWeaponChange.GetCurrentWeaponview());
                }
            }
        }
    }


    private void weaponChange()
    {
        if (playerInput.GetWeapon1)
        {
            unitHandMotion.handMotion(unitWeaponChange, 1);
            weapon = unitWeaponChange.GetCurrentWeapon();
        }
        else if (playerInput.GetWeapon2)
        {
            unitHandMotion.handMotion(unitWeaponChange, 2);
            weapon = unitWeaponChange.GetCurrentWeapon();
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
