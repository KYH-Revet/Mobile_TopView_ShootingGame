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
            Debug.LogError("Reward Damage Type ���� �� Player.Instance()�� Null�Դϴ�.");
            return false;
        }
        return true;
    }
}
