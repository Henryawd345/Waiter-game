using UnityEngine;
using UnityEngine.AI;

public class RudeCustomerMovement : MonoBehaviour
{
    [SerializeField] private float rudeCustomerSpeed = 100f;
    [SerializeField] private float rudeCustomerAcceleration = 100f;
    private NavMeshAgent agent;
    Transform playerTransform;

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
        // Vector3 target = playerTransform.position;
        // agent.SetDestination(target);
    }
}