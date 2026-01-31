using UnityEngine;
using UnityEngine.AI;

public class GoodCustomerMovement : MonoBehaviour
{
    private GoodCustomer customerSelf;
    private NavMeshAgent navAgent;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        customerSelf = GetComponent<GoodCustomer>();
    }
    public void MoveTo(Vector3 position)
    {
        navAgent.SetDestination(position);
    }
    public void MoveToExit()
    {
        navAgent.SetDestination(new Vector3(0,0,0)); // gonna be exit location
    }
    public bool HasArrived()
    {
        if (navAgent.pathPending)
            return false;

        return navAgent.remainingDistance <= navAgent.stoppingDistance
            && navAgent.velocity.sqrMagnitude < 0.01f;
    }
    public void Warp(Vector3 pos)
    {
        navAgent.Warp(pos);
    }
}