//working code for scenario 8: high-curved_cable
using UnityEngine;

public class Scenario_8 : MonoBehaviour
{
    public Transform Package;
    public Transform Pseudo;
    private Vector3 startPoint = new Vector3(207f, 207f, 0f);
    private Vector3 centerPoint = new Vector3(207f, 7f, 0f);
    private Vector3 endPoint = new Vector3(7f, 7f, 0f);
    public AnimationCurve xMoveToFinalCurve; // (7, 7, 207)
    public AnimationCurve yMoveToFinalCurve; // (7, 207, 207)
    public float moveSpeed = 11.1f; // Adjust this value for speed
    public Vector3 targetHoverPosition = new Vector3(7f, 7f, 0f); // Hover position
    public Vector3 targetLandPosition = new Vector3(7f, 0f, 0f); // Land position

    public float hoverDuration = 5.0f; // The duration of hovering in seconds
    public float landMoveDuration = 5.0f; // The duration of landing movement

    private enum DroneState { Approach, Hover, Deliver, MoveToFinal }
    private DroneState currentState = DroneState.Approach;

    private bool isRetractingPseudo = false;
    private float pseudoRetractStartTime;

    private Vector3 initialPackagePosition;
    private Vector3 initialPseudoPosition;
    private float normalizedTimee = 0f;
    private bool landing = false;
    private float hoverTimer;
    private float landMoveStartTime;

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
            case DroneState.Deliver:
                Deliver();
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
            currentState = DroneState.Deliver;
            hoverTimer = 0.0f;
            landMoveStartTime = Time.time;
        }
    }

    private void Deliver()
    {
        initialPackagePosition = Package.position; // Use world position instead of localPosition
        initialPseudoPosition = Pseudo.position; // Use world position instead of localPosition
        Package.SetParent(null); // Detach the Package from its parent immediately
        float elapsedTime = Time.time - landMoveStartTime;
        float t = Mathf.Clamp01(elapsedTime / landMoveDuration);
        Package.position = Vector3.Lerp(targetHoverPosition, targetLandPosition, t);
        Pseudo.position = Vector3.Lerp(targetHoverPosition, targetLandPosition, t);

        if (t >= 1.0f)
        {
            if (!isRetractingPseudo) // Check if retraction is not already initiated
            {
                isRetractingPseudo = true;
                pseudoRetractStartTime = Time.time; // Start the timer for pseudo retraction
            }

            float pseudoNormalizedTime = (Time.time - pseudoRetractStartTime) / hoverDuration;
            Vector3 pseudoNewPosition = Vector3.Lerp(targetLandPosition, targetHoverPosition, pseudoNormalizedTime);
            Pseudo.position = pseudoNewPosition;

            if (pseudoNormalizedTime >= 1.0f)
            {
                landing = true;
                currentState = DroneState.MoveToFinal;
                landMoveStartTime = Time.time;
                Package.SetParent(null);
            }
        }
    }

    private void MoveToFinal()
    {
        normalizedTimee += Time.deltaTime * 0.02775f; // You can use the same duration for consistency

        float xOffset = xMoveToFinalCurve.Evaluate(normalizedTimee);
        float yOffset = yMoveToFinalCurve.Evaluate(normalizedTimee);

        Vector3 newPosition = new Vector3(xOffset, yOffset, 0);

        transform.position = newPosition;

    }
}
