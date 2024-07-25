using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임 매니저 관리 목록
/// 1. 게임 진행 상태 (Game Result)
/// 2. 스테이지 클리어 보상 (Reward)
/// 3. 전체 플레이 타임 기록 (playTime)
/// </summary>

public class GameManager : MonoBehaviour, IObservable<GameManager.GameResult>, IDisposable
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
    public enum GameResult
    {
        Pause,
        Processing,
        StageClear,
        Win,
        Lose
    }
    public GameResult gameResult { get; private set; }
    [Header("UI")]
    public GameResultScreen gameResultScreen;

    // 전체 플레이 타임
    [HideInInspector]
    public float playTime = 0f;

    // 스테이지 클리어 보상 리스트
    [SerializeField][Space]
    [Tooltip("UI Reward Container")]
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

        // GameResult Reset
        gameResult = GameResult.Processing;

        // Don't Destroy Manager Object
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        // Record play time
        if (gameResult == GameResult.Processing)
            playTime += Time.deltaTime;
    }


    // Game Result Functions

    /// <summary> WaveManager >> StageClear, Boss >> Win, Player >> Lose</summary>
    /// <param name="gameResult">Processing, StageClear, Win, Lose</param>
    public void SetGameResult(GameResult gameResult)
    {
        switch (gameResult)
        {
            case GameResult.Pause:
                this.gameResult = gameResult;
                Time.timeScale = 0f;
                break;
            case GameResult.Processing:
            case GameResult.StageClear:
                if (this.gameResult < gameResult)
                {
                    this.gameResult = gameResult;
                    Time.timeScale = 1f;
                }
                break;
            case GameResult.Win:
            case GameResult.Lose:
                this.gameResult = gameResult;
                gameResultScreen.gameObject.SetActive(true);
                break;

        }
        NotifyGameResult();
    }


    // Load Scene

    /// <summary>Stage 전환 함수</summary>
    /// <param name="sceneName">전환할 Scene 이름</param>
    public void NextStage(string sceneName)
    {
        // Rest Observer list
        observers_GameResult.Clear();

        // ReSubscribe(Only Player), Player subscribes to GameManager
        Player.instance.Subscribe();

        // Game Result => Processing
        SetGameResult(GameResult.Processing);

        // Load Scene
        SceneManager.LoadScene(sceneName);
    }


    // Reward Functions

    /// <summary> 3개의 랜덤 보상을 Reward Container에 보내주기 위한 함수 </summary>
    void CurrentRewards()
    {
        List<Reward> cur_Reward = new List<Reward>();
        for(int i = 0; i < 3; i++)
        {
            //Random index
            int idx = UnityEngine.Random.Range(0, rewards.Count);
            //Keep searching until you find a nonduplicate reward.
            while (cur_Reward.Contains(rewards[idx]))
                idx = UnityEngine.Random.Range(0, rewards.Count);
            //Add reward
            cur_Reward.Add(rewards[idx]);
        }
        //Set reward in container
        rewardContainer.SetData(cur_Reward);
    }
    /// <summary> 선택된 보상 카운트다운 </summary>
    /// <param name="target"> 보상의 이름 </param>
    public void SelectedReward(string target)
    {
        for(int i = 0; i < rewards.Count; i++)
        {
            if (target == rewards[i].string_Name)
            {
                if (--rewards[i].leftRewardCount <= 0)
                    rewards.RemoveAt(i);
                break;
            }
        }
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
    List<IObserver<GameResult>> observers_GameResult = new List<IObserver<GameResult>>();
    public IDisposable Subscribe(IObserver<GameResult> observer)
    {
        if(!observers_GameResult.Contains(observer))
            observers_GameResult.Add(observer);
        return this;
    }
    public void UnSubscribe(IObserver<GameResult> observer)
    {
        if(observers_GameResult.Contains(observer))
            observers_GameResult.Remove(observer);
    }
    public void NotifyGameResult()
    {
        //Reward list update
        if (gameResult == GameResult.StageClear && rewardContainer != null)
            CurrentRewards();

        // Send game result data
        for (int i = observers_GameResult.Count - 1; i >= 0; i--)
            observers_GameResult[i].OnNext(gameResult);
    }
    void IDisposable.Dispose()
    {
        throw new NotImplementedException();
    }
}
