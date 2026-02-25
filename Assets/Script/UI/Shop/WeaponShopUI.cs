using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopUI : MonoBehaviour
{
    [System.Serializable]
    public class ShopSlot
    {
        public Button button;
        public Image weaponIcon;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI priceText;
        public TextMeshProUGUI upgradeText;
    }
    [SerializeField] private List<ShopSlot> shopSlot;
    [SerializeField] private Button exitButton;
    private WeaponView slot1View;
    private WeaponView slot2View;

    [SerializeField] private GameObject shopPanel;
   
    [System.Serializable]
    public struct WeaponIcon
    {
        public eWeaponType weapon;
        public Sprite sprite;
    }

    [SerializeField] private List<WeaponIcon> weaponIcon = new List<WeaponIcon>();
    void Start()
    {
        exitButton.onClick.AddListener(exitShop);
    }

    public void ShopOpen()
    {
        shopPanel.SetActive(true);
    }

    private void exitShop()
    {
        shopPanel.SetActive(false);
        GameManager.instance.UnitStop = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.instance.EscInputLocked = false;
    }

    private void UpgradeSlot(WeaponView view, int _slot)
    {
        if (view == null) return;
        Unit player = GameManager.instance.GetUnit;


        if (player.Gold < view.WeaponUpPrice)
            return;

        player.Gold -= view.WeaponUpPrice;

        // 무기 업그레이드 처리
        shopSlot[_slot].upgradeText.text = view.WeaponUpgrade();

        //업글후 가격조절
        resetPrice();


    }

    private void resetPrice()
    {
        shopSlot[0].priceText.text = $"{slot1View.WeaponUpPrice}";
        shopSlot[1].priceText.text = $"{slot2View.WeaponUpPrice}";
    }

    public void SetWeaponShop(WeaponView _slot1View, string _slot1Name, WeaponView _slot2View, string _slot2Name)
    {
        GameManager.instance.EscInputLocked = true;
        GameManager.instance.UnitStop = true;

        slot1View = _slot1View;
        slot2View = _slot2View;

        shopSlot[0].button.onClick.RemoveAllListeners();
        shopSlot[1].button.onClick.RemoveAllListeners();

        shopSlot[0].weaponIcon.sprite = GetIcon(_slot1Name);
        shopSlot[1].weaponIcon.sprite = GetIcon(_slot2Name);

        shopSlot[0].nameText.text = _slot1Name;
        shopSlot[1].nameText.text = _slot2Name;

        resetPrice();

        shopSlot[0].button.onClick.AddListener(() => UpgradeSlot(slot1View, 0));
        shopSlot[1].button.onClick.AddListener(() => UpgradeSlot(slot2View, 1));
    }

    private Sprite GetIcon(string weaponName)
    {
        for (int i = 0; i < weaponIcon.Count; i++)
        {
            if (weaponIcon[i].weapon.ToString() == weaponName)
                return weaponIcon[i].sprite;
        }

        return null;
    }
}
