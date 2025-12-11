using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableShop : MonoBehaviour
{
    private bool isPlayerIn = false;
    private Unit unit;

    private Button hpButton;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayerIn = true;
            unit = other.GetComponent<Unit>();
            GameManager.instance.CenterTextObj.SetActive(true);
            GameManager.instance.CenterText.text = "[F] 상점열기";
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
    // Update is called once per frame
    void Update()
    {
        if (!isPlayerIn) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            openShop();
        }
    }
    private void openShop()
    {
        GameManager.instance.UnitStop = true;
        Cursor.lockState = CursorLockMode.None;

        ShopUI.instance.ConsumableShopUI.ShopOpen();
        GameManager.instance.CenterTextObj.SetActive(false);
    }
    
}
