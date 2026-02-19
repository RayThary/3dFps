using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHp : MonoBehaviour
{
    [SerializeField] private Image hpImage;
    [SerializeField] private Enemy boss;
    private GameObject hpUI;
    [SerializeField]
    private float maxHp;

    private bool stageStart = false;

    void Start()
    {
        stageStart = false;
        hpUI = GetComponentInChildren<Image>().gameObject;
        hpUI.SetActive(false);

        StartCoroutine(WaitStageStart());
    }

    private IEnumerator WaitStageStart()
    {
        yield return new WaitUntil(() => GameManager.instance.IsStageStarted);

        maxHp = boss.Hp;
        hpImage.fillAmount = (float)boss.Hp / maxHp;
        hpUI.SetActive(true);
        stageStart = true;
    }



    void Update()
    {
        if (boss == null)
            return;

        if (stageStart)
        {
            hpImage.fillAmount = (float)boss.Hp / maxHp;
        }
    }
}
