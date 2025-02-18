using UnityEngine;

public class Beh_PUp_Luca : MonoBehaviour
{
    public Vector3 startPoint = new Vector3(207f, 207f, 0f);
    public Vector3 endPoint = new Vector3(7f, 7f, 0f);
    public float moveSpeed = 11.1f; // Linear movement speed

    private Vector3 centerPoint;
    private float radius;
    private float rotationSpeed;
    private float angle = 0f;
    private float maxAngle = Mathf.PI / 2f; // End at 90 degrees (half rotation)

    void Start()
    {
        centerPoint = (startPoint + endPoint) / 2f; // Calculate the center point of the arc
        radius = Vector3.Distance(centerPoint, startPoint); //// Calculate the radius of symmetric arc path
        Debug.Log("Center Point: " + centerPoint);
        Debug.Log("Start position: " + transform.position);
        Debug.Log("Radius: " + radius);
    }

    void Update()
    {
        if (angle < maxAngle)
        {
            rotationSpeed = (moveSpeed * 360) / (2 * radius * Mathf.PI); 
            angle += rotationSpeed * Time.deltaTime * Mathf.Deg2Rad;// Update the angle based on time and movement speed
            Debug.Log("Angle: " + angle);

            // Calculate x and y positions symmetrically around the center point and based on the angle
            float x = Mathf.Sin(angle) * radius + centerPoint.x;
            float y = Mathf.Cos(angle) * radius + centerPoint.y;

            // Set the new position of the GameObject
            Vector3 newPosition = new Vector3(x, y, transform.position.z);
            transform.position = newPosition;
            Debug.Log("New Position: " + newPosition);

            // Check if the movement is completed
            if (angle >= maxAngle)
            {
                enabled = false;
                Debug.Log("Movement Completed.");
            }
        }
    }
}