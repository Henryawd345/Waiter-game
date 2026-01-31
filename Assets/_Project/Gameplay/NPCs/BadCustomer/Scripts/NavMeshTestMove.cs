using UnityEngine;
using UnityEngine.AI;

public class NavMeshTestMove : MonoBehaviour
{
    private NavMeshAgent agent;

    Transform playerTransform;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (agent == null)
        {
            Debug.LogError("No NavMeshAgent found on this object.");
            return;
        }

        // Test target point (change this if you want)
        playerTransform = PlayerLocator.PlayerTransform;

            }

    void Update()
    {
        Vector3 target = playerTransform.position;

        agent.SetDestination(target);
        Debug.Log("PLayer location" + transform.position.x);
    }
}


