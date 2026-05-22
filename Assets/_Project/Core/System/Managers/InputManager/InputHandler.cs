using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour // Is a singleton
{
    // Reference for other scripts
    public Vector2 moveInput { get; private set; }
    public Vector2 lookInput { get; private set; }
    public bool isMoving { get; private set; }
    public bool isRunningInput { get; private set; }
    public bool isAttackHeld { get; private set; }

    // Key input maps

    // Movements
    [SerializeField] private KeyCode forward = KeyCode.W;
    [SerializeField] private KeyCode backward = KeyCode.S;
    [SerializeField] private KeyCode left = KeyCode.A;
    [SerializeField] private KeyCode right = KeyCode.D;
    [SerializeField] private KeyCode run = KeyCode.LeftShift;
    // Action Events
    [SerializeField] private KeyCode attack = KeyCode.Mouse0;
    [SerializeField] private KeyCode throwItem = KeyCode.Q;
    [SerializeField] private KeyCode interact = KeyCode.E;
    [SerializeField] private KeyCode mode1 = KeyCode.Alpha1;
    [SerializeField] private KeyCode mode2 = KeyCode.Alpha2;
    [SerializeField] private KeyCode mode3 = KeyCode.Alpha3;
    [SerializeField] private KeyCode swapItemUp = KeyCode.Mouse3;
    [SerializeField] private KeyCode swapItemDown = KeyCode.Mouse4;
    // Gameplay Events
    [SerializeField] private KeyCode pause = KeyCode.Escape;
    [SerializeField] private KeyCode toggleCursor = KeyCode.C;
    [SerializeField] private KeyCode startDay = KeyCode.P;
    private GameState gameState;

    //Events for others to subscribe
    // Action Events
    public event System.Action onAttack;
    public event System.Action onThrowItem;
    public event System.Action onInteract;
    public event System.Action onMode1;
    public event System.Action onMode2;
    public event System.Action onMode3;
    public event System.Action<bool> onSwapItem;
    // Gameplay Events
    public event System.Action onPausePressed;
    public event System.Action onToggleCursorPressed;
    public event System.Action onDayStartPressed;

    // Singleton
    public static InputHandler Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    // Singleton
    void Start()
    {
        gameState = GameState.Instance;
    }
    void Update()
    {

        //--------------------------------------------------------------------------//
        // Movement Inputs
        //--------------------------------------------------------------------------//
        // Walk input
        float x = 0;
        float y = 0;

        if (Input.GetKey(right)) x += 1;
        if (Input.GetKey(left))  x -= 1;
        if (Input.GetKey(forward)) y += 1;
        if (Input.GetKey(backward)) y -= 1;

        // Assign WALK variables
        moveInput = new Vector2(x, y).normalized;
        isRunningInput = Input.GetKey(run);
        // Assign LOOK variables
        lookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        isMoving = !(moveInput.x == 0 && moveInput.y == 0);

        //--------------------------------------------------------------------------//
        // UI Input (Maybe)
        //--------------------------------------------------------------------------//
        if (Input.GetKeyDown(pause))
            onPausePressed?.Invoke();
        if (Input.GetKeyDown(toggleCursor))
            onToggleCursorPressed?.Invoke();
        if (Input.GetKeyDown(startDay))
            onDayStartPressed?.Invoke();

        isAttackHeld = false;
        if (gameState.isPaused == false)
        {
            isAttackHeld = Input.GetKey(attack);

            if (Input.GetKeyDown(attack))
                onAttack?.Invoke();
            if (Input.GetKeyDown(throwItem))
                onThrowItem?.Invoke();
            if (Input.GetKeyDown(interact))
                onInteract?.Invoke();

            if (Input.GetKeyDown(mode1)) onMode1?.Invoke();
            if (Input.GetKeyDown(mode2)) onMode2?.Invoke();
            if (Input.GetKeyDown(mode3)) onMode3?.Invoke();

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0f)
                onSwapItem?.Invoke(true);
            else if (scroll < 0f)
                onSwapItem?.Invoke(false);
        }
    }
}
