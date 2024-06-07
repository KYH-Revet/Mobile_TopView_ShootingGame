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
        string_Name = "ȭ�� ȿ��";
        string_Description = "��󿡰� �߰� ���ظ� �����ϴ�";
        leftRewardCount = 1;
    }
}
