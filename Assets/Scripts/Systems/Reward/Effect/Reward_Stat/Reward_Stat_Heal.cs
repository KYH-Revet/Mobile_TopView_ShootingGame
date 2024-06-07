using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward_Stat_Heal", menuName = "Scriptables/Reward/Effact/Stat/Heal", order = 0)]
public class Reward_Stat_Heal : Reward_Stat
{
    // Create Functions
    public Reward_Stat_Heal()
    {
        string_Name = "ȸ��";
        string_Description = "������� ȸ���մϴ�. ";    // �ִ� ü���� 25%
        leftRewardCount = 1000;    // ����
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
