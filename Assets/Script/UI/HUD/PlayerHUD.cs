using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;




public class PlayerHUD : MonoBehaviour
{
    private Unit unit;
    private UnitWeaponChange unitWeaponChange;
    private UnitSkill unitSkill;
    private UnitDodge unitDodge;

    //스킬
    [SerializeField] private Image skillCool;
    [SerializeField] private TextMeshProUGUI skillCoolText;
    private float skillCoolTime;
    private bool uesSkill;

    // 대시
    [SerializeField] private Image dodgeCool;
    [SerializeField] private TextMeshProUGUI dodgeCoolText;
    private float dodgeCoolTime;
    private bool isDodge;

    // 무기 / 총알
    [SerializeField] private TextMeshProUGUI ammoText;
    private int currentAmmo;
    private int reserveAmmo;


    //무기 UI/ 아이콘
    [System.Serializable]
    public struct WeaponIcon
    {
        public eWeaponType type;
        public Sprite sprite;
    }

    [SerializeField] private Image iconImage;
    [SerializeField] private Image weaponCool;
    [SerializeField] private List<WeaponIcon> weaponIcon = new List<WeaponIcon>();

    //체력 UI
    [SerializeField] private Image hpFill;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Image unitIcon;
    [SerializeField] private List<Sprite> unitIcons = new List<Sprite>();

    //gold
    [SerializeField] private TextMeshProUGUI playerGold;


    void Start()
    {
        unit = GameManager.instance.GetUnit;
        unitDodge = unit.GetComponent<UnitDodge>();
        unitSkill = unit.GetComponent<UnitSkill>();

        dodgeCoolTime = unitDodge.GetDodgeCool;
        unitWeaponChange = unit.UnitAttackModule.GetUnitWeaponChange;

        onUIWeaponIcon(unit.UnitWeapon);
        unitWeaponChange.OnWeaponSwitched += onUIWeaponIcon;

        skillCoolTime = unitSkill.GetCoolTime;

        unitIcon.sprite = GameManager.instance.UnitName == "Ludo" ? unitIcons[0] : unitIcons[1];
    }

    void OnDestroy()
    {
        unitWeaponChange.OnWeaponSwitched -= onUIWeaponIcon;
    }
    private void onUIWeaponIcon(Weapon _weapon)
    {
        foreach (WeaponIcon _wicon in weaponIcon)
        {
            if (_wicon.type == _weapon.WeaponType)
            {
                iconImage.sprite = _wicon.sprite;
                break;
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        uiDodge();
        uiAmmo();
        uiSkillCool();
        uiWeaponChange();
        uiHp();
        uiGold();
    }

    private void uiDodge()
    {
        if (!isDodge && unit.IsDodge)
        {

            dodgeCool.fillAmount = 1;
            isDodge = true;
        }
        if (isDodge)
        {
            dodgeCool.fillAmount -= Time.deltaTime / dodgeCoolTime;

            float remainTime = dodgeCool.fillAmount * dodgeCoolTime;
            int ceilTime = Mathf.CeilToInt(remainTime);

            if (ceilTime > 0.3f)
            {
                dodgeCoolText.text = ceilTime.ToString();
            }
            else
            {
                dodgeCoolText.text = "";
            }

            if (dodgeCool.fillAmount <= 0)
            {
                isDodge = false;
            }
        }
    }

    private void uiAmmo()
    {
        if (unit != null)
        {
            currentAmmo = unit.CurrentAmmo;
            reserveAmmo = unit.ReserveAmmo;
            ammoText.text = $"{currentAmmo}/{reserveAmmo}";
        }
    }

    private void uiWeaponChange()
    {
        weaponCool.fillAmount = unitWeaponChange.ChangeCooldown;

    }

    private void uiSkillCool()
    {
        if (!uesSkill && unitSkill.UseSkill == true)
        {
            uesSkill = true;
            unitSkill.UseSkill = false;
            skillCool.fillAmount = 1;
        }

        if (uesSkill)
        {
            skillCool.fillAmount -= Time.deltaTime / skillCoolTime;

            float remainTime = skillCool.fillAmount * skillCoolTime;
            int ceilTime = Mathf.CeilToInt(remainTime);

            if (ceilTime > 0.3f)
            {
                skillCoolText.text = ceilTime.ToString();
            }
            else
            {
                skillCoolText.text = "";
            }

            if (skillCool.fillAmount <= 0)
            {
                uesSkill = false;
            }
        }
    }

    private void uiHp()
    {
        if (unit != null)
        {
            float current = unit.UnitHp;
            float max = unit.CurrentStat.unitMaxHp;

            hpFill.fillAmount = current / max;
            hpText.text = $"{(int)current}/{(int)max}";
        }
    }

    private void uiGold()
    {
        if (unit != null)
        {
            playerGold.text = unit.Gold.ToString();
        }
    }
}
