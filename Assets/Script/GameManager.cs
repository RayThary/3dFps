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

    [SerializeField]
    private Transform tempParent;
    public Transform TempParent { get { return tempParent; } }

    //일단 체크f부분만 리턴나중에 많이쓸경우에 캔버스로두고 따로자식으로 개개인별로찾아주는게좋을거같음
    [SerializeField] private GameObject checkF;
    public GameObject CheckF { get { return checkF; } }
    [SerializeField] private Transform weaponParent;

    public Transform GetWeaponParent;


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
        unit = FindObjectOfType<Unit>();
        cameraManager = GetComponentInChildren<CameraManager>();
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //무기부모정해주는곳 
    }
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }

}
