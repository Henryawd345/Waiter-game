using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputHandler inputScript;
    private PlayerWalk walkScript;
    private PlayerLook lookScript;

    void Awake()
    {
        inputScript = InputHandler.InputHandlerInstance;
        walkScript = GetComponentInChildren<PlayerWalk>();
        lookScript = GetComponentInChildren<PlayerLook>();
    }

    void Update()
    {
        walkScript.Move(inputScript.moveInput, inputScript.isRunningInput);

        lookScript.MoveHead(inputScript.lookInput);
        lookScript.HeadBobbing(inputScript.isMoving, inputScript.isRunningInput);
        // Debug.Log(input.moveInput.x + " " + input.moveInput.y);
    }
}
