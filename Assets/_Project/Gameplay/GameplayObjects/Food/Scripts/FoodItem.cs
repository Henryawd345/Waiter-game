using UnityEngine;

public class FoodItem : MonoBehaviour
{
    //Adjustable variables
    [SerializeField] public float initFoodColdTime = 30; // in seconds

    [SerializeField] private FoodTypes foodType; // show foodtype in inspector
    public FoodTypes FoodType => foodType;
    public FoodStates foodStates {get; private set;}
    public float foodColdTime;

    void Start()
    {
        foodColdTime = initFoodColdTime;
    }
    void Update()
    {
        if (foodType != FoodTypes.Soda 
            && foodType != FoodTypes.Juice 
            && foodType != FoodTypes.Water)
            {foodColdTime -= Time.deltaTime;}
    }
}
