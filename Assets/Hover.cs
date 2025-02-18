using UnityEngine;

public class Hover : MonoBehaviour
{
    public Vector3 targetPosition = new Vector3(7.0f, 7.0f, 0.0f); // The position to float to
    // public float floatSpeed = 1.0f; // The speed of floating
    public float floatDuration = 5.0f; // The duration of floating in seconds

    private Vector3 initialPosition;
    private float floatTimer;

    private void Start()
    {
        initialPosition = transform.position;
        floatTimer = 0.0f;
    }

    private void Update()
    {
        // Increment the timer
        floatTimer += Time.deltaTime;

        // Calculate the new position with floating effect
        Vector3 newPosition = Vector3.Lerp(initialPosition, targetPosition, floatTimer / floatDuration);

        // Apply the new position
        transform.position = newPosition;

        // Check if floating duration is reached
        if (floatTimer >= floatDuration)
        {
            // Reset the timer and stop floating
            floatTimer = 0.0f;
            transform.position = targetPosition;
        }
    }
}
