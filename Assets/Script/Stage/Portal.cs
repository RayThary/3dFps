using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawn : MonoBehaviour
{
    private CapsuleCollider capCol;
    private bool isPlayerIn = false;

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
    public bool test = false;
    // Update is called once per frame
    void Update()
    {
        if (test)
        {
            UIManager.instance.GetSkillUpgradeUI.OpenUpgradeUI();
            test = false;
        }

        if (!isPlayerIn)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            UIManager.instance.GetSkillUpgradeUI.OpenUpgradeUI();
        }

    }
}
