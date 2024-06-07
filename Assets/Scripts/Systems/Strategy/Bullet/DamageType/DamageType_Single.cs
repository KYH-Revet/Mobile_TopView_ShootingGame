using UnityEngine;

public class DamageType_Single : IDamaged
{
    public void Damaged(Character target, float damage)
    {
        target.HPControll(-damage);
    }
}
