using UnityEngine;

public class FoodItem : MonoBehaviour
{
    private new Rigidbody rigidbody;
    public FoodTypes foodType;
    public bool isFresh;
    public bool isBeingHeld;
    private Transform glowOutlineTransform;
    private FoodCounter boundFoodCounter;
    void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        glowOutlineTransform = transform.Find("Visual/FoodVisualOutline");

        transform.SetParent(GameObject.Find("_GameplayObjects/Foods").transform);
    }
    public void PickUp()
    {
        if (isFresh == true)
        {
            if (boundFoodCounter != null)
                boundFoodCounter.FoodIsPickedUp(this);
        }
        boundFoodCounter = null;
        rigidbody.isKinematic = true;
        isFresh = false;
        isBeingHeld = true;
    }
    public void Init(FoodCounter foodCounter ,FoodTypes foodType)
    {
        LoadTexture();
        this.foodType = foodType;
        boundFoodCounter = foodCounter;
        isFresh = true;
        isBeingHeld = false;
        // rigidbody.isKinematic = false;
    }
    private void LoadTexture()
    {
        // will load texture later
    }
    public void OnThrow(Vector3 direction)
    {
        transform.SetParent(null);
        rigidbody.isKinematic = false;
        rigidbody.AddForce(direction * 15, ForceMode.Impulse);
        isBeingHeld = false;
    }
    public void Glow() {glowOutlineTransform?.gameObject.SetActive(true);}
    public void StopGlow() {glowOutlineTransform?.gameObject.SetActive(false);}
}