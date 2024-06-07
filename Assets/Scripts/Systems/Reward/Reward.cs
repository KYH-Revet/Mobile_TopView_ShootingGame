using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Reward : ScriptableObject
{
    // Reward type : �Ѿ� �׼� = Action, ��Ÿ ���� = Effect
    public enum RewardType
    {
        EFFECT,
        ACTION
    }
    [Header("Type")]
    public RewardType rewardType;

    // UI
    [Header("UI")]
    public Sprite img_Thumbnail;        // �����(������) �̹���
    public string string_RewardType;    // ���� Ÿ��
    public string string_Name;          // �����
    [TextArea]
    public string string_Description;   // ����
    public int leftRewardCount;         // ���� ���� ���� ���� ��

    // Function
    public abstract bool Rewarding();    
}
