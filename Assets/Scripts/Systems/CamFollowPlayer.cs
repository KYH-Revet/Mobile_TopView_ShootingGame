using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    Transform target;

    [Header("Camera Position")]
    public float height = 10f;
    public float offset = 10f;  //���߿� ȭ�� ������ ����ϸ� �����Ű���
    [Header("Limit")]
    public float min = 0;
    public float max = 19;

    // Start is called before the first frame update
    void Start()
    {
        target = Player.instance.transform;

        LetterBox();
    }

    
    // Update is called once per frame
    void Update()
    {
        Follow();
    }

    // Outside from letter box
    void OnPreCull() => GL.Clear(true, true, Color.black);


    void Follow()
    {
        var position = new Vector3(0, height, target.position.z - offset);
        if(position.z < min)    position.z = min;
        if(position.z > max)    position.z = max;
        transform.position = position;
    }

    void LetterBox()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / (9f / 16f); // (���� / ����)
        float scalewidth = 1f / scaleheight;
        
        if (scaleheight < 1)    // 
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else                    // 
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect;
    }

    
}
