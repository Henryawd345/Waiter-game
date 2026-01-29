using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private Transform playerCameraTransform;
    private Transform cameraHolderTransform;
    private Transform playerTransform;
    private float pitch = 0;
    private float bobTime = 0;
    private float currentBobOffset = 0;
    void Awake()
    {
        playerCameraTransform = GetComponentInChildren<Camera>().transform;
        cameraHolderTransform = transform.Find("CameraHolder");
        playerTransform = this.transform;
    }

    public void MoveHead(Vector2 lookDelta)
    {
        float yawDelta   = lookDelta.x;
        float pitchDelta = lookDelta.y;

        pitch -= pitchDelta;
        pitch = Mathf.Clamp(pitch, -75f, 75f);

        playerCameraTransform.localRotation =
            Quaternion.Euler(pitch, 0f, 0f);

        playerTransform.Rotate(Vector3.up * yawDelta);
    }
    public void HeadBobbing(bool isMoving, bool isRunning)
    {
        float bobFrequency = isRunning ? 20f : 15f;
        float bobReturnSpeed = 20f;
        float bobAmplitude = isRunning ? 0.25f : 0.15f;

        if (isMoving)
        {
            bobTime += Time.deltaTime * bobFrequency;
            currentBobOffset = Mathf.Sin(bobTime) * bobAmplitude;
        }
        else
        {
            bobTime = 0;
            currentBobOffset = Mathf.Lerp( currentBobOffset, 0f, Time.deltaTime * bobReturnSpeed);
        }

        playerCameraTransform.localPosition = Vector3.up * currentBobOffset;
    }

}
