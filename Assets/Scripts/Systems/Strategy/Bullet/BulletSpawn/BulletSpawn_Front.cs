using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Àü¹æÀ¸·Î ¹ß»çÇÏ´Â Àü·«</summary>
public class BulletSpawn_Front : IBulletSpawn
{
    public bool Spawn(GameObject bulletObj, Transform spawnTr, Bullet.Option bulletOption, int level)
    {
        try
        {
            float x = 0;
            for (int i = 0; i < level; i++)
            {
                // Spawn
                GameObject bullet = MonoBehaviour.Instantiate(bulletObj, spawnTr);

                // Get parent
                bullet.transform.parent = spawnTr;

                // Setting bullet
                bullet.GetComponent<Bullet>().option = bulletOption;

                // Set Local Position & Direction
                Vector3 localPos = bullet.transform.localPosition;
                // È¦¼ö
                if (level % 2 != 0)
                {
                    // (Áß¾Ó) (-> ÁÂ -> ¿ì) (-> ÁÂ -> ¿ì)
                    localPos.x = x + (i * Mathf.Pow(-1, i)) * 1f;

                    // ²ªÀÓ ¸ð¾çÀ» À§ÇØ Ã¹ ¹ßÀ» Á¦¿ÜÇÑ ÅºÀº µÚ·Î ÀÌµ¿
                    if (i != 0)
                        localPos.z = -0.5f;
                }
                // Â¦¼ö
                else
                {
                    // (ÁÂ -> ¿ì) (-> ÁÂ -> ¿ì) (-> ÁÂ -> ¿ì)
                    localPos.x = x + (i * 1.5f) - 0.5f;
                }

                // Translate
                bullet.transform.localPosition = localPos;

                // Record value for next calculation
                x = bullet.transform.localPosition.x;

                // Independent
                bullet.transform.parent = null;
                bullet.transform.localScale = bulletObj.transform.localScale;
            }
            return true;
        }
        catch (NullReferenceException)
        {
            if(bulletObj == null)
                Debug.LogError("BulletSpawn_Front.cs : bulletObj is null");
            if(spawnTr == null)
                Debug.LogError("BulletSpawn_Front.cs : spawnTr is null");

            return false;
        }
    }
}
