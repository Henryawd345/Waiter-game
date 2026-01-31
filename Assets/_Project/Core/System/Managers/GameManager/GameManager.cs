using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GoodCustomerManager goodCustomerManager;
    private InputHandler inputHandler;
    private GameState gameState;

    void Awake()
    {
        goodCustomerManager = GetComponent<GoodCustomerManager>();

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

        gameState.DayStarted += DayStarted;
    }

    void DayStarted()
    {   // start every events
        goodCustomerManager.StartDay();
    }
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
}
