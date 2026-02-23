using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private InputHandler inputScript;
    // Game States
    private GameState gameState;
    private PlayerInventory playerInventory;
    private PlayerStatsHolder playerStatsHolder;
    private Transform playerCameraTransform;
    private float lookDistance = 4.5f;

    // private just for code's
    FoodItem lastGlowedFood = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerStatsHolder = this.GetComponent<PlayerStatsHolder>();
        playerInventory = this.GetComponent<PlayerInventory>();
        playerCameraTransform = GetComponentInChildren<Camera>().transform;
    }
    void Start()
    {
        gameState = GameState.Instance;
    }
    void Update()
    {
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit hit, lookDistance))
        {
            if (hit.collider.TryGetComponent<FoodItem>(out FoodItem foodHitted))
            {
                if (foodHitted != lastGlowedFood)
                {
                    lastGlowedFood?.StopGlow();
                    lastGlowedFood = foodHitted;
                    lastGlowedFood.Glow();
                }
            }
            else
            {
                lastGlowedFood?.StopGlow();
                lastGlowedFood = null;
            }
        }
        else
        {
            lastGlowedFood?.StopGlow();
            lastGlowedFood = null;
        }
    }
    public void OnInteract()
    {
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit hit, lookDistance))
        {
            GameObject obj = hit.collider.gameObject;
            // Debug.Log(obj.gameObject.name);
            
            if (hit.collider.TryGetComponent<FoodItem>(out FoodItem foodHitted))
            {
                if (!playerInventory.PickUpFood(foodHitted)) Debug.Log("Inventory Full.");
            }
        }
    }
    public void OnThrowItem()
    {
        playerInventory.ThrowFood(playerCameraTransform);
    }
    public void SwitchMode1() // server mode
    {
        
    }
    public void SwitchMode2() // brawl mode
    {
        if (playerStatsHolder.hasFloatingTray == false)
        {
            playerInventory.ThrowAllFood(playerCameraTransform);
        }
    }
    public void SwitchMode3()
    {
        
    }
    public void OnSwapItem(bool isScrollUp)
    {
        playerInventory.SwapFood(isScrollUp);
    }
}
