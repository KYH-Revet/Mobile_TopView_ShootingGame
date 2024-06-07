using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�������� �߻��ϴ� ����</summary>
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
                // Ȧ��
                if (level % 2 != 0)
                {
                    // (�߾�) (-> �� -> ��) (-> �� -> ��)
                    localPos.x = x + (i * Mathf.Pow(-1, i)) * 1f;

                    // ���� ����� ���� ù ���� ������ ź�� �ڷ� �̵�
                    if (i != 0)
                        localPos.z = -0.5f;
                }
                // ¦��
                else
                {
                    // (�� -> ��) (-> �� -> ��) (-> �� -> ��)
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
