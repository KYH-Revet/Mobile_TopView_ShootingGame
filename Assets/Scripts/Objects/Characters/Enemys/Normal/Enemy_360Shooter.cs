using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_360Shooter : Enemy_Normal
{
    // Without Aim to target
    public override void Idle()
    {
        // Waiting attack delay
        if (DelayTimer())
            ChangeState(_StateMachine.Attack);
    }

    /// <summary> Single Damage & 360 Spawn</summary>
    protected override void SetDefaultStrategy()
    {
        // Enemy_Normal
        base.SetDefaultStrategy();

        //Bullet spawn
        AddBulletSpawnStrategy(new BulletSpawn_360());
    }
}
