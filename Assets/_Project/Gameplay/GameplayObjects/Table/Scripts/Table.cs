using UnityEngine;

public class Table : MonoBehaviour
{
    public TableStates occupationState {get; private set;}
    public GoodCustomer occupier {get; private set;}
    //Position of chair and access point
    public Transform accessPointTransform {get; private set;}
    public Transform chairTransform {get; private set;}
    void Awake()
    {
        accessPointTransform = transform.Find("PosHolders/AccessLocationPos");
        chairTransform = transform.Find("PosHolders/ChairPos");

        occupationState = TableStates.Free;
        occupier = null;
    }
    void Update()
    {
        
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
}
