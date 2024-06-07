using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MG : Enemy_Normal
{
    public override void Attack()
    {
        // Holding aim when attack end
    }
    protected override void SetDefaultStrategy()
    {
        // Enemy_Normal
        base.SetDefaultStrategy();

        // Bullet spawn
        AddBulletSpawnStrategy(new BulletSpawn_Front());
    }
}
