using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardCard : MonoBehaviour, IObservable<GameManager.GameResult>, IDisposable
{
    // UI
    [Header("UI")]
    public Image img_Thumbnail;
    public Text txt_Type;
    public Text txt_Name;
    public Text txt_Description;

    // Reward
    Reward reward;

    public void SetReward(Reward reward)
    {
        // Reward
        this.reward             = reward;

        // Image
        img_Thumbnail.sprite    = reward.img_Thumbnail;

        // Text
        txt_Type.text           = reward.string_RewardType;
        txt_Name.text           = reward.string_Name;
        txt_Description.text    = reward.string_Description;
    }
    public void OnClicked()
    {
        try
        {
            // Rewarding
            reward.Rewarding();
            GameManager.instance.SelectedReward(reward.string_Name);
            
            // Time
            Time.timeScale = 1f;

            // Send a sign to observers
            NotifyObserver();
        }
        catch (NullReferenceException)
        {
            Debug.LogError("reward or GameManager.instance is NULL");
        }
    }

    //Observer Pattern
    List<IObserver<GameManager.GameResult>> gameResultObservers = new List<IObserver<GameManager.GameResult>>();
    public IDisposable Subscribe(IObserver<GameManager.GameResult> observer)
    {
        gameResultObservers.Add(observer);
        return this;
    }
    public void UnSubscribe(IObserver<GameManager.GameResult> observer)
    {
        gameResultObservers.Remove(observer);
    }
    /// <summary> 보상이 선택됨 </summary>
    public void NotifyObserver()
    {
        foreach (IObserver<GameManager.GameResult> observer in gameResultObservers)
            observer.OnNext(GameManager.GameResult.StageClear);
    }
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}