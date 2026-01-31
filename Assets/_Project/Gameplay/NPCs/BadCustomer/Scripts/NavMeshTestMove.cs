using UnityEngine;
using UnityEngine.AI;

public class NavMeshTestMove : MonoBehaviour
{
    private NavMeshAgent agent;

    Transform playerTransform = PlayerLocator.PlayerTransform;

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

        Vector3 target = Vector3.zero;
        //Vector3 target = playerTransform.position;

        agent.SetDestination(target);
        Debug.Log("SetDestination to " + target);
    }

    void Update()
    {
        Debug.Log("PLayer location" + transform.position.x);
    }
}


