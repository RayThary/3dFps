using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionData
{
    public FullScreenMode ScreenMode = FullScreenMode.FullScreenWindow;
    public int ResolutionIndex = 0;
    public int FrameRate = 60;
    public float MasterVolume = 1f;
    public float BGMVolume = 0.8f;
    public float SFXVolume = 0.8f;
    public float Sensitivity = 2.5f;

}
