using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public WeaponView CurrentWeaponView { get; private set; }

    private Unit unit;
    public Unit GetUnit { get { return unit; } }
    public Unit SetUnit { set { unit = value; } }

    private CameraManager cameraManager;
    public CameraManager GetCameraManager { get { return cameraManager; } }

    //일단 체크f부분만 리턴나중에 많이쓸경우에 캔버스로두고 따로자식으로 개개인별로찾아주는게좋을거같음
    private GameObject checkF;
    public GameObject CheckF { get { return checkF; } }
    private Transform worldnParent;

    public Transform GetWorldParent { get { return worldnParent; } }

    private Dictionary<string, Transform> poolingParents = new();
    public Dictionary<string, Transform> PoolingParents { get { return poolingParents; } }

    [SerializeField] private Transform poolingRoot;
    public Transform GetPoolinRoot { get { return poolingRoot; } }

    [SerializeField] private Transform weaponSoundParent;
    public Transform WeaponSoundParent { get { return weaponSoundParent; } }

    private bool isStageStart = false;
    public bool IsStageStarted { get { return isStageStart; } set { isStageStart = value; } }

    private Image loadingBar;
    private bool isLoading = false;
    public bool GetisLoading { get { return isLoading; } }

    private int nextStageNum = 1;
    [SerializeField]
    private int stageNum = 1;
    public int GetStageNum { get { return stageNum; } }

    [SerializeField] private GameObject zoomScope;
    public GameObject ZoomScope { get { return zoomScope; } }
    private Image crosshair;
    public Image Crosshair { get { return crosshair; } }
    private Image hitCrosshair;
    public Image HitCrosshair { get { return hitCrosshair; } }
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
        DontDestroyOnLoad(gameObject);

        unit = FindObjectOfType<Unit>();
        cameraManager = GetComponentInChildren<CameraManager>();
        UnityEngine.Random.InitState(Environment.TickCount);

    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    //초기화될때마다 새로해줄것들
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject worldObject = GameObject.Find("WorldObjects");
        worldnParent = worldObject.transform.Find("WeaponParent");

        if (SceneManager.sceneCount == 0)
        {
            return;
        }

        isStageStart = false;
        nextStageNum++;
        Initialize();
    }

    private void Initialize()
    {
        //캔버스
        GameObject canvas = GameObject.Find("Canvas");
        if (checkF == null)
        {
            checkF = canvas.transform.Find("PlayerUI/CheckF").gameObject;
        }

        if (crosshair == null || hitCrosshair == null)
        {
            crosshair = canvas.transform.Find("PlayerUI/Crosshair").GetComponent<Image>();
            hitCrosshair = crosshair.transform.GetChild(0).GetComponent<Image>();
        }

        if (zoomScope == null)
        {
            zoomScope = canvas.transform.Find("PlayerUI/ZoomScope").gameObject;
        }

        //카메라
        if (unit != null && cameraManager != null)
            cameraManager.InitializeCamera(unit, unit.transform);

        //플레이어
        if (unit != null)
        {
            unit.transform.position = new Vector3(0, 0, -50);
            unit.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }

    void Start()
    {

    }

    [Tooltip("테스트용")]
    public bool stageChange = false;

    // Update is called once per frame
    void Update()
    {
        if (stageChange)
        {
            StartCoroutine(loadSceneWithLoading(nextStageNum));
            stageChange = false;
        }
    }
    IEnumerator loadSceneWithLoading(int _stageNum)
    {
        SoundManager sm = SoundManager.instance;
        switch (nextStageNum)
        {
            case 2:
                StartCoroutine(sm.BGMSoundChange(sm.BackGroundClip[1])); break;
            case 4:
                StartCoroutine(sm.BGMSoundChange(sm.BackGroundClip[2])); break;
            case 6:
                StartCoroutine(sm.BGMSoundChange(sm.BackGroundClip[3])); break;
        }

        GameObject obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.LoadingCanvas, worldnParent);
        loadingBar = obj.transform.Find("Loading/LoadingBar").GetComponent<Image>();
        isLoading = true;
        AsyncOperation op = SceneManager.LoadSceneAsync(_stageNum);
        op.allowSceneActivation = false;

        while (op.progress < 0.89f)
        {

            //게임자체적으로 로딩이늘어나면 op.progress로 로딩바를 바꿔줘야함
            yield return null;
        }

        float timer = 0;
        float fakeDuration = 3;
        float startFill = loadingBar.fillAmount == 0 ? 0 : op.progress;

        while (timer < fakeDuration)
        {
            timer += Time.deltaTime;
            loadingBar.fillAmount = Mathf.Lerp(startFill, 1, timer / fakeDuration);
            yield return null;
        }

        isLoading = false;
        stageNum++;

        //loadingBar.fillAmount = 0;
        //PoolingManager.Instance.RemovePoolingObject(obj);
        op.allowSceneActivation = true;
    }

}
