using UnityEngine;

public class FoodItem : MonoBehaviour
{
    private new Rigidbody rigidbody;
    public FoodTypes foodType;
    public bool isPickedUp;
    private Transform glowOutlineTransform;
    void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        glowOutlineTransform = transform.Find("Visual/FoodVisualOutline");
        isPickedUp = false;
    }
    public void PickUp()
    {
        rigidbody.isKinematic = true;
        isPickedUp = true;
    }
    public void Init(FoodTypes foodType)
    {
        LoadTexture();
        this.foodType = foodType;
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
    }
    public void Glow() {glowOutlineTransform?.gameObject.SetActive(true);}
    public void StopGlow() {glowOutlineTransform?.gameObject.SetActive(false);}
}