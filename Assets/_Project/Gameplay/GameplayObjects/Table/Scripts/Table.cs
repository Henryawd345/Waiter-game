using UnityEngine;
using UnityEngine.UIElements;

public class Table : MonoBehaviour, RudeCustomerTarget
{
    public TableStates occupationState {get; private set;}
    public GoodCustomer occupier {get; private set;}
    //Position of chair and access point
    public Transform accessPointTransform {get; private set;}
    public Transform chairTransform {get; private set;}
    public Transform orderAnchorPos {get; private set;}
    private Transform playerTransform;
    void Awake()
    {
        accessPointTransform = transform.Find("PosHolders/AccessPointPos");
        chairTransform = transform.Find("PosHolders/ChairPos");
        orderAnchorPos = transform.Find("PosHolders/OrderAnchorPos");

        occupationState = TableStates.Free;
        occupier = null;
    }
    void Start()
    {
        playerTransform = PlayerLocator.PlayerTransform;
    }
    void Update()
    {
        if (playerTransform != null)
        {
            // make orderAnchor always spin face the player
            Vector3 playerPos = playerTransform.position;
            Vector3 anchorPos = orderAnchorPos.position;

            playerPos.y = anchorPos.y;

            Vector3 direction = playerPos - anchorPos;

            if (direction.sqrMagnitude > 0.001f)
                orderAnchorPos.rotation = Quaternion.LookRotation(direction);
        }
    }

    public bool requestTable(GoodCustomer customer)
    {
        if (occupationState == TableStates.Free)
        {
            occupationState = TableStates.Reserved;
            occupier = customer;
            return true;
        }
        return false;
    }
    public void Sit(GoodCustomer customer)
    {
        if (customer.Equals(this.occupier))
            occupationState = TableStates.Taken;
    }
    public void StandUp(GoodCustomer customer)
    {
        if (occupier.Equals(customer))
        {
            occupationState = TableStates.Free;
            occupier = null;
        }
    }
    public void attachOrderToTable(Order order)
    {
        order.transform.parent = orderAnchorPos;
        order.transform.localPosition = Vector3.zero;
        order.transform.localRotation = Quaternion.identity;
    }
}
