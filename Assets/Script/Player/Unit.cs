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

    public int CurrentAmmo { get { return weapon.GetCurrentAmmo; } }
    public int ReserveAmmo { get { return weapon.GetReserveAmmo; } }

    private Animator anim;
    private Rigidbody rigid;
    private CinemachineImpulseSource cImpulse;
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

        cImpulse = GetComponentInChildren<CinemachineImpulseSource>();

        addWeapon();
    }

    private void addWeapon()
    {

        weaponSlot[1] = WeaponFactory.CreateWeapon(eWeaponType.HandGun);
        weaponSlot[2] = WeaponFactory.CreateWeapon(eWeaponType.SubMachineGun);

        unitSlot1 = unitHandSlot.GetChild(0);
        unitSlot2 = unitHandSlot.GetChild(1);

        unitWeaponChange = new UnitWeaponChange(weaponSlot, unitSlot1.gameObject, unitSlot2.gameObject);
        weapon = unitWeaponChange.GetCurrentWeapon();
    }

    void Update()
    {
        playerInput.ReadInput();
        unitMovement.UnitMove(speed, isDodge, dodgeVec);
        unitMovement.jump(jumpPower, playerInput);
        unitRotation.unitMouseLook(transform, playerInput.GetMouseX, playerInput.GetMouseY, sensitivity);
        unitDodge.dodge(playerInput, this, unitMovement, speed, unitMovement.GetMoveVec);

        attack();
        weaponChange();

        //�׽�Ʈ
        if (Input.GetKeyDown(KeyCode.R))
        {
            weapon.Reload(unitWeaponChange.GetCurrentWeaponview());
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Cursor.lockState==CursorLockMode.Locked)
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
        if (!weapon.Automatic)
        {
            if (playerInput.GetFireDown)
            {
                unitAttack.Attack(weapon, unitWeaponChange.GetCurrentWeaponview(), unitHead);
            }
        }
        else
        {
            if (playerInput.GetFireHold)
            {
                unitAttack.Attack(weapon, playerInput, unitWeaponChange.GetCurrentWeaponview(), unitHead);
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
}
