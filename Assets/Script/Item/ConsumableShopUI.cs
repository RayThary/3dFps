using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;


public class ConsumableShopUI : MonoBehaviour
{
    [System.Serializable]
    public class ShopSlot
    {
        public Button button;
        public int ItemAmount;
        public int ItemPrice;
        public int ItemPriceIncrease;
        public TextMeshProUGUI priceText;
    }
    [SerializeField] private List<ShopSlot> shopSlot;
    [SerializeField] private Button exitButton;

    private int ammoBuyCount = 0;

    [SerializeField] private GameObject shopPanel;
    private bool firstOpen = true;

    public void AmmoBuyReset()
    {
        ammoBuyCount = 0;
        firstOpen = true;

        foreach (var slot in shopSlot)
            slot.button.interactable = true;
    }


    void Awake()
    {
        shopSlot[0].button.onClick.AddListener(hpItem);
        shopSlot[1].button.onClick.AddListener(ammoItem);
        shopSlot[2].button.onClick.AddListener(skillItem);

        exitButton.onClick.AddListener(exitShop);
    }

    public void ShopOpen()
    {
        if (firstOpen)
        {
            shopSlot[0].priceText.text = shopSlot[0].ItemPrice.ToString();
            shopSlot[2].priceText.text = shopSlot[2].ItemPrice.ToString();
            shopSlot[1].priceText.text = shopSlot[1].ItemPrice.ToString();

            Unit player = GameManager.instance.GetUnit;
            UnitSkill skill = player.GetComponent<UnitSkill>();
            
            var list = skill.GetAvailableUpgradeCards();
            if (list.Count == 0) return;

            UpgradeType selected = list[Random.Range(0, list.Count)];

            UpgradeCard card = shopSlot[2].button.GetComponent<UpgradeCard>();

            card.ShopSetup(selected, skill);
            firstOpen = false;
        }

        shopPanel.SetActive(true);
        GameManager.instance.ShopOpen = true;
        GameManager.instance.UnitStop = true;
    }


    private void exitShop()
    {
        shopPanel.SetActive(false);
        GameManager.instance.UnitStop = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.instance.ShopOpen = false;
    }


    private void hpItem()
    {
        Unit player = GameManager.instance.GetUnit;

        if (player.Gold < shopSlot[0].ItemPrice)
            return;

        player.Gold -= shopSlot[0].ItemPrice;
        player.UnitHp += Mathf.FloorToInt(shopSlot[0].ItemAmount * Random.Range(0.8f, 1.3f));
        player.UnitHp = Mathf.Min(player.UnitHp, player.CurrentStat.unitMaxHp);

        shopSlot[0].button.interactable = false;
    }

    private void ammoItem()
    {
        Unit player = GameManager.instance.GetUnit;
        ShopSlot slot = shopSlot[1];

        int ammoIncrease = Mathf.FloorToInt(slot.ItemPriceIncrease * Random.Range(0.9f, 1.2f));

        int currentPrice = slot.ItemPrice + (ammoBuyCount * ammoIncrease);
        if (player.Gold < currentPrice)
            return;

        player.Gold -= currentPrice;

        player.UnitWeapon.BuyAddAmmo();

        ammoBuyCount++;

        int nextPrice = slot.ItemPrice + (ammoBuyCount * ammoIncrease);
        slot.priceText.text = nextPrice.ToString();
    }

    private void skillItem()
    {
        Unit player = GameManager.instance.GetUnit;
        UnitSkill skill = player.GetComponent<UnitSkill>();

        // 가격 체크
        if (player.Gold < shopSlot[2].ItemPrice)
            return;

        player.Gold -= shopSlot[2].ItemPrice;

        // 업그레이드 적용
        UpgradeCard card = shopSlot[2].button.GetComponent<UpgradeCard>();
        UpgradeType selected = card.GetUpgradeType;

        switch (skill.SkillName)
        {
            case UnitSkill.eSkillName.Shockwave:
                skill.ShockwaveSkillUpgrade(selected);
                break;
            case UnitSkill.eSkillName.ThrowMissile:
                skill.MissileSkillUpgrade(selected);
                break;
        }

        shopSlot[2].button.interactable = false;


    }

}
