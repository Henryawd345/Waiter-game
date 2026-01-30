using UnityEngine;
public class PlayerLocator : MonoBehaviour
{
    public static Transform PlayerTransform { get; private set; }

    public void register(Transform playerTransform)
    {
        PlayerTransform = playerTransform;
    }
    public void unregister(Transform playerTransform)
    {
        if (playerTransform.Equals(PlayerTransform))
            PlayerTransform = null;
    }
}