using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    Transform target;

    [Header("Camera Position")]
    public float height = 10f;
    public float offset = 10f;  //나중에 화면 비율로 계산하면 좋을거같음
    [Header("Limit")]
    public float min = 0;
    public float max = 19;

    // Start is called before the first frame update
    void Start()
    {
        target = Player.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
    }

    void Follow()
    {
        var position = new Vector3(0, height, target.position.z - offset);
        if(position.z < min)    position.z = min;
        if(position.z > max)    position.z = max;
        transform.position = position;
    }
}
