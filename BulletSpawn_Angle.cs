using System;
using System.Collections;
using UnityEngine;

/// <summary>정면을 기준으로 angle도 만큼 벌려서 양쪽으로 발사하는 전략</summary>
public class BulletSpawn_Angle : IBulletSpawn
{
    protected float angle;
    protected virtual void SetAngle() { angle = 0; }
    public bool Spawn(GameObject bulletObj, Transform spawnTr, Bullet.Option bulletOption, int level, int layer)
    {
        SetAngle();
        try
        {
            Debug.Log(angle + "도 사격 Lv." + level);
            float x = 0;
            for (int i = 0; i < level; i++)
            {
                //Spawn
                GameObject[] bullets = new GameObject[2];
                for (int j = 0; j < 2; j++)
                {
                    //Instantiate
                    GameObject obj = MonoBehaviour.Instantiate(bulletObj, spawnTr);
                    obj.transform.parent = spawnTr;

                    //Setting bullet
                    obj.GetComponent<Bullet>().option = bulletOption;
                    obj.gameObject.layer = layer;

                    //Pivot rotate
                    spawnTr.transform.Rotate(0, j % 2 == 0 ? angle : -angle, 0);

                    //Set Local Position & Direction
                    Vector3 localPos = obj.transform.localPosition;
                    localPos.x = level % 2 != 0 ? x + i * Mathf.Pow(-1, i) : x + (i * 2) - 1f;

                    //Translate
                    obj.transform.localPosition = localPos;

                    //Independent
                    obj.transform.parent = null;
                    obj.transform.localScale = bulletObj.transform.localScale;

                    bullets[j] = obj;

                    //Pivot restore
                    spawnTr.transform.Rotate(0, j % 2 == 0 ? -angle : angle, 0);
                }

                //Record value for next calculation
                x = bullets[0].transform.localPosition.x;
            }
            return true;
        }
        catch (NullReferenceException)
        {
            return false;
        }
    }
}