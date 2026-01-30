using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodCustomer : MonoBehaviour
{
    // setable variables
    private double initWaitTimer = 30; // how long it take till customer angry (in seconds)

    public Table ownedTable = null;
    private double waitTimer;
    private GoodCustomerMovement movementScript;
    private GoodCustomerStates customerState = GoodCustomerStates.Waiting;
    public Order currentOrder;
    [SerializeField] private GameObject orderPrefab;

    void Update()
    {
        if (customerState == GoodCustomerStates.WalkingToTable && movementScript.HasArrived())
            sitAndOrder();

        if (customerState == GoodCustomerStates.Seated)
            waitTimer -= 1 * Time.deltaTime;
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

    private void sitAndOrder()
    {
        if (ownedTable == null)
            return;

        customerState = GoodCustomerStates.Seated;
        ownedTable.Sit(this);

        waitTimer = initWaitTimer;
    }
}
