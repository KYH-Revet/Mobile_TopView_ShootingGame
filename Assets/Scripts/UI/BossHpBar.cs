using System;
using UnityEngine;

public class BossHpBar : HPBar, IObserver<GameManager.GameState>
{
    // Unity Functions
    void Awake()
    {
        
    }
    void Start()
    {
        // Observer Pattern
        Subscribe();
    }
    void Update()
    {
        
    }

    // Calling from other script(Character.GetDamage()
    public override void HPSynchronization()
    {
        if (owner == null)
        {
            Debug.LogError("HPBar.cs void HPSynchronization() : boss is null");
            return;
        }
        
        // Move HP bar
        float rate = 1f - (owner.stat.hp / owner.stat.maxHp);
        cur_hp.localPosition = new Vector2(cur_hp.rect.width * rate * -1f, 0);
    }

    // Observer Pattern
    public void Subscribe()
    {
        GameManager.instance.Subscribe(this);   //GameManger
    }
    public void UnSubscribe()
    {
        GameManager.instance.UnSubscribe(this); //GameManger
    }
    public void OnCompleted()
    {
        throw new NotImplementedException();
    }
    public void OnError(Exception error)
    {
        Debug.LogError(error.ToString());
    }
    public void OnNext(GameManager.GameState value)
    {
        // Destroy UI when Win or Lose
        switch(value)
        {
            case GameManager.GameState.Win:
            case GameManager.GameState.Lose:
                UnSubscribe();          // Observer Pattern
                Destroy(gameObject);    // Destroy UI
                break;
        }
    }
}
