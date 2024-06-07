using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward_BulletSpawn_Front", menuName = "Scriptables/Reward/Action/BulletSpawn/Front", order = 0)]
public class Reward_BulletSpawn_Front : Reward_BulletSpawn
{
    public Reward_BulletSpawn_Front()
    {
        bulletSpawn = new BulletSpawn_Front();
        string_Name = "���� ����ü";
        string_Description = "������ ���� ����ü �߻�";
        leftRewardCount = 2;
    }
}
