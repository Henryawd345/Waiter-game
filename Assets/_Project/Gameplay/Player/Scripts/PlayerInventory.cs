using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    List<FoodItem> foods;
    PlayerStatsHolder playerStatsHolder;
    PlayerInventory playerInventory;
    private Transform playerCameraTransform;
    private int selectedIndex;
    void Awake()
    {
        playerStatsHolder = GetComponent<PlayerStatsHolder>();
        playerCameraTransform = GetComponentInChildren<Camera>().transform;
        foods = new List<FoodItem>();

        selectedIndex = 0;
    }

    // Update is called once per frame
    public bool PickUpFood(FoodItem foodInput)
    {
        if (foods.Count > playerStatsHolder.trayCapacity - 1 || foodInput.isBeingHeld == true)
            return false;
        
        foods.Add(foodInput);
        foodInput.PickUp();
        foodInput.gameObject.SetActive(false);
        SwapFood(true);

        return true;
    }
    public void SwapFood(bool swapDirection)
    {
        if (foods.Count == 0) return;

        FoodItem prevFood = foods[selectedIndex];
        selectedIndex += swapDirection ? 1 : -1;
        selectedIndex = (selectedIndex + foods.Count) % foods.Count;

        if (foods.Count > 1 && prevFood == foods[selectedIndex]) return;
        prevFood?.gameObject.SetActive(false);

        ShowSelectedFood();
    }
    public void ShowSelectedFood()
    {
        if (foods.Count == 0) return;

        FoodItem currentFood = foods[selectedIndex];
        currentFood.gameObject.SetActive(true);
        currentFood.transform.SetParent(playerCameraTransform);
        currentFood.transform.rotation = playerCameraTransform.rotation;
        currentFood.transform.position = playerCameraTransform.position 
            + (playerCameraTransform.forward * 1f) 
            + (playerCameraTransform.right * 0.75f) 
            + (playerCameraTransform.up * -0.45f);
    }
    public void ThrowFood(Transform transformInput) // smaller overload
    {
        ThrowFood(transformInput.forward, transformInput);
    }
    public void ThrowFood(Vector3 adjDirection, Transform transformInput) // bigger overload
    {
        if (foods.Count == 0) return;

        if (foods[selectedIndex] != null)
        {
            foods[selectedIndex].OnThrow(adjDirection);
            foods.RemoveAt(selectedIndex);
            selectedIndex = Mathf.Max(0, selectedIndex - 1);
        }
        SwapFood(true);
    }
    public void ThrowAllFood(Transform transformInput)
    {
        while (foods.Count > 0)
        {
            Vector3 randomDirection = transformInput.forward + new Vector3(
                UnityEngine.Random.Range(-0.1f, 0.1f),
                UnityEngine.Random.Range(-0.1f, 0.1f),
                UnityEngine.Random.Range(-0.1f, 0.1f)
            );
            ThrowFood(randomDirection, transformInput);
        }
    }
    void Update()
    {
        
    }
}
