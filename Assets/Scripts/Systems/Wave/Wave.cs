using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [Tooltip("�̹� ���̺꿡 ���� Enemy��")]
    public List<Transform> enemys;
    [Tooltip("�̹� ���̺꿡 ���� Enemy���� ù ���������� �θ� ��ü")]
    public Transform positions;

    void Start()
    {
        //�ڽ� ��ü�� Enemy Component�� ���� ��ü���� enemys�� ����
        for(int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.GetComponent<Character>() != null)
            {
                enemys.Add(child);

                //ù ������ ����
                if (positions != null && positions.childCount > i)
                    child.GetComponent<Enemy>().firstDestination = positions.GetChild(i);

                //����� enemy�� ��Ȱ��ȭ(���� ���)
                child.gameObject.SetActive(false);
            }
        }
    }
}
