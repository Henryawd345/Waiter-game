using UnityEngine;
public class PlayerLocator : MonoBehaviour
{
    public static Transform PlayerTransform { get; private set; }

    public static void register(Transform playerTransform)
    {
        PlayerTransform = playerTransform;
        Debug.Log("player registered!");
    }
    public static void unregister(Transform playerTransform)
    {
        if (playerTransform.Equals(PlayerTransform))
            PlayerTransform = null;
    }
}