using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward_BulletSpawn : Reward_Action
{
    //Variables    
    [Tooltip("�Ѿ� �߻� ���� ����")]
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
            Debug.LogError("Reward Damage Type ���� �� Player.Instance()�� Null�Դϴ�.");
            return false;
        }
    }
}
