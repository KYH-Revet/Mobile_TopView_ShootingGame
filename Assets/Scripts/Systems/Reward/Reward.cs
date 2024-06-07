using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Reward : ScriptableObject
{
    // Reward type : 총알 액션 = Action, 기타 보상 = Effect
    public enum RewardType
    {
        EFFECT,
        ACTION
    }
    [Header("Type")]
    public RewardType rewardType;

    // UI
    [Header("UI")]
    public Sprite img_Thumbnail;        // 썸네일(아이콘) 이미지
    public string string_RewardType;    // 보상 타입
    public string string_Name;          // 보상명
    [TextArea]
    public string string_Description;   // 설명
    public int leftRewardCount;         // 남은 보상 선택 가능 수

    // Function
    public abstract bool Rewarding();    
}
