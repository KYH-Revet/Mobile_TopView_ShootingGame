using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward_Penetrate", menuName = "Scriptables/Reward/Action/Penetrate", order = 0)]
public class Reward_BulletPenetrate : Reward_Action
{
    public Reward_BulletPenetrate()
    {
        string_Name = "���� ����ü";
        string_Description = "����� �����մϴ�";
        leftRewardCount = 1;
    }
    public override bool Rewarding()
    {
        try
        {
            Player.instance.bulletOption.penetrate = true;
            return true;
        }
        catch (NullReferenceException)
        {
            Debug.LogError("Reward Damage Type ���� �� Player.Instance()�� Null�Դϴ�.");
            return false;
        }
    }
}
