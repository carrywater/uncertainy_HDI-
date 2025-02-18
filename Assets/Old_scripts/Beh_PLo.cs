using UnityEngine;

public class Beh_PLo : MonoBehaviour

{
    private Vector3 startPoint = new Vector3(207f, 207f, 0f);
    private Vector3 centerPoint = new Vector3(7f, 207f, 0f);
    private Vector3 endPoint = new Vector3(7f, 7f, 0f);
    public float moveSpeed = 11.1f; // Linear movement speed

    private float radius;
    private float rotationSpeed;
    private float angle = 0f;
    private float maxAngle = Mathf.PI / 2f; // End at 90 degrees (quarter rotation)

    void Start()
    {
        radius = Vector3.Distance(centerPoint, startPoint);
        Debug.Log("Radius: " + radius);
    }

    void Update()
    {
        if (angle < maxAngle)
        {
            rotationSpeed = (moveSpeed * 360) / (2 * radius * Mathf.PI); 
            angle += rotationSpeed * Time.deltaTime * Mathf.Deg2Rad;
            Debug.Log("Angle: " + angle);

            float x =  Mathf.Cos(angle) * radius + centerPoint.x; 
            float y =  - Mathf.Sin(angle) * radius + centerPoint.y; 

            Vector3 newPosition = new Vector3(x, y, transform.position.z);
            transform.position = newPosition;
            Debug.Log("New Position: " + newPosition);

            if (angle >= maxAngle)
            {
                enabled = false;
                Debug.Log("Movement Completed.");
            }
        }
    }
}

