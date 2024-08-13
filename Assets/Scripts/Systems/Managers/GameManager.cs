using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ���� �Ŵ��� ���� ���
/// 1. ���� ���� ���� (Game Result)
/// 2. �������� Ŭ���� ���� (Reward)
/// 3. ��ü �÷��� Ÿ�� ��� (playTime)
/// </summary>

public class GameManager : MonoBehaviour, IObservable<GameManager.GameState>, IDisposable
{
    // Singleton
    public static GameManager instance { get; private set; }
    private void InstanceSuccession()
    {
        rewards = instance.rewards;     //Reward list
        playTime = instance.playTime;   //Play time

        Destroy(instance.gameObject);
    }

    // Game State
    public enum GameState
    {
        Pause,          // �Ͻ�����
        Processing,     // ���� ������
        RewardSelect,   // ���� ������
        StageClear,     // �������� Ŭ����
        Win,            // �¸�
        Lose            // �й�
    }
    public GameState gameState { get; private set; }

    [Header("UI")]
    public GameResultScreen gameResultScreen;

    // ��ü �÷��� Ÿ��
    [HideInInspector]
    public float playTime = 0f;

    // �������� Ŭ���� ���� ����Ʈ
    [SerializeField][Space]
    [Tooltip("Reward(UI) Container")]
    public RewardContainer rewardContainer;
    [SerializeField]
    [Tooltip("Reward List")]
    public List<Reward> rewards;


    // Unity Functions

    void Awake()
    {
        // Singleton
        if (instance != null)
            InstanceSuccession();
        instance = this;

        // GameState Reset
        gameState = GameState.Processing;

        // Don't Destroy Manager Object
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        // Record play time
        if (gameState == GameState.Processing)
            playTime += Time.deltaTime;
    }


    // Game State Functions

    /// <summary>
    /// ���� ���� ���¸� �����ϴ� �Լ�
    /// </summary>
    public void SetGameState(GameState gameState)
    {
        // Change the game result
        switch (gameState)
        {
            case GameState.Pause:
                this.gameState = gameState;
                break;
            case GameState.Win:
            case GameState.Lose:
                this.gameState = gameState;

                // Result UI On
                gameResultScreen.gameObject.SetActive(true);    // ������ �װ��� NULL ���� ��
                break;
            default:
                if (this.gameState < gameState)
                    this.gameState = gameState;
                break;
        }

        // Time Scale
        switch (gameState)
        {
            case GameState.Processing:
            case GameState.StageClear:
                // Time active
                Time.timeScale = 1f;
                break;
            default:
                // Time Stop (Select Reward)
                Time.timeScale = 0f;
                break;
        }

        // Observer Pattern : Game result
        NotifyGameState();
    }


    // Load Scene

    /// <summary>Stage ��ȯ �Լ�</summary>
    /// <param name="sceneName">��ȯ�� Scene �̸�</param>
    public void NextStage(string sceneName)
    {
        // Rest Observer list
        observers_GameState.Clear();

        // ReSubscribe(Only Player), Player subscribes to GameManager
        Player.instance.Subscribe();

        // Game Result => Processing
        gameState = GameState.Processing;   // SetGameState()���� Processing < StageClear�� ������ �ȵǴ� ���� �ذ�
        SetGameState(GameState.Processing);

        // Load Scene
        SceneManager.LoadScene(sceneName);
    }


    // Reward Functions

    /// <summary> 3���� ���� ������ Reward Container�� �����ֱ� ���� �Լ� </summary>
    void CurrentRewards()
    {
        List<Reward> cur_Reward = new List<Reward>();
        for(int i = 0; i < 3; i++)
        {
            // Random index
            int idx = UnityEngine.Random.Range(0, rewards.Count);
            // Keep searching until you find a nonduplicate reward.
            while (cur_Reward.Contains(rewards[idx]))
                idx = UnityEngine.Random.Range(0, rewards.Count);
            // Add reward
            cur_Reward.Add(rewards[idx]);
        }
        // Set reward in container
        rewardContainer.SetData(cur_Reward);
    }
    /// <summary> ���õ� ���� ī��Ʈ�ٿ� </summary>
    /// <param name="target"> ������ �̸� </param>
    public void SelectedReward(string target)
    {
        // Find selected reward in list for decrease counter
        for(int i = 0; i < rewards.Count; i++)
        {
            if (target == rewards[i].string_Name)
            {
                if (--rewards[i].leftRewardCount <= 0)
                    rewards.RemoveAt(i);
                break;
            }
        }

        // Game State Change -> StageClear
        SetGameState(GameState.StageClear);
    }


    // System Functions

    /// <summary> Destroy an object that you set up that don't destroy </summary>
    public void DestroyDontDestroyObject()
    {
        Destroy(Player.instance.gameObject);        // Player
        Destroy(SoundManager.instance.gameObject);  // SoundManager
        Destroy(gameObject);                        // GameManager
    }


    // Observer Pattern Subject : Game Result
    List<IObserver<GameState>> observers_GameState = new List<IObserver<GameState>>();
    public IDisposable Subscribe(IObserver<GameState> observer)
    {
        if(!observers_GameState.Contains(observer))
            observers_GameState.Add(observer);
        return this;
    }
    public void UnSubscribe(IObserver<GameState> observer)
    {
        if(observers_GameState.Contains(observer))
            observers_GameState.Remove(observer);
    }
    public void NotifyGameState()
    {
        Debug.Log("GameManager.NotifyGameState(" + gameState + ")");

        //Reward list update
        if (gameState == GameState.RewardSelect && rewardContainer != null)
            CurrentRewards();

        // Send game result data
        for (int i = observers_GameState.Count - 1; i >= 0; i--)
            observers_GameState[i].OnNext(gameState);
    }
    void IDisposable.Dispose()
    {
        throw new NotImplementedException();
    }
}
