using UnityEngine;

public class PlayerWalk : MonoBehaviour
{
    private float speed = 20f;
    private CharacterController characterController;
    private float verticalVelocity = 0f;

    void Awake()
    {
        characterController = this.GetComponent<CharacterController>();
    }

    public void Move(Vector2 directionInput, bool isRunning)
{
    Vector3 horizontal =
        transform.right * directionInput.x +
        transform.forward * directionInput.y;

    float finalSpeed = speed * (isRunning ? 1.75f : 1f);

    horizontal *= finalSpeed;

    if (characterController.isGrounded && verticalVelocity < 0f)
        verticalVelocity = -2f;

    verticalVelocity -= 9.81f * Time.deltaTime;

    Vector3 velocity = horizontal + (Vector3.up * verticalVelocity);

    characterController.Move(velocity * Time.deltaTime);
}

}
