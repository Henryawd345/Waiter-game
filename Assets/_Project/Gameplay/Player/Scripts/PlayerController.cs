using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputHandler inputScript;
    private PlayerWalk walkScript;
    private PlayerLook lookScript;
    private PlayerInteractions interactionsScript;
    // Game States
    private GameState gameState;
    private PlayerStatsHolder playerStatsHolder;

    void Awake()
    {
        walkScript = this.GetComponent<PlayerWalk>();
        lookScript = this.GetComponent<PlayerLook>();
        interactionsScript = this.GetComponent<PlayerInteractions>();
        playerStatsHolder = this.GetComponent<PlayerStatsHolder>();

        PlayerLocator.register(this.transform); // register self transform to singleton before start (bc other codes will read from start and this will solve race condition problem)
    }
    void Start()
    {
        // since its singleton it must be load after awake() done to avoid race condition which already happened lol
        inputScript = InputHandler.Instance;
        gameState = GameState.Instance;

        inputScript.onInteract += OnInteract;
        inputScript.onThrowItem += OnThrowItem;
        inputScript.onMode1 += SwitchMode1;
        inputScript.onMode2 += SwitchMode2;
        inputScript.onMode3 += SwitchMode3;
        inputScript.onSwapItem += OnSwapItem;
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

    // Interact Buttons
    void OnInteract() {interactionsScript.OnInteract();}
    void OnThrowItem() {interactionsScript.OnThrowItem();}
    void SwitchMode1() {interactionsScript.SwitchMode1();}
    void SwitchMode2() {interactionsScript.SwitchMode2();}
    void SwitchMode3() {interactionsScript.SwitchMode3();}
    public void OnSwapItem(bool isScrollUp) {interactionsScript.OnSwapItem(isScrollUp);}
}
