using System.Collections.Generic;
using UnityEngine;

public class GameplayObjectList : MonoBehaviour
{
    private GameObject tableRoot;
    private GameObject foodCounterRoot;
    public GameObject customerSpawnPoint {get; private set;}
    public List<Table> tablesList {get; private set;} = new();
    public List<FoodCounter> foodCountersList {get; private set;} = new();
    // public List<Furnitures> furnituresList;
    public static GameplayObjectList Instance;
    void Awake()
    {
        // singleton
        if (Instance == null || Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
        // singleton

        tableRoot = GameObject.Find("_GameplayObjects/Tables");
        foodCounterRoot = GameObject.Find("_GameplayObjects/Counters");
        customerSpawnPoint = GameObject.Find("_EventLocations/CustomerSpawnPoint");
        // Debug.Log("Start with " + furnituresList.Count + " breakable furnitures");
    }
    void Start()
    {
        if (tableRoot == null) // check if root exist or not
        {
            Debug.Log("No TableRoot found!");
            return;
        }
        if (foodCounterRoot == null) // check if root exist or not
        {
            Debug.Log("No FoodCounterRoot found!");
            return;
        }
        if (customerSpawnPoint == null)
        {
            Debug.Log("No customer spawn point found!");
            return;
        }

        tablesList.AddRange(tableRoot.GetComponentsInChildren<Table>(includeInactive: true));
        foodCountersList.AddRange(foodCounterRoot.GetComponentsInChildren<FoodCounter>(includeInactive: true));

        Debug.Log("Start with " + tablesList.Count + " tables");
        Debug.Log("Start with " + foodCountersList.Count + " counters");
        foreach (FoodCounter counter in foodCountersList)
            Debug.Log("   Counter: " + counter.GetFoodTypes().ToString());
    }
}
