using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnitSkill;

public class UpgradeCard : MonoBehaviour
{
    [SerializeField] private Image skillImage;
    [SerializeField] private TextMeshProUGUI skillText;//스킬설명
    [SerializeField] private TextMeshProUGUI desc;//스킬업글수치
    [SerializeField] private Button button;

    private UpgradeType type;
    public UpgradeType GetUpgradeType { get { return type; } }
    [SerializeField] private SkillUpgradeUI skillUpUI;


    public void Setup(UpgradeType _type, UnitSkill _unitSkill)
    {
        type = _type;
        skillText.text = GetTitle(type);
        desc.text = GetValueText(type, _unitSkill);
        button.onClick.AddListener(() => OnUpgradeClicked(_unitSkill));
    }
    public void ShopSetup(UpgradeType _type, UnitSkill _unitSkill)
    {
        type = _type;
        skillText.text = GetTitle(type);
        desc.text = GetValueText(type, _unitSkill);
    }

    private void OnUpgradeClicked(UnitSkill _unitSkill)
    {
        switch (_unitSkill.SkillName)
        {
            case eSkillName.Shockwave:
                _unitSkill.ShockwaveSkillUpgrade(type);
                break;
            case eSkillName.ThrowMissile:
                _unitSkill.MissileSkillUpgrade(type);
                break;
        }

        if (skillUpUI != null)
            skillUpUI.Close();
    }

    private string GetTitle(UpgradeType type)
    {

        switch (type)
        {
            // 공통
            case UpgradeType.Damage:
                return "스킬 피해 증가";

            case UpgradeType.CoolDown:
                return "스킬 쿨타임 감소";


            case UpgradeType.Radius:
                return "충격파 범위 증가";

            case UpgradeType.Slow:
                return "감속 지속시간 증가";

            case UpgradeType.ResidueShockwave:
                return "충격파 지속시간 증가";

            case UpgradeType.DoubleShockwave:
                return "이중 충격파 발동";


            case UpgradeType.MissileCount:
                return "미사일 발사 수 증가";

            case UpgradeType.FireInterval:
                return "발사 간격 감소";

            case UpgradeType.MissileSpeed:
                return "미사일 속도 증가";

            case UpgradeType.CriticalEnable:
                return "치명타 발동";

            default:
                return "강화";
        }
    }

    private string GetValueText(UpgradeType type, UnitSkill _unitSkill)
    {

        switch (type)
        {
            //공용업글
            case UpgradeType.Damage:
                if (_unitSkill.SkillName == eSkillName.Shockwave)
                    return GetLevelBar(_unitSkill.shockwaveUpLevel.damageLevel, _unitSkill.shockwaveUpLevel.damageMaxLevel);
                else
                    return GetLevelBar(_unitSkill.missileUpLevel.damageLevel, _unitSkill.missileUpLevel.damageMaxLevel);

            case UpgradeType.CoolDown:
                if (_unitSkill.SkillName == eSkillName.Shockwave)
                    return GetLevelBar(_unitSkill.shockwaveUpLevel.coolTimeLevel, _unitSkill.shockwaveUpLevel.coolTimeMaxLevel);
                else
                    return GetLevelBar(_unitSkill.missileUpLevel.coolDownLevel, _unitSkill.missileUpLevel.coolDownMaxLevel);


            //쇼크웨이브 전용 업글
            case UpgradeType.Radius:
                return GetLevelBar(_unitSkill.shockwaveUpLevel.radiusLevel, _unitSkill.shockwaveUpLevel.radiusMaxLevel);

            case UpgradeType.Slow:
                return GetLevelBar(_unitSkill.shockwaveUpLevel.slowLevel, _unitSkill.shockwaveUpLevel.slowMaxLevel);

            case UpgradeType.ResidueShockwave:
                return GetLevelBar(_unitSkill.shockwaveUpLevel.radiusLevel, _unitSkill.shockwaveUpLevel.residueMaxLevel);

            case UpgradeType.DoubleShockwave:
                return GetLevelBar(_unitSkill.shockwaveUpLevel.doubleShockwaveLevel, _unitSkill.shockwaveUpLevel.doubleShockwaveMaxLevel);

            //미사일 전용 업글
            case UpgradeType.MissileCount:
                return GetLevelBar(_unitSkill.missileUpLevel.missileCountLevel, _unitSkill.missileUpLevel.missileCountMaxLevel);

            case UpgradeType.FireInterval:
                return GetLevelBar(_unitSkill.missileUpLevel.fireIntervalLevel, _unitSkill.missileUpLevel.fireIntervalMaxLevel);

            case UpgradeType.MissileSpeed:
                return GetLevelBar(_unitSkill.missileUpLevel.missileSpeedLevel, _unitSkill.missileUpLevel.missileSpeedMaxLevel);

            case UpgradeType.CriticalEnable:
                return GetLevelBar(_unitSkill.missileUpLevel.criticalEnableLevel, _unitSkill.missileUpLevel.criticalEnableMaxLevel);

            default:
                return "";
        }
    }

    private string GetLevelBar(int level, int maxLevel)
    {
        string filled = new string('●', level);
        string empty = new string('○', maxLevel - level);
        return filled + empty;
    }
}
