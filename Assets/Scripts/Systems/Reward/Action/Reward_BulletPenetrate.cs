using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward_Penetrate", menuName = "Scriptables/Reward/Action/Penetrate", order = 0)]
public class Reward_BulletPenetrate : Reward_Action
{
    public Reward_BulletPenetrate()
    {
        string_Name = "관통 투사체";
        string_Description = "대상을 관통합니다";
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
            Debug.LogError("Reward Damage Type 동작 중 Player.Instance()가 Null입니다.");
            return false;
        }
    }
}
