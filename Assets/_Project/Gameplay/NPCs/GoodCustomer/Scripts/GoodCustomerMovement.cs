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

        navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    }

    public void MoveTo(Vector3 position)
    {
        navAgent.SetDestination(position);
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
        navAgent.ResetPath();
        navAgent.Warp(pos);
    }
}
