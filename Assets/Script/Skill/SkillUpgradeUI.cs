using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUpgradeUI : MonoBehaviour
{
    private UnitSkill unitSkill;
    [SerializeField] private UpgradeCard[] cards;

    [SerializeField]private GameObject skillUI;
    public void OpenUpgradeUI()
    {
        skillUI.SetActive(true);
        unitSkill = GameManager.instance.GetUnit.GetComponent<UnitSkill>();

        var list = unitSkill.GetAvailableUpgradeCards();

        var selected = GetRandomUpgrades(list, 3);

        // 3) UI에 세팅
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].Setup(selected[i], unitSkill);
        }

        // 4) 게임 멈춤(선택 강제)
        Time.timeScale = 0f;
    }

    public void Close()
    {
        Time.timeScale = 1f;
        skillUI.SetActive(false);
        GameManager.instance.StageChange = true;
    }

    private List<UpgradeType> GetRandomUpgrades(List<UpgradeType> list, int count)
    {
        List<UpgradeType> result = new();

        for (int i = 0; i < count; i++)
        {
            int idx = Random.Range(0, list.Count);
            result.Add(list[idx]);
            list.RemoveAt(idx);
        }

        return result;
    }
}
