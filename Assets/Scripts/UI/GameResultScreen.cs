using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameResultScreen : MonoBehaviour, IObserver<GameManager.GameResult>
{
    [Header("Text")]
    public Text txt_result; // 게임 결과
    public Text txt_time;   // 플레이 시간
    public Text txt_stage;  // 도달한 스테이지

    [Header("Button")]
    public Button btn_confirm;

    // Unity Function
    void Start()
    {
        // Observer Pattern
        Subscribe();

        // Link
        GameManager.instance.gameResultScreen = this;

        // UI Off
        gameObject.SetActive(false);
    }

    // Button Function
    public void Btn_Confirm()
    {
        // Observer Pattern
        GameManager.instance.UnSubscribe(this);

        // Destroy objects who dont destroy
        GameManager.instance.DestroyDontDestroyObject();

        // Return to lobby
        SceneManager.LoadScene("StartScene");
    }

    // Observer Pattern
    private void Subscribe()
    {
        GameManager.instance.Subscribe(this);   // GameManager
    }
    public void OnCompleted()
    {
        throw new NotImplementedException();
    }
    public void OnError(Exception error)
    {
        Debug.LogError(error.ToString());
    }
    public void OnNext(GameManager.GameResult value)
    {
        switch(value)
        {
            case GameManager.GameResult.Win:
            case GameManager.GameResult.Lose:
                // UI On
                gameObject.SetActive(true);

                // Text : Game Result
                txt_result.text = value == GameManager.GameResult.Win ? "Victory" : "Lose";

                // Text : Play time
                float time = GameManager.instance.playTime;
                txt_time.text = ((int)(time / 60)).ToString() + ":" + ((int)(time % 60)).ToString();

                // Text :Stage
                string sceneName = SceneManager.GetActiveScene().name;
                txt_stage.text = "Stage " + sceneName.Substring("Stage".Length);
                break;
        }
    }
}
