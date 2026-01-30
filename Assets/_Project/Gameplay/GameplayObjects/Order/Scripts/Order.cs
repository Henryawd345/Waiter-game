using System.Collections.Generic;
using UnityEngine;
public class Order : MonoBehaviour
{
    // variables
    [SerializeField] int MaximumFoodPerCustomer = 4;

    private List<FoodTypes> wantedFood;
    private HashSet<FoodTypes> servedFood = new HashSet<FoodTypes>();
    private GoodCustomer customerOrderOwner;

    public void Init(GoodCustomer customer)
    {
        customerOrderOwner = customer;
        GenerateRandomOrder();
    }
    void GenerateRandomOrder()
    {
        wantedFood = new List<FoodTypes>();

        FoodTypes[] allFoods = (FoodTypes[])System.Enum.GetValues(typeof(FoodTypes));

        int count = Random.Range(1, MaximumFoodPerCustomer + 1);

        List<FoodTypes> pool = new List<FoodTypes>(allFoods);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, pool.Count);
            wantedFood.Add(pool[index]);
            pool.RemoveAt(index);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        FoodItem food = other.GetComponent<FoodItem>();
        if (food == null) return;

        TryServeFood(food);
    }

    private void TryServeFood(FoodItem food)
    {
        if (servedFood.Contains(food.FoodType))
        {
            RejectFood(food, true);
            return;
        }

        if (wantedFood.Contains(food.FoodType))
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
        servedFood.Add(food.FoodType);
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
}
