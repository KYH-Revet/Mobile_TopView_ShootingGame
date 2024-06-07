using System.Collections;
using UnityEngine;

public class DamageType_Dot : MonoBehaviour, IDamaged
{
    public float damage;
    int count, dotCount = 4;

    public void Damaged(Character target, float damage)
    {
        Debug.Log("��Ʈ ������");
        if(target.GetComponent<DamageType_Dot>() == null)
            target.gameObject.AddComponent<DamageType_Dot>();

        target.GetComponent<DamageType_Dot>().count = 0;
        target.GetComponent<DamageType_Dot>().damage = (target.stat.maxHp / 100f) * 3f; //�ִ� ü���� 3%��ŭ ����
    }
    void Start()
    {
        StartCoroutine(DotDamaged());
    }
    IEnumerator DotDamaged()
    {
        for (; count < dotCount; count++)
        {
            GetComponent<Character>().HPControll(-(damage / dotCount));
            yield return new WaitForSeconds(0.25f);
        }
        Destroy(this);
    }
}