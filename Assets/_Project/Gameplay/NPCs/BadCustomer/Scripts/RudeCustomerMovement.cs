using UnityEngine;
using UnityEngine.AI;

public class RudeCustomerMovement : MonoBehaviour
{
    [SerializeField] private float rudeCustomerSpeed = 4f;
    [SerializeField] private float rudeCustomerAcceleration = 8f;
    [SerializeField] private float updateRate = 0.25f;
    [SerializeField] private float detectionRange = 6f;
    private NavMeshAgent agent;
    Transform playerTransform;
    private float targetUpdateTime;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        playerTransform = PlayerLocator.PlayerTransform;

        agent.speed = rudeCustomerSpeed;
        agent.acceleration = rudeCustomerAcceleration;
    }

    void Update()
    {

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > detectionRange) // if distance from player is larger than bad cus range stopped
        {
            agent.isStopped = true;
            return;
        }
            agent.isStopped = false;


        if (Time.time < targetUpdateTime) // Wait until the next allowed update time
        {
            return;
        }

        targetUpdateTime = Time.time + updateRate;

        agent.SetDestination(playerTransform.position);
    }
}