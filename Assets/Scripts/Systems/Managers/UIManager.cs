using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Singleton Pattern
    public static UIManager instance { get; private set; }
    
    // Queue for UI On/Off
    static Queue<GameObject> uiQueue = new Queue<GameObject>();

    private void Update()
    {
        UIOnOff();
    }

    // Game Pause
    public void Pause(GameObject panel_Pause)
    {
        // Reverse the active self
        panel_Pause.SetActive(!panel_Pause.activeSelf);

        // GameManager GameResult = panel On ? Pause : Processing
        TimeScale(panel_Pause.activeSelf ? 0f : 1f);
    }

    // Turn queued UI on/off
    void UIOnOff()
    {
        while(uiQueue.Count > 0)
        {
            GameObject obj = uiQueue.Dequeue();
            obj.SetActive(!obj.activeSelf);
        }
    }

    // Add UI(On/Off) in queue
    public void UIOnOff(GameObject targetUI)
    {
        uiQueue.Enqueue(targetUI);
    }

    /// <summary> Time scale </summary>
    public void TimeScale(float value)
    {
        // 나중에 GameManager에 함수를 만들고 호출하게 할 수도 있음
        switch(value)
        {
            case 0:
                GameManager.instance.SetGameResult(GameManager.GameResult.Pause);
                break;
            case 1:
                GameManager.instance.SetGameResult(GameManager.GameResult.Processing);
                break;
        }
    }

    public void BackToLoby()
    {
        // Game Play
        TimeScale(1f);

        // Destroy objects who dont destroy
        GameManager.instance.DestroyDontDestroyObject();

        // Return to lobby
        SceneManager.LoadScene("StartScene");
    }

    /// <summary> Game Exit </summary>
    public void ApplicationExit()
    {
        Application.Quit();
    }
}
