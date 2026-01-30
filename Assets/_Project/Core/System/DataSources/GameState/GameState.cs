using System;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] private float timeOfEachDay = 8; // in minutes
    [SerializeField] private float startReputation = 100;

    // Game states
    public bool isPaused {get; private set;}
    public bool isDayStarted {get; private set;}
    public float timeOfDayRemaining {get; private set;}

    // Player stats
    public float restaurantReputation{get; private set;}
    public float scoreTotal {get; private set;}
    public float scoreToday {get; private set;}
    public float highScore {get; private set;}
    public float money {get; private set;}
    public int DaysPassed {get; private set;}

    // Events for others to subscribe
    public event Action<float> OnMoneyChanged;
    public event Action<float> OnScoreChanged;
    public event Action<float> OnReputationChanged;
    public event Action<bool> OnPauseChanged;

    // singleton
    public static GameState GameStateInstance {get; private set;}
    void Awake()
    {
        if (GameStateInstance != null && GameStateInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        GameStateInstance = this;
        // singleton stuff done

        // read stuff from save files
        // if file dont exist init everything to 0
        // saving system not done yet

        isPaused = false;
        isDayStarted = false;
        timeOfDayRemaining = timeOfEachDay;
        restaurantReputation = startReputation;
    }
    public void StartDay()
    {
        isDayStarted = true;
        timeOfDayRemaining = timeOfEachDay * 60f;
    }

    public void AddScore(float amount)
    {
        if (amount <= 0) return;

        scoreToday += amount;
        scoreTotal += amount;

        if (scoreTotal > highScore)
            highScore = scoreTotal;

        OnScoreChanged?.Invoke(amount);
    }
    void AddMoney(float money)
    {
        if (money < 0)
            return;

        this.money += money;
        OnMoneyChanged?.Invoke(money);
    }
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        Debug.Log("Paused : " + isPaused);

        OnPauseChanged?.Invoke(isPaused);
    }
    //reputation
    void OnReputationGain(float amount)
    {
        if (amount < 0)
            return;

        restaurantReputation += amount;
        OnReputationChanged?.Invoke(amount);
    }
    void OnReputationLost(float amount)
    {
        if (amount < 0)
            return;

        restaurantReputation -= amount;
        OnReputationChanged?.Invoke(-amount);
    }
}
