using UnityEngine;
public class PlayerLocator : MonoBehaviour
{
    public static Transform PlayerTransform { get; private set; }

    public static void register(Transform playerTransform)
    {
        PlayerTransform = playerTransform;
    }
    public static void unregister(Transform playerTransform)
    {
        if (playerTransform.Equals(PlayerTransform))
            PlayerTransform = null;
    }
    void Update()
    {
        Debug.Log(PlayerTransform.position.x + " " + PlayerTransform.position.x + " " + PlayerTransform.position.z);
    }
}