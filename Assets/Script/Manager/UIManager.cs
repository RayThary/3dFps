using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject baseWindow;
    [SerializeField] private GameObject characterWindow;
    [SerializeField] private GameObject optionWindow;
    [SerializeField] private GameObject pauseWindow;
    [SerializeField] private GameObject loadingBarWindow;
    public GameObject LoadingBar { get { return loadingBarWindow; } }


    [SerializeField] private Slider s_MasterSound;
    [SerializeField] private Slider s_BGM;
    [SerializeField] private Slider s_SFX;

    [SerializeField] private TMP_Dropdown d_Resolutions;
    private readonly List<Resolution> resolutions = new();
    [SerializeField] private TMP_Dropdown d_FPS;
    private readonly List<int> frameRate = new();
    [SerializeField] private TMP_Dropdown d_ScreenMode;
    private readonly List<FullScreenMode> screenModes = new();

    [SerializeField] private Button optionClose;

    [SerializeField] private Slider s_Sensitivity;

    [SerializeField] private SkillUpgradeUI skillUpgradeUI;
    public SkillUpgradeUI GetSkillUpgradeUI { get { return skillUpgradeUI; } }

    [SerializeField] private FadeWinodw fadeWinodw;
    public FadeWinodw FadeWindow { get { return fadeWinodw; } }

    [SerializeField] private ResultWindow resultWindow;
    public ResultWindow ResultWindow { get { return resultWindow; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        InitResolutionOptions();
        InitFPSOption();
        InitScreenModeOption();


        d_Resolutions.onValueChanged.AddListener(ApplyResolution);
        d_FPS.onValueChanged.AddListener(ApplyFPS);

        SoundManager.instance.SetMasterSound(s_MasterSound);
        SoundManager.instance.SetBGMSound(s_BGM);
        SoundManager.instance.SetSFXSound(s_SFX);
        optionClose.onClick.AddListener(OnButtonOptionClose);

        LoadOptionData();
        DontDestroyOnLoad(gameObject);
    }

    private void LoadOptionData()
    {
        string path = Application.persistentDataPath + "/option.json";
        if (!File.Exists(path))
        {

            Debug.Log("옵션값이없어서 초기화합니다.");
            return;
        }

        string json = File.ReadAllText(path);
        OptionData data = JsonUtility.FromJson<OptionData>(json);

        //해상도
        if (data.ResolutionIndex < resolutions.Count)
        {
            Resolution r = resolutions[data.ResolutionIndex];
            Screen.SetResolution(r.width, r.height, data.ScreenMode);
            d_Resolutions.value = data.ResolutionIndex;
            d_Resolutions.RefreshShownValue();
        }
        else
        {
            data.ResolutionIndex = 0;

            if (resolutions.Count > 0)
            {
                Resolution r = resolutions[0];
                Screen.SetResolution(r.width, r.height, data.ScreenMode);
                d_Resolutions.value = 0;
            }
        }
        int modeIndex = screenModes.IndexOf(data.ScreenMode);

        if (modeIndex >= 0)
        {
            d_ScreenMode.value = modeIndex;
            d_ScreenMode.RefreshShownValue();
        }

        Screen.fullScreenMode = data.ScreenMode;

        //프레임
        Application.targetFrameRate = data.FrameRate == -1 ? -1 : data.FrameRate;

        int fpsIntdx = frameRate.IndexOf(data.FrameRate);
        if (fpsIntdx >= 0)
        {
            d_FPS.value = fpsIntdx;
            d_FPS.RefreshShownValue();
        }

        //사운드
        s_MasterSound.value = data.MasterVolume;
        s_BGM.value = data.BGMVolume;
        s_SFX.value = data.SFXVolume;

        s_Sensitivity.value = data.Sensitivity;
    }
    private void InitResolutionOptions()
    {
        d_Resolutions.options.Clear();
        resolutions.Clear();

        float targetAspect = (float)Screen.currentResolution.width / Screen.currentResolution.height;
        Resolution[] allResolutions = Screen.resolutions;

        int currentIndex = 0;
        for (int i = 0; i < allResolutions.Length; i++)
        {
            float aspect = (float)allResolutions[i].width / allResolutions[i].height;

            if (Mathf.Abs(aspect - targetAspect) > 0.01f)
                continue;

            resolutions.Add(allResolutions[i]);

            TMP_Dropdown.OptionData optionData = new()
            { text = $"{allResolutions[i].width} x {allResolutions[i].height} {Mathf.RoundToInt((float)allResolutions[i].refreshRateRatio.value)}hz" };

            d_Resolutions.options.Add(optionData);

            if (allResolutions[i].width == Screen.width &&
                allResolutions[i].height == Screen.height)
            {
                currentIndex = resolutions.Count - 1;
            }

        }

        d_Resolutions.value = currentIndex;
        d_Resolutions.RefreshShownValue();
    }

    private void InitFPSOption()
    {
        d_FPS.options.Clear();
        if (Application.targetFrameRate > 240)
            Application.targetFrameRate = 240;
        frameRate.Add(-1);
        frameRate.Add(240);
        frameRate.Add(120);
        frameRate.Add(60);
        frameRate.Add(30);

        for (int i = 0; i < frameRate.Count; i++)
        {
            string text = frameRate[i] == -1 ? "Unlimited" : frameRate[i].ToString();
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData(text);
            d_FPS.options.Add(data);
        }
        d_FPS.RefreshShownValue();
    }

    private void InitScreenModeOption()
    {
        d_ScreenMode.options.Clear();
        screenModes.Clear();


        List<string> modeNames = new() { "전체화면", "테두리 없는 전체창", "창 모드" };


        screenModes.Add(FullScreenMode.ExclusiveFullScreen);
        screenModes.Add(FullScreenMode.FullScreenWindow);
        screenModes.Add(FullScreenMode.Windowed);

        // Dropdown 옵션 세팅
        for (int i = 0; i < modeNames.Count; i++)
        {
            d_ScreenMode.options.Add(new TMP_Dropdown.OptionData(modeNames[i]));
        }

        d_ScreenMode.RefreshShownValue();
    }
    private void saveOption()
    {
        OptionData data = new OptionData();
        data.ResolutionIndex = d_Resolutions.value;
        data.FrameRate = frameRate[d_FPS.value];
        data.ScreenMode = screenModes[d_ScreenMode.value];

        data.MasterVolume = s_MasterSound.value;
        data.BGMVolume = s_BGM.value;
        data.SFXVolume = s_SFX.value;
        data.Sensitivity = s_Sensitivity.value;

        GameManager.instance.characterSensitivity(s_Sensitivity.value);

        string json = JsonUtility.ToJson(data, true);
        string path = Application.persistentDataPath + "/option.json";
        File.WriteAllText(path, json);

        saveSensitivity();
    }

    private void saveSensitivity()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            Unit unit = GameManager.instance.GetUnit;
            if (unit != null)
            {
                unit.Sensitivity = s_Sensitivity.value * 0.5f;
            }
        }
    }

    private void ApplyScreenOption()
    {
        FullScreenMode mode = screenModes[d_ScreenMode.value];
        Screen.fullScreenMode = mode;
    }

    public void PauseKey()
    {
        Time.timeScale = 0;
        GameManager.instance.IsPaused = true;
        pauseWindow.SetActive(true);
    }

    public void PauseOption()
    {
        optionWindow.SetActive(true);
    }

    public void PauseContinue()
    {
        Time.timeScale = 1;
        GameManager.instance.IsPaused = false;
        pauseWindow.SetActive(false);
    }
    public void PauseExit()
    {
        pauseWindow.SetActive(false);

        if (GameManager.instance.GetUnit != null)
        {
            GameObject obj = GameManager.instance.GetUnit.gameObject;
            GameManager.instance.SetUnit = null;
            GameManager.instance.RemoveEnemy();
            GameManager.instance.RemovePoolingRoot();
            Destroy(obj);
        }


        GameManager.instance.IsPaused = false;
        SceneManager.LoadSceneAsync(0);
        baseWindow.SetActive(true);
        //timeScale은 씬에서 초기화되게 해줌
    }
    public void OnButtonInGameOption()
    {
        optionWindow.SetActive(true);
    }

    public void OnButtonStart()
    {
        baseWindow.SetActive(false);
        characterWindow.SetActive(true);
    }

    public void OnButtonOption()
    {
        baseWindow.SetActive(false);
        optionWindow.SetActive(true);
    }

    public void OnButtonOptionClose()
    {
        ApplyScreenOption();
        saveOption();
        optionWindow.SetActive(false);
        if (SceneManager.GetActiveScene().buildIndex == 0)
            baseWindow.SetActive(true);
    }

    private void ApplyResolution(int index)
    {
        Resolution r = resolutions[index];


        Screen.SetResolution(r.width, r.height, Screen.fullScreenMode);
    }

    private void ApplyFPS(int index)
    {
        int selected = frameRate[index];
        Application.targetFrameRate = selected == -1 ? -1 : selected;
    }

    public void OnButtonExit()
    {
        Application.Quit();
    }
}
