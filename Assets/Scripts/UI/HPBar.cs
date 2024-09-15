using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class HPBar : MonoBehaviour
{
    [SerializeField]
    protected Character owner;
    [SerializeField]
    protected RectTransform cur_hp;

    private void Awake()
    {
        //Find parent who have a Character Component
        Transform cur = transform;
        while (cur.parent != null && cur.GetComponent<Character>() == null)
            cur = cur.parent;
        owner = cur.GetComponent<Character>();
    }
    private void Update()
    {
        transform.rotation = Quaternion.Euler(45f, 0, 0);
    }

    //Calling from other script(Character.GetDamage()
    public virtual void HPSynchronization()
    {
        if (owner == null)
        {
            Debug.LogError("HPBar.cs void HPSynchronization() : owner is null");
            return;
        }

        float rate = 1f - (owner.stat.hp / owner.stat.maxHp);
        cur_hp.localPosition = new Vector2(rate, 0);
    }
}
