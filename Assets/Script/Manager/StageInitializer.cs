using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInitializer : MonoBehaviour
{
    void Start()
    {
        Transform unit = GameManager.instance.GetUnit.transform;

        StartCoroutine(soundCheck());
    }

    private IEnumerator soundCheck()
    {
        yield return new WaitForSeconds(0.2f);
        SoundManager.instance.BGMSoundPause(false);
    }
}
