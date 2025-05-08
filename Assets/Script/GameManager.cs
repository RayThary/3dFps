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

    [SerializeField]
    private Transform tempParent;
    public Transform TempParent { get { return tempParent; } }

    private bool isFirstPerson;
    public bool FirstPersonCheck { get { return isFirstPerson; } set { isFirstPerson = value; } }

    //�ϴ� üũf�κи� ���ϳ��߿� ���̾���쿡 ĵ�����εΰ� �����ڽ����� �����κ���ã���ִ°������Ű���
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
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //����θ������ִ°� 
    }
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }

}
