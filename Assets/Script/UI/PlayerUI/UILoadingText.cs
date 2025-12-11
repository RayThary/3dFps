using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILoadingText : MonoBehaviour
{
    private TextMeshProUGUI text;
    private string[] loadingSteps = { "Loading.", "Loading..", "Loading..." };
    private float timer = 0;
    [SerializeField]private float nextTime = 0.5f;
    private int stepIndex = 0;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = loadingSteps[stepIndex];
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= nextTime)
        {
            stepIndex++;
            text.text = loadingSteps[stepIndex];
            timer = 0;
            if (stepIndex == 2) stepIndex = -1;
        }
    }
}
