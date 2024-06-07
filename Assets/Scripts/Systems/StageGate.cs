using System;
using UnityEngine;

public class StageGate : MonoBehaviour, IObserver<GameManager.GameResult>
{
    [SerializeField]
    string nextScene;
    bool stageClear = false;

    // Unity Functions
    void Start()
    {
        //Observer Pattern
        Subscribe();
    }
    
    // Load Scene
    void OnTriggerEnter(Collider other)
    {
        if (stageClear && other.GetComponent<Player>() != null)
        {
            // Observer Pattern
            UnSubscribe();

            // Player Reset
            Player.instance.ResetPlayer();

            // Load Scene
            GameManager.instance.NextStage(nextScene);
        }
    }

    // Observer Pattern
    void Subscribe()
    {
        GameManager.instance.Subscribe(this);   // GameManager
    }
    void UnSubscribe()
    {
        GameManager.instance.UnSubscribe(this); // GameManager
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
        stageClear = value == GameManager.GameResult.StageClear;
    }
}
