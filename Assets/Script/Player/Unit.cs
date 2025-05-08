using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    //Ŭ����
    private PlayerInput playerInput;
    private UnitMovement unitMovement;
    private UnitRotation unitRotation;
    private UnitAttack unitAttack;
    private UnitDodge unitDodge;
    private UnitWeaponChange unitWeaponChange;
    private Weapon weapon;

    //���
    private bool isDodge = false;
    public bool IsDodge { get { return isDodge; } set { isDodge = value; } }
    private Vector3 dodgeVec;
    public Vector3 DodgeVec { set { dodgeVec = value; } }

    //�̵�
    [SerializeField] private float speed;
    public float SetSpeed { get { return speed; } set { speed = value; } }
    [SerializeField] private float jumpPower;


    //�÷��̾� ������Ʈ
    [SerializeField] private Transform unitHead;
    [SerializeField] private Transform unitHandSlot;
    [SerializeField] private Transform unitMeleeSlot;

    private Transform unitMeleeSlot1;
    private Transform unitMeleeSlot2;
    //�⺻���� 
    private Transform unitSlot1;
    private Transform unitSlot2;
    private Dictionary<int, Weapon> weaponSlot = new Dictionary<int, Weapon>();

    //���콺����
    [SerializeField] private float sensitivity = 0.8f;
    //�ѹݵ�����
    // �ݵ� ���� �ӵ�
    [SerializeField] private float recoilRecoverSpeed = 5f;
    //�ִ� �����ݵ�
    [SerializeField] private float maxRecoilAngle = 15f;
    //���Ʒ� �ּ� �ִ밪
    [SerializeField] private float minPitch = -45f;
    [SerializeField] private float maxPitch = 45f;

    [SerializeField] private Transform neck;
    public int CurrentAmmo { get { return weapon.GetCurrentAmmo; } }
    public int ReserveAmmo { get { return weapon.GetReserveAmmo; } }

    private Animator anim;
    private Rigidbody rigid;

    private CinemachineImpulseSource cImpulse;//���ӸŴ����� ī�޶� �Ŵ����ڽ����γ־�� ���߿��ʿ��Ҷ� �����ؼ��־������
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        playerInput = new PlayerInput();

        unitMovement = new UnitMovement();
        unitMovement.SetUp(transform, anim, rigid, playerInput);

        unitRotation = new UnitRotation();
        unitRotation.SetUnitRotation(unitHead, minPitch, maxPitch, maxRecoilAngle, recoilRecoverSpeed);

        unitAttack = GetComponent<UnitAttack>();
        unitAttack.SetUnitRot(unitRotation);

        unitDodge = GetComponent<UnitDodge>();

        //cImpulse = GetComponentInChildren<CinemachineImpulseSource>();

        addWeapon();
    }

    private void addWeapon()
    {

        weaponSlot[1] = WeaponFactory.CreateWeapon(eWeaponType.HandGun);
        weaponSlot[2] = WeaponFactory.CreateWeapon(eWeaponType.SubMachineGun);

        unitSlot1 = unitHandSlot.GetChild(0);
        unitSlot2 = unitHandSlot.GetChild(1);
        unitMeleeSlot1 = unitMeleeSlot.GetChild(0);
        unitMeleeSlot2 = unitMeleeSlot.GetChild(1);

        unitWeaponChange = new UnitWeaponChange(weaponSlot, unitSlot1.gameObject, unitSlot2.gameObject, unitMeleeSlot1.gameObject, unitMeleeSlot2.gameObject);
        weapon = unitWeaponChange.GetCurrentWeapon();
    }

    void Update()
    {
        playerInput.ReadInput();
        unitMovement.UnitMove(speed, isDodge, dodgeVec);
        unitMovement.jump(jumpPower, playerInput);
        unitRotation.unitMouseLook(transform, neck, playerInput.GetMouseX, playerInput.GetMouseY, sensitivity);
        unitDodge.dodge(playerInput, this, unitMovement, speed, unitMovement.GetMoveVec);
        unitWeaponChange.WeaponChangeCheck(playerInput);

        attack();
        weaponChange();

        //�׽�Ʈ
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!weapon.IsMelee)
            {
                weapon.Reload(unitWeaponChange.GetCurrentWeaponview());
            }
            else
            {
                weapon.Reload(anim);
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
        unitRotation.ApplyRotation(unitAttack.GetIsRecoil);
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
            unitWeaponChange.WeaponSwitch(1);
        }
        else if (playerInput.GetWeapon2)
        {
            unitWeaponChange.WeaponSwitch(2);
        }
        weapon = unitWeaponChange.GetCurrentWeapon();
    }

    public void ReloadEnd()
    {
        weapon.ReloadAmmo();
        weapon.IsReloading = false;
    }
}
