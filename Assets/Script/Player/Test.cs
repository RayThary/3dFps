using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public bool test;
    public Image a1;
    public Sprite a;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        //if (test)
        //{
        //    UIManager.instance.GetSkillUpgradeUI.OpenUpgradeUI();
        //    test = false;
        //}

        if (test)
        {
            a1.sprite = a;
            test = false;
        }


    }
}
