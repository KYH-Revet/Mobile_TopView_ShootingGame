using System;
using System.Collections;
using UnityEngine;

/// <summary>정면을 기준으로 angle도 만큼 벌려서 양쪽으로 발사하는 전략</summary>
public class BulletSpawn_Angle : IBulletSpawn
{
    protected float angle;
    public bool Spawn(GameObject bulletObj, Transform spawnTr, Bullet.Option bulletOption, int level)
    {
        try
        {
            float x = 0;
            for (int i = 0; i < level; i++)
            {
                //Spawn
                for (int j = 0; j < (angle > 0 ? 2 : 1); j++)
                {
                    //Instantiate
                    GameObject bullet = MonoBehaviour.Instantiate(bulletObj, spawnTr);
                    bullet.transform.parent = spawnTr;

                    //Setting bullet
                    bullet.GetComponent<Bullet>().option = bulletOption;

                    //Pivot rotate
                    spawnTr.transform.Rotate(0, j % 2 == 0 ? angle : -angle, 0);

                    //Set Local Position & Direction
                    Vector3 localPos = bullet.transform.localPosition;
                    localPos.x = level % 2 != 0 ? x + i * Mathf.Pow(-1, i) : x + (i * 2) - 1f;

                    //Translate
                    bullet.transform.localPosition = localPos;

                    //Record value for next calculation
                    x = j == 1 ? bullet.transform.localPosition.x : x;

                    //Independent
                    bullet.transform.parent = null;
                    bullet.transform.localScale = bulletObj.transform.localScale;

                    //Pivot restore
                    spawnTr.transform.Rotate(0, j % 2 == 0 ? -angle : angle, 0);
                }
            }
            return true;
        }
        catch (NullReferenceException)
        {
            return false;
        }
    }
}