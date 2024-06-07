using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward_Stat : Reward_Effect
{
    //Main Function
    public override bool Rewarding()
    {
        if (Player.instance == null)
        {
            Debug.LogError("Reward Damage Type 동작 중 Player.Instance()가 Null입니다.");
            return false;
        }
        return true;
    }
}
