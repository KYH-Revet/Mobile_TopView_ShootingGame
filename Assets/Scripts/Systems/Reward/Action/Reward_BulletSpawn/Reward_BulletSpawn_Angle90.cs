using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward_BulletSpawn_Angle90", menuName = "Scriptables/Reward/Action/BulletSpawn/Angle 90", order = 2)]
public class Reward_BulletSpawn_Angle90 : Reward_BulletSpawn
{
    public Reward_BulletSpawn_Angle90()
    {
        bulletSpawn = new BulletSpawn_Angle_90();
        string_Name = "좌우 투사체";
        string_Description = "좌우를 향해 투사체 발사";
        leftRewardCount = 1;
    }
}
