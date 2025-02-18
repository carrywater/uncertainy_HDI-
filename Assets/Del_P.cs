using UnityEngine;

public class Del_P : MonoBehaviour
{
    float mass = 1.0f; // Assume a mass of 1 kg for demonstration purposes
    float gravity = 9.81f; // Acceleration due to gravity in m/s²
    float distance = 7.0f; // Vertical distance in meters
    float freeFallTime = 0.5f; // Free fall time in seconds
    float totalTime = 4.7f; // Total time in seconds (takes 4.98seconds to settle in scene)

    private void Update()
    {
        float currentTime = Time.time - freeFallTime;
        Debug.Log("Time: " + Time.time);
        Debug.Log("Position: " + transform.position);
        if (currentTime > 0f)
        {
            float d = (2 * mass * gravity * distance) / ((totalTime - freeFallTime) * (totalTime - freeFallTime));

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.drag = d;
        }
    }
}


