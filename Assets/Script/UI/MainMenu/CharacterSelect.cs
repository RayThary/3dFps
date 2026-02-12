using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{

    [SerializeField] private Button b_Ludo;
    [SerializeField] private Button b_Luna;

    [SerializeField] private GameObject g_Ludo;
    [SerializeField] private GameObject g_Luna;

    private SkillIcon skillIcon;


    void Start()
    {
        b_Ludo.onClick.AddListener(ludoCreate);
        b_Luna.onClick.AddListener(lunaCreate);

        skillIcon = GetComponentInParent<SkillIcon>();
    }

    private void ludoCreate()
    {
        GameManager.instance.PlayerCharacter = g_Ludo;
        string name = "Ludo";
        GameManager.instance.UnitName = name;
        skillIcon.SetIcon(name);
        SceneManager.LoadSceneAsync(1);
        gameObject.SetActive(false);
    }
    private void lunaCreate()
    {
        GameManager.instance.PlayerCharacter = g_Luna;
        string name = "Luna";
        GameManager.instance.UnitName = name;
        skillIcon.SetIcon(name);
        SceneManager.LoadSceneAsync(1);
        gameObject.SetActive(false);
    }
}
