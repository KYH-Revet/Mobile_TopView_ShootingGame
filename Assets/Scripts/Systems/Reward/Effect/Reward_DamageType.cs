using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward_DamageType : Reward_Effect
{
    // Variables    
    [Tooltip("���� ��ȭ ����")]
    protected IDamaged damageType;
    
    // Main Function
    public override bool Rewarding()
    {
        try
        {
            Player.instance.AddBulletDamageType(damageType);
            return true;
        }
        catch(NullReferenceException)
        {
            Debug.LogError("Reward Damage Type ���� �� Player.Instance()�� Null�Դϴ�.");
            return false;
        }   
    }
}
