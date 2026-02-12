using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGuideUI : MonoBehaviour
{
    [SerializeField] private float guideRemoveTime = 2;

    private CanvasGroup canvasGroup;
    private float startAlpha;
    private float t = 1;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        startAlpha = 0.8f;
    }

    // Update is called once per frame
    void Update()
    {

        if (t >= 0)
        {
            t -= Time.deltaTime / guideRemoveTime;
            t = Mathf.Clamp01(t);
            canvasGroup.alpha = Mathf.Lerp(0, startAlpha, t);
        }

    }
}
