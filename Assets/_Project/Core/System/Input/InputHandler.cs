using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour // Is a singleton
{
    public Vector2 moveInput { get; private set; }
    public Vector2 lookInput { get; private set; }
    public bool isMoving { get; private set; }
    public bool isRunningInput { get; private set; }

    //Key input maps
    private KeyCode forward = KeyCode.W;
    private KeyCode backward = KeyCode.S;
    private KeyCode left = KeyCode.A;
    private KeyCode right = KeyCode.D;
    private KeyCode run = KeyCode.LeftShift;

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
    }
}
