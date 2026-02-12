using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour
{
    [SerializeField] private Sprite shockwaveIcon;
    [SerializeField] private Sprite missileIcon;
    [SerializeField] private UpgradeCard[] upgradeCards;

    private Sprite icon;
    public enum SkillName
    {
        shockwave,
        missile,
    }

    public void SetIcon(string _name)
    {
        switch (_name)
        {
            case "Ludo":
                icon = missileIcon; break;

            case "Luna":
                icon = shockwaveIcon; break;
        }

        if (icon != null)
        {
            for(int i = 0; i < upgradeCards.Length; i++)
            {
                upgradeCards[i].SkillImage.sprite = icon;
            }
        }
    }
}
