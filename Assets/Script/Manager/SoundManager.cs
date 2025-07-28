using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public enum Clips
    {
        Atk_Cavalry,
        Atk_Guard,
        Atk_Magic,
        Atk_Range,
        Atk_Sword,
        Buy,
        Defeat,
        Upgrade,
        Victory,
    }

    private AudioSource m_backGroundSource;
    //오디오믹서와 배경음악을 넣어줄것
    [SerializeField] private AudioMixer m_mixer;
    [SerializeField] private AudioClip m_BackGroundClip;
    [SerializeField] private float m_bgmStartDealy = 0.5f;

    private Transform pollingObjParentTrs;//풀링오브젝트의 부모위치

    //오디오클립이 들어간 비어있는 오디오소스
    [SerializeField] private GameObject SFXsource;
    //클립들을 넣어주고 이넘에 클립이랑 이름을똑같이 넣어주어야함
    [SerializeField] private List<AudioClip> clips = new List<AudioClip>();
    [SerializeField] private int poolingCount = 50;


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
        m_backGroundSource = GetComponent<AudioSource>();

        StartCoroutine(bgStart());
        pollingObjParentTrs = transform.GetChild(0);
        initPoolingClip();
    }


    private void initPoolingClip()
    {
        for (int i = 0; poolingCount > i; i++)
        {
            GameObject sfx = Instantiate(SFXsource);
            sfx.transform.SetParent(pollingObjParentTrs);
            sfx.SetActive(false);
        }
    }

    IEnumerator bgStart()
    {
        yield return new WaitForSeconds(m_bgmStartDealy);

        bgSoundPlay(m_BackGroundClip);
    }


    /// <summary>
    /// 내부 이넘을 통해서 사용할 클립을 선택, 볼륨 크기 , 사운드의 시작지점 미지정시 시작지점은 처음 볼륨은1
    /// </summary>
    /// <param name="_clip">사용될 소리</param>
    /// <param name="_volum">소리의 크기</param>
    /// <param name="_SFXTime">소리의 시작지점</param>
    /// <param name="_parent">오브젝트의 부모</param>
    public void SFXCreate(Clips _clip, float _volum, float _SFXTime, Transform _parent)
    {
        AudioClip clip = clips.Find(x => x.name == _clip.ToString());
        StartCoroutine(SFXPlaying(clip, _volum, _SFXTime, _parent));
    }
    public void SFXCreate(Clips _clip, Transform _parent)
    {
        AudioClip clip = clips.Find(x => x.name == _clip.ToString());
        StartCoroutine(SFXPlaying(clip, 1, 0, _parent));
    }

    IEnumerator SFXPlaying(AudioClip clip, float _volum, float _SFXTime, Transform _parent)
    {
        GameObject SFXSource = getPoolingObject(_parent);

        AudioSource m_sfxaudiosource = SFXSource.GetComponent<AudioSource>();

        m_sfxaudiosource.outputAudioMixerGroup = m_mixer.FindMatchingGroups("SFX")[0];
        m_sfxaudiosource.clip = clip;
        m_sfxaudiosource.loop = false;
        m_sfxaudiosource.volume = _volum;
        m_sfxaudiosource.time = _SFXTime;
        m_sfxaudiosource.Play();
        yield return new WaitForSeconds(clip.length);
        removePooling(SFXSource);
    }

    private GameObject getPoolingObject(Transform _parent)
    {
        int parentCount = pollingObjParentTrs.childCount;
        GameObject obj = null;
        if (parentCount > 0)
        {
            obj = pollingObjParentTrs.GetChild(0).gameObject;

        }
        else
        {
            GameObject sfx = Instantiate(SFXsource);
            sfx.transform.SetParent(pollingObjParentTrs);
            sfx.SetActive(false);
            obj = sfx;
        }

        obj.transform.SetParent(_parent);
        obj.SetActive(true);

        return obj;
    }

    private void removePooling(GameObject _obj)
    {

        //본인이 없는경우 채워넣어주고 만약 자식의 개수가 더많다면 삭제해준다.
        if (pollingObjParentTrs.childCount < poolingCount)
        {
            if (_obj == null)
            {
                _obj = Instantiate(SFXsource);
            }

            _obj.transform.SetParent(pollingObjParentTrs);
            _obj.SetActive(false);
            _obj.transform.position = Vector3.zero;
            _obj.GetComponent<AudioSource>().clip = null;

        }
        else
        {
            Destroy(_obj);
        }
    }


    private void bgSoundPlay(AudioClip clip)
    {

        m_backGroundSource.outputAudioMixerGroup = m_mixer.FindMatchingGroups("BackGround")[0];
        m_backGroundSource.clip = clip;
        m_backGroundSource.loop = true;
        m_backGroundSource.time = 0;
        m_backGroundSource.volume = 0.5f;
        m_backGroundSource.Play();
    }

    /// <summary>
    /// 사운드 멈추기 
    /// </summary>
    /// <param name="_value"></param>
    public void bgSoundPause(bool _value)
    {
        if (_value)
        {
            m_backGroundSource.Pause();
        }
        else
        {
            m_backGroundSource.time = 0;
            m_backGroundSource.UnPause();
        }
    }


    /// 슬라이더연결

    public void SetMasterSound(Slider _valeu)
    {
        _valeu.onValueChanged.AddListener(x => m_mixer.SetFloat("Master", Mathf.Log10(x) * 20));
    }

    public void SetSFXSound(Slider _value)
    {
        _value.onValueChanged.AddListener(x => m_mixer.SetFloat("SFX", Mathf.Log10(x) * 20));

    }

    public void SetBGMSound(Slider _value)
    {
        _value.onValueChanged.AddListener(x => m_mixer.SetFloat("BackGround", Mathf.Log10(x) * 20));
    }
}
