using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GoodCustomerManager goodCustomerManager;
    private RudeCustomerManager rudeCustomerManager;
    private InputHandler inputHandler;
    private GameState gameState;

    void Awake()
    {
        goodCustomerManager = transform.parent.GetComponentInChildren<GoodCustomerManager>();
        rudeCustomerManager = transform.parent.GetComponentInChildren<RudeCustomerManager>();

        goodCustomerManager.RegisterGameManager(this);

        // since game start with not paused so mouse will disappear at first load
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Start()
    {
        inputHandler = InputHandler.Instance;
        gameState = GameState.Instance;

        inputHandler.onPausePressed += PauseButtonPressed;
        inputHandler.onToggleCursorPressed += ToggleCursorButtonPressed;
        inputHandler.onDayStartPressed += DayStartButtomPressed;

        gameState.OnDayStartedEvent += DayStarted;
    }

    void DayStarted()
    {   // start every events
        goodCustomerManager.StartDay();
        rudeCustomerManager.StartDay();
    }
    void EndDay()
    {
        goodCustomerManager.EndDay();
        rudeCustomerManager.EndDay();
    }
    public void SpawnRudeCustomer(Vector3 position)
    {
        rudeCustomerManager.TrySpawnCustomer(position);
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
        Debug.Log("Day started!");
        gameState.StartDay();
    }
}
