using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class RudeCustomerMovement : MonoBehaviour
{


    [Header("Movement")]
    [SerializeField] private float rudeCustomerSpeed = 4f;
    [SerializeField] private float rudeCustomerAcceleration = 8f;

    private NavMeshAgent badAgent;

    void Awake()
    {
        badAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        badAgent.speed = rudeCustomerSpeed;
        badAgent.acceleration = rudeCustomerAcceleration;

        badAgent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    }

    public void MoveTo(Vector3 position)
    {
        badAgent.SetDestination(position);
    }

    public bool HasArrived()
    {
        if (badAgent.pathPending)
            return false;

        return badAgent.remainingDistance <= badAgent.stoppingDistance
            && badAgent.velocity.sqrMagnitude < 0.01f;
    }
    public void Warp(Vector3 pos)
    {
        badAgent.ResetPath();
        badAgent.Warp(pos);
    }

    public void Stop()
    {
        badAgent.ResetPath();
    }

}