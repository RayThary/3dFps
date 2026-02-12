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

    private float startTime;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameResult(Unit _unit, bool _clear)
    {
        if (_clear)
        {
            ResultText.text = "Game Clear";
        }
        else
        {
            ResultText.text = "You Died";
        }


    }
}
