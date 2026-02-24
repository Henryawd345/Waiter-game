using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Order : MonoBehaviour
{
    // variables
    [SerializeField] int MinimumFoodPerCustomer = 2;
    [SerializeField] int MaximumFoodPerCustomer = 4;

    private List<FoodTypes> wantedFood;
    private HashSet<FoodTypes> servedFood = new HashSet<FoodTypes>();
    private GoodCustomer customerOrderOwner;
    private List<FoodTypes> avaliableFoodTypes = new List<FoodTypes>();

    public void Init(GoodCustomer customer)
    {
        customerOrderOwner = customer;
        GenerateRandomOrder();
    }
    void GenerateRandomOrder()
    {
        wantedFood = new List<FoodTypes>();
        GameplayObjectList gameplayObjectList = GameplayObjectList.Instance;

        if (gameplayObjectList == null)
        {
            Debug.Log("gameplayObjectList is null");
            return;
        }

        List<FoodTypes> allFoods = new List<FoodTypes>();
        for (int i = 0; i < gameplayObjectList.foodCountersList.Count; i++)
        {
            int start = (int)gameplayObjectList.foodCountersList[i].GetFoodTypes() * 5;
            for (int j = start; j < start + 5; j++)
                allFoods.Add((FoodTypes)j);
        }

        int count = Random.Range(MinimumFoodPerCustomer, MaximumFoodPerCustomer + 1); // customer will order atleast 2 food

        List<FoodTypes> pool = new List<FoodTypes>(allFoods);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, pool.Count);
            wantedFood.Add(pool[index]);
            pool.RemoveAt(index);
        }

        //for debug purpose
        // for (int i = 0; i < wantedFood.Count; i++)
        // {
        //     Debug.Log("Food: " + wantedFood[i] + gameObject.name);
        // }
        // Debug.Log("------------------");
    }

    private void TryServeFood(FoodItem food)
    {
        if (servedFood.Contains(food.foodType))
        {
            RejectFood(food, true);
            return;
        }

        if (wantedFood.Contains(food.foodType))
        {
            AcceptFood(food);
        }
        else
        {
            RejectFood(food, false);
        }
    }

    private void AcceptFood(FoodItem food)
    {
        servedFood.Add(food.foodType);
        Destroy(food.gameObject);

        if (IsOrderComplete())
        {
            customerOrderOwner.OnOrderCompleted();
        }
    }

    private void RejectFood(FoodItem food, bool reason)
    {
        Destroy(food.gameObject);
        // reason true = duped food, false = unmatched food
        if (reason == true) // duped food
        {
            customerOrderOwner.ApplyAnnoyance(5f);
        }
        else // unmatched food
        {
            customerOrderOwner.ApplyAnnoyance(5f);
        }
    }

    private bool IsOrderComplete()
    {
        return servedFood.Count == wantedFood.Count;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<FoodItem>(out FoodItem food))
        {
            if (food.isBeingHeld == false)
                TryServeFood(food);
        }
    }
}
