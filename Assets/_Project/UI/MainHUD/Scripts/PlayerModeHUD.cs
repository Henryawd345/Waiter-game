using TMPro;
using UnityEngine;

public class PlayerModeHUD : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private TMP_Text modeText;

    [Header("Labels")]
    [SerializeField] private string serverLabel = "Serving";
    [SerializeField] private string brawlLabel = "Brawl";
    [SerializeField] private string downLabel = "Down";

    [Header("Colors")]
    [SerializeField] private Color serverColor = new Color(0.3f, 0.7f, 1f);
    [SerializeField] private Color brawlColor = new Color(1f, 0.35f, 0.3f);
    [SerializeField] private Color downColor = Color.gray;

    private PlayerStatsHolder playerStats;
    private PlayerState lastState;

    void Start()
    {
        if (PlayerLocator.PlayerTransform != null)
            playerStats = PlayerLocator.PlayerTransform.GetComponent<PlayerStatsHolder>();

        if (playerStats != null)
            Refresh(playerStats.playerState);
    }

    void Update()
    {
        if (playerStats == null) return;

        if (playerStats.playerState != lastState)
            Refresh(playerStats.playerState);
    }

    private void Refresh(PlayerState state)
    {
        lastState = state;
        if (modeText == null) return;

        switch (state)
        {
            case PlayerState.Server:
                modeText.text = serverLabel;
                modeText.color = serverColor;
                break;
            case PlayerState.Brawl:
                modeText.text = brawlLabel;
                modeText.color = brawlColor;
                break;
            case PlayerState.Down:
                modeText.text = downLabel;
                modeText.color = downColor;
                break;
        }
    }
}
