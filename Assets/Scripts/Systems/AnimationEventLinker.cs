using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventLinker : MonoBehaviour
{
    Character link;

    void Awake()
    {
        link = transform.parent.GetComponent<Character>();
    }

    // For Characters
    public void BulletSpawn()
    {
        link.BulletSpawn();
    }
    public void Dead()
    {
        Debug.Log(link.gameObject.name + "ÀÇ Dead");
        link.Dead();
    }

    // For Enemy Boss
    public void AttackEnd()
    {
        //Get Boss Component
        Enemy boss = link.GetComponent<Enemy>();
        if (boss != null)
            boss.AttackEnd();
    }
}
