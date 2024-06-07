using UnityEngine;

public interface IBulletSpawn
{
    public bool Spawn(GameObject bulletObj, Transform spawnTr, Bullet.Option bulletOption, int level);
}
