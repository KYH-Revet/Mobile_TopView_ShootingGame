using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward_BulletSpawn_Front", menuName = "Scriptables/Reward/Action/BulletSpawn/Front", order = 0)]
public class Reward_BulletSpawn_Front : Reward_BulletSpawn
{
    public Reward_BulletSpawn_Front()
    {
        bulletSpawn = new BulletSpawn_Front();
        string_Name = "전방 투사체";
        string_Description = "전방을 향해 투사체 발사";
        leftRewardCount = 2;
    }
}
