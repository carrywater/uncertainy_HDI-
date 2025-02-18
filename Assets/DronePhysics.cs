using UnityEngine;

public class DronePhysics : MonoBehaviour
{
    // Speed and rotation factors
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 2.0f;
    public float sensitivity = 0.5f; // Adjust this sensitivity factor

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // Get the rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Drone control inputs
        float verticalInput = Input.GetAxis("Vertical") * sensitivity; // Throttle
        float horizontalInput = Input.GetAxis("Horizontal") * sensitivity; // Pitch and roll
        float yawInput = Input.GetAxis("Yaw") * sensitivity; // Yaw

        // Calculate forces based on inputs
        Vector3 movement = transform.forward * verticalInput * moveSpeed;
        Vector3 rotation = new Vector3(0, yawInput * rotationSpeed, 0);
        Vector3 pitchRoll = new Vector3(horizontalInput * rotationSpeed, 0, 0);

        // Apply forces and rotations to the rigid body
        rb.AddForce(movement);
        rb.AddTorque(rotation);
        rb.AddRelativeTorque(pitchRoll);

        // Clamp the drone's velocity to prevent excessive speed
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, moveSpeed);

        // Clamp angular velocity to prevent excessive spinning
        rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, rotationSpeed);
    }
}
