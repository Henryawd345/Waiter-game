using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputHandler inputScript;
    private PlayerWalk walkScript;
    private PlayerLook lookScript;
    private GameState gameState;

    void Awake()
    {
        walkScript = this.GetComponent<PlayerWalk>();
        lookScript = this.GetComponent<PlayerLook>();

        PlayerLocator.register(this.transform); // register self transform to singleton before start (bc other codes will read from start and this will solve race condition problem)
    }
    void Start()
    {
        // singletons must be read in Start, after every Awake() has run, to avoid init-order race conditions
        inputScript = InputHandler.Instance;
        gameState = GameState.Instance;
    }

    void Update()
    {
        if (gameState.isPaused == false)
        {
            walkScript.Move(inputScript.moveInput, inputScript.isRunningInput);

            lookScript.MoveHead(inputScript.lookInput);
            lookScript.HeadBobbing(inputScript.isMoving, inputScript.isRunningInput);
        }
    }
}
