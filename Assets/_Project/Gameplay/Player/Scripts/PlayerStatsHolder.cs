using UnityEngine;

public enum PlayerState
{
    Server,
    Brawl,
    Down
}
public class PlayerStatsHolder : MonoBehaviour
{
    public float HP;
    public PlayerState playerState;
    

    [Header("Movement stats")]
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float JumpHeight;

    [Header("Perk stats")]
    [SerializeField] private float Luck;
    [SerializeField] private float Income;

    [Header("Permanent Perks")]
    [SerializeField] private bool HasFloatingTray;
    [SerializeField] private bool HasSneakers;
    [SerializeField] private bool HasTablet;

    [Header("Numeric stats")]
    [SerializeField] private int TrayCapacity;

    // Public read-only access
    public float movementSpeed => MovementSpeed;
    public float jumpHeight => JumpHeight;
    public float luck => Luck;
    public float income => Income;
    public int trayCapacity => TrayCapacity;

    public bool hasFloatingTray => HasFloatingTray;
    public bool hasSneakers => HasSneakers;
    public bool hasTablet => HasTablet;

    void Awake()
    {
        // movements
        MovementSpeed = 12f;
        JumpHeight = 10f;

        // multiplier
        Luck = 0f;
        Income = 0f;

        // boolean (have/not have)
        HasFloatingTray = false;
        HasSneakers = false;
        HasTablet = false;
    }
}
