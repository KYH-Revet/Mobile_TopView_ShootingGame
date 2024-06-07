using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BGM     : �κ�, ����, ����, �¸�, �й�
/// ȿ����  : Player�� ������
/// </summary>

public class SoundManager : MonoBehaviour, IObserver<GameManager.GameResult>
{
    // Singleton Pattern
    public static SoundManager instance { get; private set; }
    void InstanceSuccession()
    {
        // Audio Source
        GetComponent<AudioSource>().volume = instance.audioSource.volume;   //Volume

        // BGM's
        mainBGM = instance.mainBGM; //Main
        winBGM = instance.winBGM;   //Win
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

        // Dont Destroy this
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        //Observer Pattern
        Subscribe();
    }

    // Observer Pattern
    private void Subscribe()
    {
        GameManager.instance.Subscribe(this);   // GameManager
    }
    private void UnSubscribe()
    {
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
            // ������ BGM
            AudioClip nextClip = mainBGM;

            // ���� ����� ���� BGM ����
            switch (value)
            {
                case GameManager.GameResult.Win:    // �¸�
                    nextClip = winBGM;
                    break;
                case GameManager.GameResult.Lose:   // �й�
                    nextClip = loseBGM;
                    break;
                default:                            // ��Ÿ(�������, �Լ� ����)
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
            Debug.LogError(GameManager.instance.gameResult + "�� BGM�� null�Դϴ�.");
        }
    }
}
