using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    GameManager.GameState beforeGameState;

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

        // GameManager GameState = panel On ? Pause : Processing
        if (panel_Pause.activeSelf)
        {
            // Record last State (Processing or StageClear)
            beforeGameState = GameManager.instance.gameState;
            GameManager.instance.SetGameState(GameManager.GameState.Pause);
        }
        else
            GameManager.instance.SetGameState(beforeGameState);
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

    public void BackToLoby()
    {
        // Game Play
        Time.timeScale = 1.0f;

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
