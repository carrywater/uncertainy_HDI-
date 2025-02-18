using UnityEngine;

public class Beh_PUp : MonoBehaviour

{
    private Vector3 startPoint = new Vector3(207f, 207f, 0f);
    private Vector3 centerPoint = new Vector3(207f, 7f, 0f);
    private Vector3 endPoint = new Vector3(7f, 7f, 0f);
    public float moveSpeed = 11.1f; // Linear movement speed, check!!!

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
        if (angle < maxAngle) //check!!!
        {
            rotationSpeed = (moveSpeed * 360) / (2 * radius * Mathf.PI); //correct
            angle += rotationSpeed * Time.deltaTime * Mathf.Deg2Rad; //check Rad2dDeg!!!
            Debug.Log("Angle: " + angle);

            float x = - Mathf.Sin(angle) * radius + centerPoint.x; //check Cos!!!
            float y =   Mathf.Cos(angle) * radius + centerPoint.y; //checkSin!!!

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


//version 2
//{
//    public Vector3 startPoint = new Vector3(207f, 207f, 0f);
//    public Vector3 endPoint = new Vector3(7f, 7f, 0f);
//    public float moveSpeed = 11.1f; // Linear movement speed, check!!!

//    private Vector3 centerPoint;
//    private float radius;
//    private float rotationSpeed;
//    private float angle = 0f;
//    private float maxAngle = Mathf.PI / 2f; // End at 90 degrees (quarter rotation)

//    void Start()
//    {
//        float centerY = (startPoint.y + endPoint.y) / 2f; // Calculate the adjusted Y-coordinate for the center
//        float centerX = (startPoint.x + endPoint.x) / 2f; // Calculate the adjusted X-coordinate for the centerpoint
//        centerPoint = new Vector3(centerX, centerY, 0f); // Use the adjusted X- and Y-coordinate
//        Debug.Log("Center point: " + centerPoint);
//        radius = Vector3.Distance(centerPoint, startPoint);
//        Debug.Log("Radius: " + radius);
//    }

//    void Update()
//    {
//        if (angle < maxAngle)
//        {
//            rotationSpeed = (moveSpeed * 360) / (2 * radius * Mathf.PI); //correct
//            angle += rotationSpeed * Time.deltaTime * Mathf.Deg2Rad;
//            Debug.Log("Angle: " + angle);

//            float x = Mathf.Cos(angle) * radius + centerPoint.x;
//            float y = Mathf.Sin(angle) * radius + centerPoint.y;

//            Vector3 newPosition = new Vector3(x, y, transform.position.z);
//            transform.position = newPosition;
//            Debug.Log("New Position: " + newPosition);

//            if (angle >= maxAngle)
//            {
//                enabled = false;
//                Debug.Log("Movement Completed.");
//            }
//        }
//    }
//}



// older version

//public class Beh_PUp : MonoBehaviour
//{
//    public float radius = 100f; // Radius of the semisphere
//    public float moveSpeed = 11.1f; // Linear movement speed
//    public Vector3 targetCoordinates = new Vector3(7f, 7f, 0f);

//    private float angle = 0f;
//    private float maxAngle = Mathf.PI/2f; // Half of a full rotation (180 degrees). It needs to come down to 90 degree rotation

//    void Update()
//    {
//        if (angle < maxAngle)
//        {
//            angle += moveSpeed * Time.deltaTime * Mathf.Deg2Rad;

//            float x = Mathf.Cos(angle) * radius;
//            float y = Mathf.Sin(angle) * radius;

//            Vector3 newPosition = new Vector3(x, y, transform.position.z);
//            transform.position = newPosition;

//            if (Vector3.Distance(transform.position, targetCoordinates) < 0.01f)
//            {
//                enabled = false;
//            }
//        }
//    }
//}
