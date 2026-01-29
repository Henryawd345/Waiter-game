using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 moveInput;
    public Vector2 lookInput;
    public bool isMoving;
    public bool isRunningInput;

    private KeyCode forward = KeyCode.W;
    private KeyCode backward = KeyCode.S;
    private KeyCode left = KeyCode.A;
    private KeyCode right = KeyCode.D;
    private KeyCode run = KeyCode.LeftShift;

    void Update()
    {
        // Walk
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
    }
}
