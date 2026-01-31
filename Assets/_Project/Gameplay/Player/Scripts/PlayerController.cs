using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerLocator playerLocator = new PlayerLocator(); // a class is _System
    private InputHandler inputScript;
    private PlayerWalk walkScript;
    private PlayerLook lookScript;
    // Game States
    private GameState gameState;

    void Awake()
    {
        walkScript = this.GetComponent<PlayerWalk>();
        lookScript = this.GetComponent<PlayerLook>();
    }
    void Start()
    {
        // since its singleton it must be load after awake() done to avoid race condition which already happened lol
        inputScript = InputHandler.InputHandlerInstance;
        playerLocator.register(this.transform);
        gameState = GameState.GameStateInstance;
    }

    void Update()
    {
        if (gameState.isPaused == false)
        {
            walkScript.Move(inputScript.moveInput, inputScript.isRunningInput);

            lookScript.MoveHead(inputScript.lookInput);
            lookScript.HeadBobbing(inputScript.isMoving, inputScript.isRunningInput);
        }
        // Debug.Log(input.moveInput.x + " " + input.moveInput.y);
    }
}
