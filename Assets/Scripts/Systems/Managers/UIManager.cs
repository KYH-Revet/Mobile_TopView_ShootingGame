using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IObserver<GameManager.GameState>
{
    // For Unpause
    GameManager.GameState beforeGameState;

    [Header("UI Object")]
    public GameObject uiObj_PauseBtn;
    
    // Queue for UI On/Off
    static Queue<GameObject> uiQueue = new Queue<GameObject>();
    
    // Text for stage name
    [Header("Text")]
    public Text text_Stage;

    // Unity functions
    private void Awake()
    {
        // Text for stage name
        TextUpdate(text_Stage, SceneManager.GetActiveScene().name);
    }
    private void Start()
    {
        // Text for stage name
        if(text_Stage != null)
            text_Stage.text = SceneManager.GetActiveScene().name;

        // Observer Pattern
        Subscribe();
    }
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
        //targetUI.SetActive(!targetUI.activeSelf);
        uiQueue.Enqueue(targetUI);
    }

    // Update the Text UI
    void TextUpdate(Text targetUI, string text)
    {
        if (targetUI != null)
            targetUI.text = text;
    }

    /// <summary>Back to Lobby Scene</summary>
    public static void BackToLoby()
    {        
        // Observer Pattern (For destroy the Bullets)
        GameManager.instance.SetGameState(GameManager.GameState.StageClear);

        // Destroy objects who dont destroy
        GameManager.instance.DestroyDontDestroyObject();

        // Game Play
        Time.timeScale = 1.0f;

        // Return to lobby
        SceneManager.LoadScene("StartScene");
    }

    /// <summary> Game Exit </summary>
    public static void ApplicationExit()
    {       
        Application.Quit();
    }

    // Observer Pattern
    void Subscribe()
    {
        // GameManager
        if(GameManager.instance != null)
            GameManager.instance.Subscribe(this);
    }
    void UnSubscribe()
    {
        // GameManager
        if(GameManager.instance != null)
            GameManager.instance.UnSubscribe(this);
    }
    public void OnCompleted()
    {
        throw new NotImplementedException();
    }
    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }
    public void OnNext(GameManager.GameState gameState)
    {
        switch(gameState)
        {
            case GameManager.GameState.Win:
            case GameManager.GameState.Lose:
            UIOnOff(uiObj_PauseBtn);
            break;
        }
    }
}
