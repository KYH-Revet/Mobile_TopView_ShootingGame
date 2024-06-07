using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward_BulletSpawn_Angle45", menuName = "Scriptables/Reward/Action/BulletSpawn/Angle 45", order = 1)]
public class Reward_BulletSpawn_Angle45 : Reward_BulletSpawn
{
    public Reward_BulletSpawn_Angle45()
    {
        bulletSpawn = new BulletSpawn_Angle_45();
        string_Name = "�缱 ����ü";
        string_Description = "�缱�� ���� ����ü �߻�";
        leftRewardCount = 2;
    }
}
