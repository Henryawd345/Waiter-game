using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerLocator playerLocator; // a class is _System

    private InputHandler inputScript;
    private PlayerWalk walkScript;
    private PlayerLook lookScript;

    void Awake()
    {
        walkScript = this.GetComponent<PlayerWalk>();
        lookScript = this.GetComponent<PlayerLook>();
        playerLocator = GameObject.Find("_System/PlayerLocator").transform.GetComponent<PlayerLocator>();
    }
    void Start()
    {
        // since its singleton it must be load after awake() done to avoid race condition which already happened lol
        inputScript = InputHandler.InputHandlerInstance;
        // subscribe this to playerLocator for other codes find player's position easily
        playerLocator.register(this.transform);
    }

    void Update()
    {
        walkScript.Move(inputScript.moveInput, inputScript.isRunningInput);

        lookScript.MoveHead(inputScript.lookInput);
        lookScript.HeadBobbing(inputScript.isMoving, inputScript.isRunningInput);
        // Debug.Log(input.moveInput.x + " " + input.moveInput.y);
    }
}
