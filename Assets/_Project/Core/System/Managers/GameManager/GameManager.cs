using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CustomerManager customerManager;
    private InputHandler inputHandler;
    private GameState gameState;

    void Awake()
    {
        customerManager = GetComponent<CustomerManager>();

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
    }

    void Update()
    {
        ControlOtherManagers();
    }
    void ControlOtherManagers()
    {
        if (gameState.isDayStarted && !gameState.isPaused)
        {
            //customerManager.
        }
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
