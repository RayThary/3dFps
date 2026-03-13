using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawn : MonoBehaviour
{
    private CapsuleCollider capCol;
    private bool isPlayerIn = false;
    private bool checkF = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayerIn = true;
            GameManager.instance.CenterTextObj.SetActive(true);
            GameManager.instance.CenterText.text = "[F] 넘어가기";
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayerIn = false;
            checkF = false;
            GameManager.instance.CenterTextObj.SetActive(false);
        }
    }
    void Start()
    {
        capCol = GetComponent<CapsuleCollider>();
        GameManager.instance.Portal = gameObject;
        if (GameManager.instance.GetStageNum != 0)
            gameObject.SetActive(false);

    }

    void Update()
    {
      
        if (!isPlayerIn || checkF)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            checkF = true;
            if (GameManager.instance.GetStageNum < 5)
            {
                UIManager.instance.GetSkillUpgradeUI.OpenUpgradeUI();
            }
            else
            {
                UIManager.instance.ResultWindow.GameResult(GameManager.instance.GetUnit, true);
            }
        }

    }
}
