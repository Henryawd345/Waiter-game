using System;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] private float timeOfEachDay = 8; // in minutes

    // Game states
    public bool isPaused {get; private set;}
    public bool dayStarted {get; private set;}
    public float timeOfDayRemaining {get; private set;}

    // Player stats
    public float scoreTotal {get; private set;}
    public float scoreToday {get; private set;}
    public float highScore {get; private set;}
    public float money {get; private set;}
    public int DaysPassed {get; private set;}

    // Events for others to subscribe
    public event Action<float> OnMoneyChanged;
    public event Action<float> OnScoreChanged;
    public event Action<bool> OnPauseChanged;

    // singleton
    public static GameState instance {get; private set;}
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        // singleton stuff done

        // read stuff from save files
        // if file dont exist init everything to 0
        // saving system not done yet

        isPaused = false;
        dayStarted = false;
        timeOfDayRemaining = timeOfEachDay;
    }
    public void StartDay()
    {
        dayStarted = true;
        timeOfDayRemaining = timeOfEachDay * 60f;
    }

    public void AddScore(float amount)
    {
        if (amount <= 0) return;

        scoreToday += amount;
        scoreTotal += amount;

        if (scoreTotal > highScore)
            highScore = scoreTotal;

        // OnScoreChanged?.Invoke(amount);
    }
    void AddMoney(float money)
    {
        if (money >= 0) this.money += money;
    }
    public void SetPaused(bool paused)
    {
        isPaused = paused;
        Time.timeScale = paused ? 0f : 1f;

        // OnPaused?.Invoke(paused);
    }
}
