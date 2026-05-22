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

    public void Knockback(Vector3 direction, float distance)
    {
        Vector3 target = transform.position + direction.normalized * distance;

        // stop at navmesh edges (walls/obstacles) so they don't get knocked through them
        if (badAgent.isOnNavMesh && badAgent.Raycast(target, out NavMeshHit hit))
            target = hit.position;

        badAgent.Warp(target);
    }

    public void SetAgentEnabled(bool value)
    {
        badAgent.enabled = value; // disable while carried so it stops fighting the parenting
    }

    public bool WarpToNearestNavMesh(Vector3 position, float maxDistance = 5f)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
        {
            badAgent.Warp(hit.position);
            return true;
        }
        return false;
    }

}