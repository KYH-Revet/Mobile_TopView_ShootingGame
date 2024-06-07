using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [Tooltip("이번 웨이브에 속한 Enemy들")]
    public List<Transform> enemys;
    [Tooltip("이번 웨이브에 속한 Enemy들의 첫 목적지들의 부모 객체")]
    public Transform positions;

    void Start()
    {
        //자식 객체중 Enemy Component를 가진 객체들을 enemys에 저장
        for(int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.GetComponent<Character>() != null)
            {
                enemys.Add(child);

                //첫 목적지 배정
                if (positions != null && positions.childCount > i)
                    child.GetComponent<Enemy>().firstDestination = positions.GetChild(i);

                //저장된 enemy는 비활성화(스폰 대기)
                child.gameObject.SetActive(false);
            }
        }
    }
}
