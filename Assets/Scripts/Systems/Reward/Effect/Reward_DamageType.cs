using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward_DamageType : Reward_Effect
{
    // Variables    
    [Tooltip("공격 강화 종류")]
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
            Debug.LogError("Reward Damage Type 동작 중 Player.Instance()가 Null입니다.");
            return false;
        }   
    }
}
