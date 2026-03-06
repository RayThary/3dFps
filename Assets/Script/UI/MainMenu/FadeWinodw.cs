using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeWinodw : MonoBehaviour
{

    [SerializeField] private Image fadeImage;

    public void Fade(float _time)
    {
        StartCoroutine(fadeStart(_time));
        GameManager.instance.UnitStop = false;
    }

    IEnumerator fadeStart(float _time)
    {
        fadeImage.enabled = true;
        yield return new WaitForSeconds(_time);
        fadeImage.enabled = false;
    }
}
