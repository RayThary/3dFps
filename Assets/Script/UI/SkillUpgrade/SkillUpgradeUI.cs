using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillUpgradeUI : MonoBehaviour
{
    private UnitSkill unitSkill;
    [SerializeField] private UpgradeCard[] cards;

    [SerializeField] private GameObject skillUI;

    [SerializeField] private GameObject clearUI;


    public void StageClearText()
    {
        StartCoroutine(stageClearText());
    }
    private IEnumerator stageClearText()
    {
        clearUI.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        clearUI.SetActive(false);
    }

    public void OpenUpgradeUI()
    {
        skillUI.SetActive(true);
        unitSkill = GameManager.instance.GetUnit.GetComponent<UnitSkill>();
        GameManager.instance.UnitStop = true;
        GameManager.instance.GetUnit.unitStopVelocity();
        GameManager.instance.EscInputLocked = true;

        var list = unitSkill.GetAvailableUpgradeCards();

        var selected = GetRandomUpgrades(list, 3);

        //UI¿¡ ¼¼ÆÃ
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].Setup(selected[i], unitSkill);
        }

        Cursor.lockState = CursorLockMode.None;

        //°ÔÀÓ ¸ØÃã
        Time.timeScale = 0f;
    }

    public void Close()
    {
        Time.timeScale = 1f;
        skillUI.SetActive(false);
        GameManager.instance.UnitStop = false;
        GameManager.instance.StageChange();
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
