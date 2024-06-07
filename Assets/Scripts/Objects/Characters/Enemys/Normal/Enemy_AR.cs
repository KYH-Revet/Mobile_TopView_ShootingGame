using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AR : Enemy_Normal
{
    protected override void SetDefaultStrategy()
    {
        // Enemy_Normal
        base.SetDefaultStrategy();

        // Bullet spawn
        AddBulletSpawnStrategy(new BulletSpawn_Front());
    }
}
