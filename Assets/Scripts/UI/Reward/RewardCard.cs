using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardCard : MonoBehaviour
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

            // Notify Game state changed
            GameManager.instance.SetGameState(GameManager.GameState.StageClear);
        }
        catch (NullReferenceException)
        {
            Debug.LogError("reward or GameManager.instance is NULL");
        }
    }
}