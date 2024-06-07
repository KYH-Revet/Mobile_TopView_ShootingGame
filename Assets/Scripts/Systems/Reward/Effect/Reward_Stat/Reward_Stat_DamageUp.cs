using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward_Stat_DamageUp", menuName = "Scriptables/Reward/Effact/Stat/Damae Up", order = 1)]
public class Reward_Stat_DamageUp : Reward_Stat
{
    public Reward_Stat_DamageUp()
    {
        string_Name = "���ݷ� ����";
        string_Description = "���ݷ��� �����մϴ�.";
        leftRewardCount = 4;
    }
    public override bool Rewarding()
    {
        if (base.Rewarding())
        {
            Player.instance.Stat_DamageUp(10f);
            return true;
        }
        return false;
    }
}
