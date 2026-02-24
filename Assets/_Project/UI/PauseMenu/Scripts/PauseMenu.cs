using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private GameObject pauseMenuPanel;
    private GameState gameState;

    void Awake()
    {
        pauseMenuPanel = GameObject.Find("PauseMenuPanel");
        pauseMenuPanel?.SetActive(false);
    }
    void Start()
    {
        gameState = GameState.Instance;
        gameState.OnPauseChangedEvent += OnPause;
    }

    public void OnPause(bool isPause)
    {
        if (isPause == true)
            pauseMenuPanel?.SetActive(true);
        else
            pauseMenuPanel?.SetActive(false);
    }
}
