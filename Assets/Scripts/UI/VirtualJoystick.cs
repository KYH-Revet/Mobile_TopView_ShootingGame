using UnityEngine.UI;
using UnityEngine;
using System;

public class VirtualJoystick : MonoBehaviour, IObserver<GameManager.GameState>
{
    Player player;
    [Tooltip("조이스틱 구성 이미지")]
    Image[] images;

    [SerializeField]
    RectTransform lever;
    RectTransform rectTransform;

    // Joystick input values
    [SerializeField, Range(10, 150)]
    float leverRange = 30;          // Max of lever range
    Vector2 inputDirection;         // Calculated direction vector(lever)
    public bool isWorking = true;   // Joystick is working?
    bool receivingInput = false;    // Joystick is receiving input from player?
    
    //Unity Functions
    void Start()
    {
        // Get Player
        player = Player.instance;
        if (player == null)
        {
            Debug.LogError("VirtualJoystick.cs : player is null");
            Destroy(gameObject);
            return;
        }

        // Get Rect Transform
        rectTransform = GetComponent<RectTransform>();

        // Images Off
        images = GetComponentsInChildren<Image>();
        ImageAlpha(0f);

        // Is working?
        isWorking = GameManager.instance.gameState == GameManager.GameState.Processing;

        // Observer Pattern
        Subscribe();

        // Debug Platform
#if UNITY_IOS || UNITY_ANDROID  // Mobile Touch
        Debug.Log("IOS or Android");
#else                           // Test Mouse
        Debug.Log("PC");
#endif

    }

    void Update()
    {
        // Do nothing
        if (!isWorking)
            return;

        // Joystick
#if UNITY_IOS || UNITY_ANDROID  // Mobile Touch
        OnTouch();
#else                           // Test Mouse
        OnMouseButton();
#endif
    }

    void FixedUpdate()
    {
        // Do nothing
        if (!isWorking)
            return;

        // Receive input
        if (receivingInput) // Player is Move
        {
            LeverDrag(Input.mousePosition);
            player.Move(inputDirection);
        }
    }

    // Joystick Functions
    void OnMouseButton()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // UI
            ImageAlpha(0.5f);
            receivingInput = true;

            // Stick position
            transform.position = Input.mousePosition;

            // StateMachin
            player.ChangeState(Character._StateMachine.Move);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // UI
            ImageAlpha(0f);
            receivingInput = false;

            // StateMachin
            player.ChangeState(Character._StateMachine.Attack);
        }
    }
    void OnTouch()
    {
        if (Input.touchCount <= 0)
            return;

        Touch touch = Input.GetTouch(0);
        switch (touch.phase)
        {
            case TouchPhase.Began:
                if(!receivingInput)
                {
                    // UI On
                    ImageAlpha(0.5f);
                    receivingInput = true;

                    // Stick Position
                    transform.position = Input.GetTouch(0).rawPosition;
                }
                break;
            case TouchPhase.Moved:
                // Update position
                LeverDrag(Input.GetTouch(0).position);

                // StateMachin
                player.ChangeState(Character._StateMachine.Move);
                break;
            case TouchPhase.Ended:
                // UI Off
                ImageAlpha(0f);
                receivingInput = false;

                // StateMachin
                player.ChangeState(Character._StateMachine.Attack);
                break;
        }
    }
    void LeverDrag(Vector2 eventPos)
    {
        // Direction vector
        var inputPos = eventPos - (Vector2)transform.position;
        // Input vector = 0 ~ lever range
        var inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        // Repositioning to lever(Red circle)
        lever.anchoredPosition = inputVector;
        // Input direction = 0 ~ 1
        inputDirection = inputVector / leverRange;
    }


    // UI Function
    void ImageAlpha(float alpha)
    {
        foreach (var image in images)
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }

    // Observer Pattern Observe : Game State
    private void Subscribe()
    {
        // GameManager GameState
        GameManager.instance.Subscribe(this);
    }
    private void UnSubscribe()
    {
        // GameManager GameState
        GameManager.instance.UnSubscribe(this);
    }
    public void OnCompleted()
    {
        throw new NotImplementedException();
    }
    public void OnError(Exception error)
    {
        Debug.LogError(error.ToString());
    }

    /// <summary>Observe : Game state Changed</summary>
    /// <param name="value">GameManaer.instance.gameState</param>
    public void OnNext(GameManager.GameState value)
    {
        switch (value)
        {
            // UnPause the stick function
            case GameManager.GameState.Processing:
            case GameManager.GameState.StageClear:
                isWorking = true;
                break;

            // Pause the stick function
            case GameManager.GameState.RewardSelect:
            case GameManager.GameState.Pause:
                isWorking = false;
                break;

            // No more need
            case GameManager.GameState.Win:
            case GameManager.GameState.Lose:
                UnSubscribe();          // Observer Pattern
                Destroy(gameObject);    // Destroy the joystick
                break;
        }

        // Reset input sign
        receivingInput = false;

        // UI Off
        ImageAlpha(0f);
    }
}