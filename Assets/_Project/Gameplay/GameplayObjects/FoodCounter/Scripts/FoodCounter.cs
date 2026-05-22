using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FoodCounter : MonoBehaviour
{
    [SerializeField] private int foodQueueSize;
    [SerializeField] private FoodCounterTypes counterType;
    public FoodCounterTypes CounterType => counterType;
    [SerializeField] public GameObject foodPrefab;
    [SerializeField] public GameObject buttonPrefab;
    private float foodSpawnOffsetFromPrev = 1.5f;
    private float buttonPlaceOffsetFromPrev = 1.25f;
    private GameObject button1;
    private GameObject button2;
    private GameObject button3;
    private GameObject button4;
    private GameObject button5;
    private Vector3 buttonStartPosition;
    private Vector3 foodStartPosition;

    private Queue<FoodTypes> foodOrderQueue = new Queue<FoodTypes>();
    private List<FoodItem> foodItemsOnCounterList = new List<FoodItem>();
    private bool isCooking;

    // Update is called once per frame
    void Awake()
    {
        buttonStartPosition = transform.Find("PositionHolder/ButtonStartOffset").transform.position;
        foodStartPosition = transform.Find("PositionHolder/FoodStartOffset").transform.position;

        transform.SetParent(GameObject.Find("_GameplayObjects/Counters").transform);
        List<FoodTypes> availableFoodList = new List<FoodTypes>();

        int start = (int)counterType * 5;
        int end = start + 5;
        
        for (int i = start; i < end; i++)
            availableFoodList.Add((FoodTypes)i);

        for (int i = 0; i < 5; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab);
            OrderButton newButtonScript = newButton.GetComponent<OrderButton>();

            newButtonScript.Init(this, availableFoodList[i]);

            newButton.gameObject.transform.SetParent(transform);
            newButton.gameObject.transform.rotation = this.transform.rotation;
            newButton.transform.position = buttonStartPosition
                + (this.transform.right * -buttonPlaceOffsetFromPrev * i);
        }
    }

    public void OrderFood(FoodTypes foodType)
    {
        if (foodOrderQueue.Count < foodQueueSize)
        {
            foodOrderQueue.Enqueue(foodType);
        }

        if (isCooking == false)
        {
            StartCoroutine(CookingRoutine());
        }
    }
    private System.Collections.IEnumerator CookingRoutine()
    {
        isCooking = true;
        
        while (foodOrderQueue.Count > 0 && foodItemsOnCounterList.Count < 5)
        {
            // UnityEngine.Debug.Log(this.gameObject.name + " is Cooking");
            FoodTypes currentFood = foodOrderQueue.Dequeue();
            
            yield return new WaitForSeconds(5f); // cook time
            
            SpawnFood(currentFood);
        }
        
        isCooking = false;
    }
    private void SpawnFood(FoodTypes foodType)
    {
        if (foodPrefab != null)
        {
            GameObject newFood = Instantiate(foodPrefab);
            FoodItem newFoodItem = newFood.GetComponent<FoodItem>();

            foodItemsOnCounterList.Add(newFoodItem);
            newFoodItem.Init(this, foodType);
            
            RepositionAll();
        }
    }

    public void FoodIsPickedUp(FoodItem foodItem)
    {
        if (foodItemsOnCounterList.Contains(foodItem))
        {
            foodItemsOnCounterList.Remove(foodItem);
            RepositionAll();
        }
    }

    private void RepositionAll()
    {
        for (int i = 0; i < foodItemsOnCounterList.Count; i++)
        {
            foodItemsOnCounterList[i].transform.rotation = this.transform.rotation;
            foodItemsOnCounterList[i].transform.position = foodStartPosition
                + (this.transform.right * -foodSpawnOffsetFromPrev * i);
        }
    }
    public FoodCounterTypes GetFoodTypes()
    {
        return counterType;
    }
}
