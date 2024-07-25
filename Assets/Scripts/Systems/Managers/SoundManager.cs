using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BGM     : 로비, 스테이지, 보스전, 승리, 패배
/// 효과음  : Player의 발포음
/// </summary>

public class SoundManager : MonoBehaviour, IObserver<GameManager.GameResult>
{
    // Singleton Pattern
    public static SoundManager instance { get; private set; }
    void InstanceSuccession()
    {
        // Audio Source
        GetComponent<AudioSource>().volume = instance.audioSource.volume;   //Volume

        // Volume
        bgmVolume = PlayerPrefs.GetFloat("Volume_BGM");
        effectVolume = PlayerPrefs.GetFloat("Volume_Effect");

        // BGM'ss
        if(mainBGM == null)
            mainBGM = instance.mainBGM; //Main
        if(winBGM == null)
            winBGM = instance.winBGM;   //Win
        if (loseBGM == null)
            loseBGM = instance.loseBGM; //Lose

        // Destroy instance
        Destroy(instance.gameObject);
    }

    // AudioSource
    private AudioSource audioSource;

    // Audio Clips (BGM)
    [Header("Main BGM")]
    public AudioClip mainBGM;

    [Header("Game Result BGM")]
    public AudioClip winBGM;
    public AudioClip loseBGM;

    // Audio Volume
    [Header("Audio Volume")]
    [Range(0f, 1f)]
    public float bgmVolume;
    [Range(0f, 1f)]
    public float effectVolume;

    // Unity Functions
    void Awake()
    {
        // Singleton Update BGM Data
        if (instance != null)
        {
            if(mainBGM == instance.mainBGM)
                Destroy(gameObject);
            else
                InstanceSuccession();
        }
        instance = this;

        // Play audio
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = mainBGM;
        audioSource.Play();

        // Save PlayerPrefs data
        if(!PlayerPrefs.HasKey("Volume_BGM"))
            PlayerPrefs.SetFloat("Volume_BGM", bgmVolume);
        else
            bgmVolume = PlayerPrefs.GetFloat("Volume_BGM");
        if (!PlayerPrefs.HasKey("Volume_Effect"))
            PlayerPrefs.SetFloat("Volume_Effect", effectVolume);
        else
            effectVolume = PlayerPrefs.GetFloat("Volume_Effect");

        // Dont Destroy this
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        // Load & Apply the volume value
        LoadVolumeValue();

        // Observer Pattern
        Subscribe();
    }

    // Volume Functions
    public void SetBGMVolume(float value)
    {
        // Save the value
        bgmVolume = value;

        // Apply volume
        audioSource.volume = bgmVolume;
    }
    public void SetEffectVolume(float value)
    {
        // Save the value
        effectVolume = value;

        // Apply volume
        if (Player.instance != null)
            Player.instance.GetComponent<AudioSource>().volume = effectVolume;
    }
    private void LoadVolumeValue()
    {
        SetBGMVolume(PlayerPrefs.GetFloat("Volume_BGM"));       // BGM
        SetEffectVolume(PlayerPrefs.GetFloat("Volume_Effect")); // Effect
    }


    // Observer Pattern Observe : GameManager Game result
    private void Subscribe()
    {
        if(GameManager.instance != null)
            GameManager.instance.Subscribe(this);   // GameManager
    }
    private void UnSubscribe()
    {
        if (GameManager.instance != null)
            GameManager.instance.UnSubscribe(this);   // GameManager
    }
    public void OnCompleted()
    {
        throw new NotImplementedException();
    }
    public void OnError(Exception error)
    {
        Debug.LogError(error.ToString());
    }
    public void OnNext(GameManager.GameResult value)
    {
        try
        {
            // 기존의 BGM
            AudioClip nextClip = mainBGM;

            // 게임 결과에 따른 BGM 변경
            switch (value)
            {
                case GameManager.GameResult.Win:    // 승리
                    nextClip = winBGM;
                    break;
                case GameManager.GameResult.Lose:   // 패배
                    nextClip = loseBGM;
                    break;
                default:                            // 기타(변경없음, 함수 종료)
                    return;
            }

            // Clip Play
            audioSource.clip = nextClip;
            audioSource.Play();

            // Observer Pattern
            UnSubscribe();
        }
        catch (NullReferenceException)
        {
            Debug.LogError(GameManager.instance.gameResult + "의 BGM이 null입니다.");
        }
    }
}
