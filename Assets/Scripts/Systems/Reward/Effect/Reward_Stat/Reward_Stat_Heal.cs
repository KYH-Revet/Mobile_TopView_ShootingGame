using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward_Stat_Heal", menuName = "Scriptables/Reward/Effact/Stat/Heal", order = 0)]
public class Reward_Stat_Heal : Reward_Stat
{
    // Create Functions
    public Reward_Stat_Heal()
    {
        string_Name = "회복";
        string_Description = "생명력을 회복합니다. ";    // 최대 체력의 25%
        leftRewardCount = 1000;    // 무한
    }
    
    public override bool Rewarding()
    {
        if (base.Rewarding())
        {
            Player player = Player.instance;            // Get Player
            player.HPControll(player.stat.maxHp / 4f);  // Heal 25%
            return true;
        }
        return false;
    }

}
