//Working code for scenario 7: high-curved_land
using UnityEngine;

public class Scenario_7 : MonoBehaviour
{
    public Transform Package;
    private Vector3 startPoint = new Vector3(207f, 207f, 0f);
    private Vector3 centerPoint = new Vector3(207f, 7f, 0f);
    private Vector3 endPoint = new Vector3(7f, 7f, 0f);
    public AnimationCurve xMoveToFinalCurve; // (7, 7, 207)
    public AnimationCurve yMoveToFinalCurve; // (7, 207, 207)
    public float moveSpeed = 11.1f; // Adjust this value for speed
    public Vector3 targetHoverPosition = new Vector3(7f, 7f, 0f); // Hover position
    public Vector3 targetLandPosition = new Vector3(7f, 0f, 0f); // Land position
    //public Vector3 finalPosition = new Vector3(207f, 207f, 0f); // Final position

    public float hoverDuration = 5.0f; // The duration of hovering in seconds
    public float landMoveDuration = 5.0f; // The duration of landing movement

    private enum DroneState { Approach, Hover, Land, Return, MoveToFinal }
    private DroneState currentState = DroneState.Approach;

    private float normalizedTimee = 0f;
    private bool landing = false;
    private float hoverTimer;
    private float landMoveStartTime;
    //private Animator animator;

    private float radius;
    private float rotationSpeed;
    private float angle = 0f;
    private float maxAngle = Mathf.PI / 2f; // End at 90 degrees (quarter rotation)

    private void Start()
    {
        radius = Vector3.Distance(centerPoint, startPoint);
        transform.position = new Vector3(207f, 207f, 0f); // Initial position
    }

    private void Update()
    {
        switch (currentState)
        {
            case DroneState.Approach:
                Approach();
                break;
            case DroneState.Hover:
                Hover();
                break;
            case DroneState.Land:
                Land();
                break;
            case DroneState.Return:
                Return();
                break;
            case DroneState.MoveToFinal:
                MoveToFinal();
                break;
        }
    }

    private void Approach()
    {
        if (angle < maxAngle)
        {
            rotationSpeed = (moveSpeed * 360) / (2 * radius * Mathf.PI);
            angle += rotationSpeed * Time.deltaTime * Mathf.Deg2Rad;

            float x = -Mathf.Sin(angle) * radius + centerPoint.x;
            float y = Mathf.Cos(angle) * radius + centerPoint.y;

            Vector3 newPosition = new Vector3(x, y, transform.position.z);
            transform.position = newPosition;

            if (angle >= maxAngle)
            {
                currentState = DroneState.Hover;
            }
        }
    }

    private void Hover()
    {
        hoverTimer += Time.deltaTime;

        Vector3 newPosition = Vector3.Lerp(targetHoverPosition, targetHoverPosition, hoverTimer / hoverDuration);
        transform.position = newPosition;

        if (hoverTimer >= hoverDuration)
        {
            currentState = DroneState.Land;
            hoverTimer = 0.0f;
            landMoveStartTime = Time.time;
        }
    }

    private void Land()
    {
        float elapsedTime = Time.time - landMoveStartTime;
        float t = Mathf.Clamp01(elapsedTime / landMoveDuration);
        transform.position = Vector3.Lerp(targetHoverPosition, targetLandPosition, t);

        if (t >= 1.0f)
        {
            landing = true;
            currentState = DroneState.Return;
            //animator.enabled = false; // Disable the Animator component
            t = 0.0f;
            landMoveStartTime = Time.time;
            Package.SetParent(null);
        }
    }


    private void Return()
    {
        float elapsedTime = Time.time - landMoveStartTime;
        float t = Mathf.Clamp01(elapsedTime / landMoveDuration);

        Vector3 newPosition = Vector3.Lerp(targetLandPosition, targetHoverPosition, t);

        transform.position = newPosition;

        if (t >= 1.0f)
        {
            currentState = DroneState.MoveToFinal;
        }
    }



    private void MoveToFinal()/////need changes
    {
        normalizedTimee += Time.deltaTime * 0.02775f; // You can use the same duration for consistency

        float xOffset = xMoveToFinalCurve.Evaluate(normalizedTimee);
        float yOffset = yMoveToFinalCurve.Evaluate(normalizedTimee);

        Vector3 newPosition = new Vector3(xOffset, yOffset, 0);

        transform.position = newPosition;

    }
}
