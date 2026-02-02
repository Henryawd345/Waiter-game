using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class RudeCustomerMovement : MonoBehaviour
{


    [Header("Movement")]
    [SerializeField] private float rudeCustomerSpeed = 4f;
    [SerializeField] private float rudeCustomerAcceleration = 8f;
    [Header("Repath update")]
    [SerializeField] private float updateRate = 0.25f;
    [Header("Detection")]
    [SerializeField] private float detectionRange = 6f;

    [Header("Roaming")]
    [SerializeField] private float minRoamDistance = 2f;
    [SerializeField] private float roamRadius = 6f;
    //[SerializeField] private float minWaitTime = 0.7f;
    //[SerializeField] private float maxWaitTime = 1.8f;
    [SerializeField] private float minNewTargetDistanceFromLast = 1.5f;

    private NavMeshAgent badAgent;
    private RudeCustomer badSelf;

    void Awake()
    {
        badAgent = GetComponent<NavMeshAgent>();
        badSelf = GetComponent<RudeCustomer>();
    }

    void Start()
    {
        badAgent.speed = rudeCustomerSpeed;
        badAgent.acceleration = rudeCustomerAcceleration;
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