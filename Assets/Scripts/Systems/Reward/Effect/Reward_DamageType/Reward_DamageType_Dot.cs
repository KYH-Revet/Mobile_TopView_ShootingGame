using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward_Dot_Damage", menuName = "Scriptables/Reward/Effact/Damage Type/Dot", order = 1)]
public class Reward_DamageType_Dot : Reward_DamageType
{
    public Reward_DamageType_Dot()
    {
        damageType = new DamageType_Dot();
        string_Name = "화상 효과";
        string_Description = "대상에게 추가 피해를 입힙니다";
        leftRewardCount = 1;
    }
}
