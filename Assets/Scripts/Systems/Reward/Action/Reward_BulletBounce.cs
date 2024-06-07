using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward_Bounce", menuName = "Scriptables/Reward/Action/Bounce", order = 0)]
public class Reward_BulletBounce : Reward_Action
{
    public Reward_BulletBounce()
    {
        string_Name = "반사 투사체";
        string_Description = "벽에 반사합니다.";
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
            Debug.LogError("Reward Damage Type 동작 중 Player.Instance()가 Null입니다.");
            return false;
        }
    }
}
