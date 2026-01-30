using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputHandler inputScript;
    private PlayerWalk walkScript;
    private PlayerLook lookScript;

    void Awake()
    {
        walkScript = this.GetComponent<PlayerWalk>();
        lookScript = this.GetComponent<PlayerLook>();
    }
    void Start()
    {
        // since its singleton it must be load after awake() done to avoid race condition which already happened lol
        inputScript = InputHandler.InputHandlerInstance;
    }

    void Update()
    {
        if (walkScript != null)
            walkScript.Move(inputScript.moveInput, inputScript.isRunningInput);

        if (lookScript != null)
        {
            lookScript.MoveHead(inputScript.lookInput);
            lookScript.HeadBobbing(inputScript.isMoving, inputScript.isRunningInput);
        }
        // Debug.Log(input.moveInput.x + " " + input.moveInput.y);
    }
}
