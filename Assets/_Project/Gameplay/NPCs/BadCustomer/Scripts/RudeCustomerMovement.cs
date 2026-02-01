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
    Transform playerTransform;
    private float targetUpdateTime;
    //private float nextRoamActionTime;
    private Vector3 currentRoamTarget;
    private Vector3 lastRoamTarget;

    void Awake()
    {
        badAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        playerTransform = PlayerLocator.PlayerTransform;

        badAgent.speed = rudeCustomerSpeed;
        badAgent.acceleration = rudeCustomerAcceleration;
        
        PickNewRoamTarget();
    }

    void Update()
    {

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRange) // if distance from player is smaller than bad cus range start chasing
        {
            ChaseUpdate();
            return;
        }

        // Player far → Roam
        RoamUpdate();
            
    }

    private void ChaseUpdate()
    {
        badAgent.isStopped = false;


        if (Time.time < targetUpdateTime) // Wait until the next allowed update time
        {
            return;
        }

        targetUpdateTime = Time.time + updateRate;

        badAgent.SetDestination(playerTransform.position);
    }

    private void RoamUpdate()
    {
            badAgent.isStopped = false;

        // If we don't currently have a path (or path became invalid), pick a new target
        if (!badAgent.hasPath || badAgent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            PickNewRoamTarget();
            badAgent.SetDestination(currentRoamTarget);
            return;
        }

        // If we reached the destination, pick a new one immediately
        bool reached =
            !badAgent.pathPending &&
            badAgent.remainingDistance <= badAgent.stoppingDistance &&
            (badAgent.velocity.sqrMagnitude < 0.01f);

        if (reached)
        {
            PickNewRoamTarget();
            badAgent.SetDestination(currentRoamTarget);
            return;
        }
    }

    private void PickNewRoamTarget()
    {
        for (int i = 0; i < 15; i++) // try multiple times to find a good point
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * roamRadius;
        randomPoint.y = transform.position.y;

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, roamRadius, NavMesh.AllAreas))
        {
            // 1) Don't pick a target too close to the NPC
            if (Vector3.Distance(transform.position, hit.position) < minRoamDistance)
                continue;

            // 2) Don't pick a target too close to the last roam target
            if (lastRoamTarget != Vector3.zero &&
                Vector3.Distance(hit.position, lastRoamTarget) < minNewTargetDistanceFromLast)
                continue;

            // Accept target
            currentRoamTarget = hit.position;
            lastRoamTarget = currentRoamTarget;
            return;
        }
    }

    // Fallback (rare): stay where you are
    currentRoamTarget = transform.position;

    }

}