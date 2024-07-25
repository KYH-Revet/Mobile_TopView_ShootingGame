using System;
using System.Collections;
using UnityEngine;

/// <summary>전방위로 발사하는 전략</summary>
public class BulletSpawn_360 : IBulletSpawn
{
    private int bulletAmount = 6;
    public bool Spawn(GameObject bulletObj, Transform spawnTr, Bullet.Option bulletOption, int level)
    {
        try
        {
            // Spawn
            for (int i = 0; i < bulletAmount; i++)
            {
                // Instantiate
                GameObject bullet = MonoBehaviour.Instantiate(bulletObj, spawnTr);

                // Setting bullet
                bullet.GetComponent<Bullet>().option = bulletOption;

                // Rotate
                bullet.transform.Rotate(0f, (60f * i), 0f);

                // Independent
                bullet.transform.parent = null;
            }
            return true;
        }
        catch (NullReferenceException)
        {
            if (bulletObj == null)
                Debug.LogError("BulletSpawn_360Shooter.cs : bulletObj is null");
            if (spawnTr == null)
                Debug.LogError("BulletSpawn_360Shooter.cs : spawnTr is null");
            return false;
        }
    }
}