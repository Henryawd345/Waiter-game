using UnityEngine;

public class OrderButton : MonoBehaviour
{
    public FoodCounter boundFoodCounter {get; private set;}
    private FoodTypes foodType;
    private GameObject glowOutlineGameObject;

    void Awake()
    {
        glowOutlineGameObject = GameObject.Find("Visuals/ButtonGlowOutline");
        StopGlow();
    }
    public void Init(FoodCounter foodCounter, FoodTypes foodType)
    {
        boundFoodCounter = foodCounter;
        this.foodType = foodType;
    }

    public void OnPressed()
    {
        // Debug.Log("Button Pressed");
        if (boundFoodCounter != null)
        {
            boundFoodCounter.OrderFood(foodType);
        }
    }
    public void Glow()
    {
        glowOutlineGameObject?.SetActive(true);
    }
    public void StopGlow()
    {
        glowOutlineGameObject?.SetActive(false);
    }
}
