using System.Collections.Generic;
using UnityEngine;
public class Order : MonoBehaviour
{
    private List<FoodTypes> wantedFood;
    private HashSet<FoodTypes> servedFood = new HashSet<FoodTypes>();

    public void Init(List<FoodTypes> foods)
    {
        wantedFood = new List<FoodTypes>(foods);
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
            OnOrderCompleted();
        }
    }

    private void RejectFood(FoodItem food, bool reason)
    {
        Destroy(food.gameObject);
        // reason true = duped food, false = unmatched food
        if (reason == true) // duped food
        {
            
        }
        else // unmatched food
        {
            
        }
    }

    private bool IsOrderComplete()
    {
        return servedFood.Count == wantedFood.Count;
    }

    private void OnOrderCompleted()
    {
        Debug.Log("Order complete!");
        // Notify table / customer
    }
}
