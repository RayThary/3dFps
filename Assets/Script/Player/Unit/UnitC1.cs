using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitC1 : MonoBehaviour
{

    public event Action<float> OnUnitChangeHp;

    // ?? 기본 데이터/컴포넌트
    [SerializeField] private UnitData unitData;
    private PlayerInput playerInput;
    private Animator anim;
    private Rigidbody rigid;

    // ?? 플레이어 Transform (본체 뼈대)
    [SerializeField] private Transform unitHead;
    public Transform GetUnitHead => unitHead;

    [SerializeField] private Transform unitHandSlot;
    [SerializeField] private Transform unitMeleeSlot;
    [SerializeField] private Transform neck;

    // ?? 플레이어 상태값
    private float unitCurrentHp;
    private float unitSpeed;
    public float SetSpeed { get { return unitSpeed; } set { unitSpeed = value; } }

    private bool isDodge = false;
    public bool IsDodge { get { return isDodge; } set { isDodge = value; } }

    private Vector3 dodgeVec;
    public Vector3 DodgeVec { set { dodgeVec = value; } }

    private bool statChange = false;
    public bool SetStatChange { set { statChange = value; } }

    // ?? 능력치 (Unit 고유값)
    private UnitStat unitStat = new UnitStat();
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

   

        if (GameManager.instance.GetUnit == null)
        {
            //GameManager.instance.SetUnit = this;
        }
        else
        {
            if (GameManager.instance.GetUnit != this)
            {
                Destroy(gameObject);
            }
        }
    }


   

    void Update()
    {
        playerInput.ReadInput();

      
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

    //이부분문제 
    public void ReloadEnd(Weapon _weapon)
    {
        _weapon.ReloadAmmo();
        _weapon.IsReloading = false;
    }

    public void UnitMeleeEnd(UnitWeaponChange _unitWeaponChange)
    {
        _unitWeaponChange.GetCurrentWeaponview().MeleeEnd();
    }
}
