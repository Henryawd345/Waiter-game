using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GoodCustomerManager goodCustomerManager;
    private InputHandler inputHandler;
    private GameState gameState;

    void Awake()
    {
        goodCustomerManager = transform.parent.GetComponentInChildren<GoodCustomerManager>();

        // since game start with not paused so mouse will disappear at first load
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Start()
    {
        inputHandler = InputHandler.InputHandlerInstance;
        gameState = GameState.GameStateInstance;

        inputHandler.onPausePressed += PauseButtonPressed;
        inputHandler.onToggleCursorPressed += ToggleCursorButtonPressed;
        inputHandler.onDayStartPressed += DayStartButtomPressed;

        gameState.DayStarted += DayStarted;
    }

    void DayStarted()
    {   // start every events
        goodCustomerManager.StartDay();
    }


    // Button Pressed Managing
    void PauseButtonPressed()
    {
        gameState.TogglePause();
        if (gameState.isPaused)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    void ToggleCursorButtonPressed()
    {
        
    }
    void DayStartButtomPressed()
    {
        gameState.StartDay();
    }
}
