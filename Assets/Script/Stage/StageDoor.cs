using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDoor : MonoBehaviour
{

    private bool isPlayerIn = false;
    private Unit unit;

    [SerializeField] private Transform targetTrs;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayerIn = true;
            unit = other.GetComponent<Unit>();
            GameManager.instance.CenterTextObj.SetActive(true);
            GameManager.instance.CenterText.text = "[F] 스테이지 시작";
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
        unit = GameManager.instance.GetUnit;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayerIn) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            stageStart();
        }

    }

    private void stageStart()
    {
        if (GameManager.instance.GetStageNum == 1 )
        {
            GameManager.instance.StartTime = Time.time;
        }
        SoundManager.instance.SFXCreate(SoundManager.Clips.DoorOpen, GameManager.instance.WeaponSoundParent);

        UIManager.instance.FadeWindow.Fade(0.3f);
        unit.gameObject.transform.position = targetTrs.position;
    }
}
