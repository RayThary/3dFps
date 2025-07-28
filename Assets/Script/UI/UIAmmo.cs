using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIAmmo : MonoBehaviour
{
    private TextMeshProUGUI ammoText;

    private int cur;
    private int res;

    void Start()
    {
        ammoText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        cur = GameManager.instance.GetUnit.CurrentAmmo;
        res = GameManager.instance.GetUnit.ReserveAmmo;
        ammoText.text = $"{cur}/{res}";
    }

}
