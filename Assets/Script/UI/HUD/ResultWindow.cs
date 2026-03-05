using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ResultText;
    [SerializeField] private Image playerIcon;
    [SerializeField] private Image weaponIcon1;
    [SerializeField] private Image weaponIcon2;
    [SerializeField] private TextMeshProUGUI playTime;
    [SerializeField] private Button lobbyBtn;

    void Start()
    {
        lobbyBtn.onClick.AddListener(lobbyButton);
    }

    private void lobbyButton()
    {
        gameObject.SetActive(false);
        UIManager.instance.PauseExit();
    }

    public void GameResult(Unit _unit, bool _clear)
    {
        gameObject.SetActive(true);
        GameManager.instance.UnitStop = true;
        GameManager.instance.GetUnit.unitStopVelocity();
        Cursor.lockState = CursorLockMode.None;

        float startTime = GameManager.instance.StartTime;
        float playingTime = Time.time - startTime;

        int minutes = (int)(playingTime / 60);
        int seconds = (int)(playingTime % 60);

        playTime.text = $"{minutes:00}:{seconds:00}";

        Time.timeScale = 0;

        if (_clear)
        {
            ResultText.text = "Game Clear";
        }
        else
        {
            ResultText.text = "You Died";
        }
        weaponIcon1.sprite = _unit.CurrentSlot.weaponSlot[1].WeaponIcon;
        weaponIcon2.sprite = _unit.CurrentSlot.weaponSlot[2].WeaponIcon;




    }
}
