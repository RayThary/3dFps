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

    //½ºÅ³
    private UnitSkill unitSkill;
    private float skillCoolTime;
    private bool uesSkill = false;

    //´ë½Ã
    [SerializeField]
    private Image dodgeCool;

    private UnitDodge unitDodge;
    private bool isDodge;
    private float dodgeCoolTime;

    //ÃÑ¾Ë
    [SerializeField]
    private TextMeshProUGUI ammoText;

    private int currentAmmo;
    private int reserveAmmo;

    //ÃÑ ¾ÆÀÌÄÜ
    [System.Serializable]
    public struct WeaponIcon
    {
        public eWeaponType type;
        public Sprite sprite;
    }

    [SerializeField] private Image iconImage;
    [SerializeField] private Image weaponCool;
    [SerializeField] private Image skillCool;
    [SerializeField] private List<WeaponIcon> weaponIcon = new List<WeaponIcon>();

    //Ã¼·Â¹Ù
    [SerializeField] private Image hpFill;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Image unitIcon;
    [SerializeField] private List<Sprite> unitIcons = new List<Sprite>();

    //°ñµå
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
