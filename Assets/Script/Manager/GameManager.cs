using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private Unit unit;
    public Unit GetUnit { get { return unit; } }
    public Unit SetUnit { set { unit = value; } }

    private CameraManager cameraManager;
    public CameraManager GetCameraManager { get { return cameraManager; } }


    //UI 관련
    private GameObject centerTextObj;
    public GameObject CenterTextObj { get { return centerTextObj; } }

    private TextMeshProUGUI centerText;
    public TextMeshProUGUI CenterText { get { return centerText; } set { centerText = value; } }

    [SerializeField] private GameObject zoomScope;
    public GameObject ZoomScope { get { return zoomScope; } }

    private Image crosshair;
    public Image Crosshair { get { return crosshair; } }

    private Image hitCrosshair;
    public Image HitCrosshair { get { return hitCrosshair; } }

    private GameObject playerCharacter;
    public GameObject PlayerCharacter { get { return playerCharacter; } set { playerCharacter = value; } }

    private GameObject currentCharacter;

    //풀링 관련
    private Dictionary<string, Transform> poolingParents = new();
    public Dictionary<string, Transform> PoolingParents { get { return poolingParents; } }

    [SerializeField] private Transform poolingRoot;
    public Transform GetPoolinRoot { get { return poolingRoot; } }
    private Transform worldnParent;
    public Transform GetWorldParent { get { return worldnParent; } }

    [SerializeField] private Transform weaponSoundParent;
    public Transform WeaponSoundParent { get { return weaponSoundParent; } }

    //스테이지 / 로딩 관련
    private bool isStageStart = false;
    public bool IsStageStarted { get { return isStageStart; } set { isStageStart = value; } }

    private int nextStageNum = 1;

    [SerializeField] private int stageNum = 1;
    public int GetStageNum { get { return stageNum; } }

    private GameObject loadingBarObj;
    private Image loadingBar;

    private bool isLoading = false;
    public bool GetisLoading { get { return isLoading; } }

    //캐릭터 관련
    private string unitName;
    public string UnitName { get { return unitName; } set { unitName = value; } }

    //게임환경관련 (Pause / Stop 등)
    private bool isPaused = false;
    public bool IsPaused { get { return isPaused; } set { isPaused = value; } }

    private bool unitStop = false;
    public bool UnitStop { get { return unitStop; } set { unitStop = value; } }

    //스테이지 관련
    private bool isEscInputLocked;
    public bool EscInputLocked { get { return isEscInputLocked; } set { isEscInputLocked = value; } }


    private int enemyCount = 0;
    public int EnemyMaxCount { set { enemyCount = value; } }
    private GameObject portalObj;
    public GameObject Portal { get { return portalObj; } set { portalObj = value; } }

    //씬전용 
    private SpawnSetting _spawnSetting;
    public SpawnSetting SetSpawnSetting { set { _spawnSetting = value; } }

    private float startTime;
    public float StartTime { get { return startTime; } set { startTime = value; } }

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


        // 로비씬 넘어가기
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            stageNum = 0;
            return;
        }

        //월드오브젝트 다시설정
        GameObject worldObject = GameObject.Find("WorldObjects");
        if (worldObject != null)
            worldnParent = worldObject.transform.Find("WeaponParent");

        //초기화할것들
        isStageStart = false;
        nextStageNum++;
        isEscInputLocked = false;

        //Awake 초기화보다 먼저 Unit이 존재해야 함
        characterCreate();

        Initialize();
        ShopUI.instance.ConsumableShopUI.AmmoBuyReset();

        //로딩바
        if (loadingBarObj != null)
        {
            loadingBarObj.SetActive(false);
            loadingBar.fillAmount = 0;
        }


        unitStop = false;
    }

    private void Initialize()
    {

        //캔버스
        GameObject canvas = GameObject.Find("SceenCanvas");
        if (centerTextObj == null)
        {
            centerTextObj = canvas.transform.Find("PlayerUI/CheckF").gameObject;
            centerText = centerTextObj.GetComponentInChildren<TextMeshProUGUI>();
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


    private void characterCreate()
    {

        if (SceneManager.GetActiveScene().buildIndex != 1)
            return;

        if (currentCharacter == null)
        {
            GameObject character = Instantiate(playerCharacter);

            currentCharacter = character;

            unit = character.GetComponent<Unit>();

            CinemachineVirtualCamera pov = unit.GetComponentInChildren<CinemachineVirtualCamera>();
            cameraManager.SetPovCam(pov);

            OptionData data = LoadOptionData();
            unit.Sensitivity = data.Sensitivity / 2;
        }
    }

    private OptionData LoadOptionData()
    {
        string path = Application.persistentDataPath + "/option.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<OptionData>(json);
        }
        else
        {
            return null; // 없을 경우그냥 비어있는걸반환
        }
    }

    public void characterSensitivity(float _sensitivity)
    {
        if (unit != null)
        {
            unit.Sensitivity = _sensitivity / 2;
        }
    }
    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isEscInputLocked && SceneManager.GetActiveScene().buildIndex != 0)
        {
            Cursor.lockState = CursorLockMode.None;
            UIManager.instance.PauseKey();
        }
    }

    public void AddKillCount()
    {
        enemyCount--;
        if (enemyCount <= 0)
        {
            if (portalObj != null)
                portalObj.SetActive(true);
        }

    }

    public void StageChange()
    {
        StartCoroutine(loadSceneWithLoading(nextStageNum));
    }

    IEnumerator loadSceneWithLoading(int _stageNum)
    {
        unitStop = true;
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

        RemoveEnemy();
        RemovePoolingRoot();

        loadingBarObj = UIManager.instance.LoadingBar;
        loadingBarObj.SetActive(true);
        loadingBar = loadingBarObj.transform.Find("Loading/LoadingBar").GetComponent<Image>();
        isLoading = true;
        AsyncOperation op = SceneManager.LoadSceneAsync(_stageNum);
        op.allowSceneActivation = false;

        //로딩이 빨라서 3초짜리 연출 추가 , 늘어난다면 progress 로 연결
        while (op.progress < 0.89f)
        {

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

        op.allowSceneActivation = true;
    }

    public void RemoveEnemy()
    {
        if (_spawnSetting != null)
            _spawnSetting.RemoveEnemy();
    }

    public void RemovePoolingRoot()
    {
        for (int i = 0; i < poolingRoot.childCount; i++)
        {
            if (poolingRoot.GetChild(i).childCount != 0)
            {
                PoolingManager.Instance.RemoveAllPoolingObject(poolingRoot.GetChild(i).gameObject);
            }
        }
    }

}
