using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward_BulletSpawn_Angle45", menuName = "Scriptables/Reward/Action/BulletSpawn/Angle 45", order = 1)]
public class Reward_BulletSpawn_Angle45 : Reward_BulletSpawn
{
    public Reward_BulletSpawn_Angle45()
    {
        bulletSpawn = new BulletSpawn_Angle_45();
        string_Name = "사선 투사체";
        string_Description = "사선을 향해 투사체 발사";
        leftRewardCount = 2;
    }
}
