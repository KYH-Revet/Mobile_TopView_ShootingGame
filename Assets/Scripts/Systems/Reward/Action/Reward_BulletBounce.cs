using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward_Bounce", menuName = "Scriptables/Reward/Action/Bounce", order = 0)]
public class Reward_BulletBounce : Reward_Action
{
    public Reward_BulletBounce()
    {
        string_Name = "�ݻ� ����ü";
        string_Description = "���� �ݻ��մϴ�.";
        leftRewardCount = 1;
    }
    public override bool Rewarding()
    {
        try
        {
            Player.instance.bulletOption.bounce = true;
            return true;
        }
        catch (NullReferenceException)
        {
            Debug.LogError("Reward Damage Type ���� �� Player.Instance()�� Null�Դϴ�.");
            return false;
        }
    }
}
