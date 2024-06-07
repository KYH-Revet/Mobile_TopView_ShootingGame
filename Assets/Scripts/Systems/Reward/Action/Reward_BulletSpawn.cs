using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward_BulletSpawn : Reward_Action
{
    //Variables    
    [Tooltip("총알 발사 전략 종류")]
    protected IBulletSpawn bulletSpawn;

    //Main Function
    public override bool Rewarding()
    {
        try
        {
            Player.instance.AddBulletSpawnStrategy(bulletSpawn);
            return true;
        }
        catch (NullReferenceException)
        {
            Debug.LogError("Reward Damage Type 동작 중 Player.Instance()가 Null입니다.");
            return false;
        }
    }
}
