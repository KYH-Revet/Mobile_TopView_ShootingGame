using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SG : Enemy_Normal
{
    protected override void SetDefaultStrategy()
    {
        // Enemy_Normal
        base.SetDefaultStrategy();

        //Bullet spawn
        AddBulletSpawnStrategy(new BulletSpawn_Front());
        AddBulletSpawnStrategy(new BulletSpawn_Angle_45());
    }
}
