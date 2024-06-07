using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward_BulletSpawn_Angle90", menuName = "Scriptables/Reward/Action/BulletSpawn/Angle 90", order = 2)]
public class Reward_BulletSpawn_Angle90 : Reward_BulletSpawn
{
    public Reward_BulletSpawn_Angle90()
    {
        bulletSpawn = new BulletSpawn_Angle_90();
        string_Name = "�¿� ����ü";
        string_Description = "�¿츦 ���� ����ü �߻�";
        leftRewardCount = 1;
    }
}
