using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wave : MonoBehaviour
{
    [Tooltip("�̹� ���̺꿡 ���� Enemy��")]
    public List<Transform> enemys;
    [Tooltip("�̹� ���̺꿡 ���� Enemy���� ù ���������� �θ� ��ü")]
    public Transform positions;

    void Start()
    {
        // �ڽ� ��ü�� Enemy Component�� ���� ��ü���� enemys�� ����
        for(int i = 0; i < transform.childCount; i++)
        {
            // Child�� Transform ��������
            Transform child = transform.GetChild(i);

            if (child.GetComponent<Character>() != null)
            {
                // List�� Eenemy�� Transform ����
                enemys.Add(child);

                // ù ������ ����
                if (positions != null && positions.childCount > i)
                {
                    Enemy enemy = child.GetComponent<Enemy>();
                    enemy.firstDestination = positions.GetChild(i);
                    enemy.positioning = false;
                    enemy.GetComponent<NavMeshAgent>().SetDestination(enemy.firstDestination.position);
                }

                // ����� enemy�� ��Ȱ��ȭ(���� ���)
                child.gameObject.SetActive(false);
            }
        }
    }
}
