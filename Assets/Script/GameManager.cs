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

    private CameraManager cameraManager;
    public CameraManager GetCameraManager { get { return cameraManager; } }

    //일단 체크f부분만 리턴나중에 많이쓸경우에 캔버스로두고 따로자식으로 개개인별로찾아주는게좋을거같음
    [SerializeField] private GameObject checkF;
    public GameObject CheckF { get { return checkF; } }
    [SerializeField] private Transform weaponParent;

    public Transform GetWeaponParent { get { return weaponParent; } }

    private Dictionary<string, Transform> poolingParents = new();
    public Dictionary<string, Transform> PoolingParents { get { return poolingParents; } }

    [SerializeField]private Transform poolingRoot;
    public Transform GetPoolinRoot { get { return poolingRoot; } }

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

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

  //초기화될때마다 새로해줄것들
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject worldObject = GameObject.Find("WorldObjects");
        weaponParent = worldObject.transform.Find("weaponParent");
    }


    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }

}
