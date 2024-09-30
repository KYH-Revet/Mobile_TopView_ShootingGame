using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Transform target;

    [Header("Camera Position")]
    public float height = 10f;
    public float offset = 10f;  //나중에 화면 비율로 계산하면 좋을거같음
    [Header("Limit")]
    public float min = 0;
    public float max = 19;

    [Header("Letter Box")]
    public bool letterBox = true;
    public Vector2 resolutionRatio = new Vector2(9f, 16f);

    // Start is called before the first frame update
    void Start()
    {
        if(Player.instance != null)
            target = Player.instance.transform;

        if(letterBox)
            LetterBox();
    }

    
    // Update is called once per frame
    void Update()
    {
        if ((target != null))
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
        float scaleheight = ((float)Screen.width / Screen.height) / (resolutionRatio.x / resolutionRatio.y); // (가로 / 세로)
        float scalewidth = 1f / scaleheight;
        
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect;
    }
}
