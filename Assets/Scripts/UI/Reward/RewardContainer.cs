using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Observ to GameManger
/// Observ by RewardCard
/// </summary>
public class RewardContainer : MonoBehaviour, IObserver<GameManager.GameState>
{
    // Singleton Pattern
    public static RewardContainer instance { get; private set; }

    [Header("Reward Cards")]
    public List<RewardCard> rewardCards;

    // Unity Functions
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        // Regist this to GameManager
        GameManager.instance.rewardContainer = instance;

        // Observer Pattern
        Subsribe();

        // Hide
        gameObject.SetActive(false);
    }

    // Set data in reward cards
    public void SetData(List<Reward> rewards)
    {
        for (int i = 0; i < rewardCards.Count; i++)
            rewardCards[i].SetReward(rewards[i]);
    }

    // Observer Pattern
    private void Subsribe()
    {
        // GameManager
        GameManager.instance.Subscribe(this);
    }
    private void UnSubscribe()
    {
        // GameManager
        GameManager.instance.UnSubscribe(this);
    }
    public void OnCompleted()
    {
        throw new NotImplementedException();
    }
    public void OnError(Exception error)
    {
        Debug.LogError(error.ToString());
    }
    public void OnNext(GameManager.GameState value)
    {
        switch(value)
        {
            // No more need
            case GameManager.GameState.StageClear:
            case GameManager.GameState.Win:
            case GameManager.GameState.Lose:
                // Observer Pattern
                UnSubscribe();

                // Destroy
                Destroy(gameObject);
                break;

            // UI on
            case GameManager.GameState.RewardSelect:
                gameObject.SetActive(true);
                break;
        }
    }
}
