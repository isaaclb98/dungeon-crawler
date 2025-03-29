using UnityEngine;

public class CompassScript : MonoBehaviour
{
    public GameObject compassNeedle;  // Reference to the compass needle image
    public Transform player;          // Reference to the player (FirstPersonController)
    public float rotationSpeed = 5f;  // Smooth rotation speed for the needle

    void Update()
    {
        RotateCompass();
    }

    void RotateCompass()
    {
        // Get the player's forward direction
        Vector3 playerForwardDirection = player.forward;

        // Calculate the angle the needle needs to rotate (convert to degrees)
        float angle = Mathf.Atan2(playerForwardDirection.x, playerForwardDirection.z) * Mathf.Rad2Deg;

        // Rotate the compass needle to match the player's facing direction
        // We use Quaternion.Euler to set the rotation in the Z axis (2D rotation)
        float smoothAngle = Mathf.LerpAngle(compassNeedle.transform.eulerAngles.z, -angle, Time.deltaTime * rotationSpeed);

        // Apply the rotation to the needle
        compassNeedle.transform.rotation = Quaternion.Euler(0, 0, smoothAngle);
    }
}
