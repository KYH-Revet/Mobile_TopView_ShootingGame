using System.Collections;
using UnityEngine;

public class DamageType_Dot : MonoBehaviour, IDamaged
{
    public float damage;
    int count = 0, dotCount = 4;

    public void Damaged(Character target, float damage)
    {
        if(target.GetComponent<DamageType_Dot>() == null)
            target.gameObject.AddComponent<DamageType_Dot>();

        target.GetComponent<DamageType_Dot>().count = 0;
        target.GetComponent<DamageType_Dot>().damage = (target.stat.maxHp / 100f) * 3f; //최대 체력의 3%만큼 피해
    }
    void Start()
    {
        StartCoroutine(DotDamaged());
    }
    IEnumerator DotDamaged()
    {
        for (; count < dotCount; count++)
        {
            GetComponent<Character>().HPControll(-damage);
            yield return new WaitForSeconds(2f/(dotCount-1));
        }
        Destroy(this);
    }
}