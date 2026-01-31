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

    // Key input maps

    // Movements
    [SerializeField] private KeyCode forward = KeyCode.W;
    [SerializeField] private KeyCode backward = KeyCode.S;
    [SerializeField] private KeyCode left = KeyCode.A;
    [SerializeField] private KeyCode right = KeyCode.D;
    [SerializeField] private KeyCode run = KeyCode.LeftShift;
    // Events
    [SerializeField] private KeyCode pause = KeyCode.Escape;
    [SerializeField] private KeyCode toggleCursor = KeyCode.C;
    [SerializeField] private KeyCode startDay = KeyCode.P;

    //Events for others to subscribe
    public event System.Action onPausePressed;
    public event System.Action onToggleCursorPressed;
    public event System.Action onDayStartPressed;

    // Singleton
    public static InputHandler InputHandlerInstance { get; private set; }
    private void Awake()
    {
        if (InputHandlerInstance != null && InputHandlerInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        InputHandlerInstance = this;
    }
    // Singleton

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

    }
}
