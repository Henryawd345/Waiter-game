using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GoodCustomer : MonoBehaviour
{
    // variables
    [SerializeField] private float InitialWaitTime = 30; // how long it take till customer angry (in seconds)
    [SerializeField] private float EatTime = 5; // how long it take till customer angry (in seconds)
    [SerializeField] int BecomeBadCustomerChance = 25; // in percent %

    // Prefabs
    [SerializeField] private Order orderPrefab;

    public Table ownedTable = null;
    public Order currentOrder;
    private float waitTimer;
    private float eatingTime;
    private GoodCustomerMovement movementScript;
    private GoodCustomerStates customerState = GoodCustomerStates.Waiting;
    private GoodCustomerManager boundManager;
    private Transform exitPosition;

    void Awake()
    {
        movementScript = GetComponent<GoodCustomerMovement>();
    }
    void Update()
    {
        if (customerState == GoodCustomerStates.WalkingToTable && movementScript.HasArrived())
            SitAndOrder();
        if ((customerState == GoodCustomerStates.LeaveHapply || customerState == GoodCustomerStates.Angry) && movementScript.HasArrived())
            Destroy(this.gameObject);

        if (customerState == GoodCustomerStates.Seated)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
                LeaveTable(GoodCustomerLeaveReason.Angry);
        }
        if (customerState == GoodCustomerStates.GotFood)
        {
            eatingTime -= Time.deltaTime;
            if (eatingTime <= 0)
                LeaveTable(GoodCustomerLeaveReason.Happy);
        }

    }

    public void FindTable(List<Table> tableList)
    {
        if (ownedTable != null)
            return;

        foreach (var currTable in tableList)
        {
            if (currTable.requestTable(this))
            {
                ownedTable = currTable;
                movementScript.MoveTo(ownedTable.accessPointTransform.position);
                return;
            }
        }
    }

    private void SitAndOrder()
    {
        if (ownedTable == null)
            return;

        waitTimer = InitialWaitTime;
        customerState = GoodCustomerStates.Seated;
        ownedTable.Sit(this);

        movementScript.Warp(ownedTable.chairTransform.position);
        transform.rotation = ownedTable.chairTransform.rotation;

        currentOrder = Instantiate(orderPrefab);
        currentOrder.Init(this);
        ownedTable.attachOrderToTable(currentOrder);
    }
    public void OnOrderCompleted()
    {
        eatingTime = EatTime;
        customerState = GoodCustomerStates.GotFood;
    }

    private void LeaveTable(GoodCustomerLeaveReason reason)
    {
        // withMood true = good, false = bad
        if (reason == GoodCustomerLeaveReason.Happy)
        {
            DoneEating();
            // increase score and reputation
            // act happy
            customerState = GoodCustomerStates.LeaveHapply;
            movementScript.MoveToExit();
        }
        else if (reason == GoodCustomerLeaveReason.Angry)
        {
            int becomeBadInt;
            becomeBadInt = Random.Range(1,101);
            if (becomeBadInt <= BecomeBadCustomerChance) //become bad customer
            {
                DoneEating();
                // BadCustomer bad = gameObject.AddComponent<BadCustomer>();
                Destroy(this.gameObject);
            }
            else // leave peacefully
            {
                DoneEating();
                // reduce score and reputation
                // act angry
                customerState = GoodCustomerStates.Angry;
                movementScript.MoveToExit();
            }
        }
    }
    private void DoneEating()
    {
        ownedTable.StandUp(this);
        movementScript.Warp(ownedTable.accessPointTransform.position);
        ownedTable = null;
    }

    public void ApplyAnnoyance(float annoyLevel = 1f) // will be called by BadCustomers
    {
        waitTimer -= annoyLevel;
    }

    public void RegisterManager(GoodCustomerManager manager)
    {
        if (boundManager == null && boundManager != manager)
            boundManager = manager;
    }

    public void ResetStateSelf()
    {
        ownedTable = null;
        customerState = GoodCustomerStates.Waiting;
        waitTimer = 0;
        eatingTime = 0;
    }
}
