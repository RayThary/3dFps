using System;
using System.Collections;
using System.Collections.Generic;
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
    private Transform weaponParent;

    public Transform GetWeaponParent { get { return weaponParent; } }

    private Dictionary<string, Transform> poolingParents = new();
    public Dictionary<string, Transform> PoolingParents { get { return poolingParents; } }

    [SerializeField] private Transform poolingRoot;
    public Transform GetPoolinRoot { get { return poolingRoot; } }
    [Tooltip("테스트용 꼭지워줄것")]
    [SerializeField]//테스트용
    private bool isStageStart = false;
    public bool IsStageStarted { get { return isStageStart; } set { isStageStart = value; } }

    private Image loadingBar;
    private bool isLoading = false;
    public bool GetisLoading { get { return isLoading; } }

    private int nextStageNum = 0;
    private int stageNum = 0;
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
        weaponParent = worldObject.transform.Find("WeaponParent");

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
    }

    void Start()
    {

    }

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
        GameObject obj = PoolingManager.Instance.CreateObject(PoolingManager.ePoolingObject.LoadingCanvas, null);
        loadingBar = obj.transform.Find("Loading/LoadingBar").GetComponent<Image>();
        isLoading = true;
        AsyncOperation op = SceneManager.LoadSceneAsync(_stageNum);
        op.allowSceneActivation = false;
        Debug.Log(op);

        while (op.progress < 0.89f)
        {

            //게임자체적으로 로딩이늘어나면 op.progress로 로딩바를 바꿔줘야함
            Debug.Log(op.progress);
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

        loadingBar.fillAmount = 0;
        PoolingManager.Instance.RemovePoolingObject(obj);
        op.allowSceneActivation = true;
    }

}
