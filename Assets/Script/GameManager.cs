using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject checkF;
    public GameObject CheckF { get { return checkF; } }
    [SerializeField] private Transform weaponParent;

    public Transform GetWeaponParent { get { return weaponParent; } }

    private Dictionary<string, Transform> poolingParents = new();
    public Dictionary<string, Transform> PoolingParents { get { return poolingParents; } }

    [SerializeField] private Transform poolingRoot;
    public Transform GetPoolinRoot { get { return poolingRoot; } }

    private bool isStageStart = false;
    public bool IsStageStarted { get { return isStageStart; } set { isStageStart = value; } }

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

        //카메라
        if (unit != null && cameraManager != null)
            cameraManager.InitializeCamera(unit, unit.transform);
    }

    void Start()
    {

    }

    public bool stageChange = false;
    public int stageNum = 1;
    // Update is called once per frame
    void Update()
    {
        if (stageChange)
        {
            SceneManager.LoadScene(stageNum);
            AsyncOperation op;
            stageChange = false;
        }
    }

}
