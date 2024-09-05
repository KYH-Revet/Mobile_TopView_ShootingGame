using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{    
    // Singleton Pattern
    public static UIManager instance { get; private set; }
    public bool dontDestroy = true;

    // For Unpause
    GameManager.GameState beforeGameState;

    // Queue for UI On/Off
    static Queue<GameObject> uiQueue = new Queue<GameObject>();

    // Text for stage name
    [Header("Text")]
    public Text text_Stage;

    private void Awake()
    {
        // Singleton Pattern
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }
    private void Start()
    {
        // Text for stage name
        if(text_Stage != null)
            text_Stage.text = SceneManager.GetActiveScene().name;

        // Don't Destroy On Load
        if (dontDestroy && GameManager.instance != null)
            GameManager.instance.AddDontDestroyObjects(gameObject);
    }
    private void Update()
    {
        UIOnOff();
    }

    // UI values Update
    public void UIUpdate()
    {
        // Text for stage name
        TextUpdate(text_Stage, SceneManager.GetActiveScene().name);
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

    // Stage Text Update
    void TextUpdate(Text targetUI, string text)
    {
        if (targetUI != null)
            targetUI.text = text;
        else
            Debug.LogError(targetUI + " is null!");
    }

    // Back to Lobby Scene
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
