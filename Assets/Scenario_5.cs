//working code for scenario 5: low-straight_cable
using UnityEngine;

public class Scenario_5 : MonoBehaviour
{
    public Transform Package;
    public Transform Pseudo;
    public AnimationCurve xTrajectoryCurve; // (207, 207, 7)
    public AnimationCurve yTrajectoryCurve; // (207, 7, 7)
    public AnimationCurve xMoveToFinalCurve; // (7, 7, 207)
    public AnimationCurve yMoveToFinalCurve; // (7, 207, 207)
    public float moveSpeed = 0.02775f; // Adjust this value for speed
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
    private float normalizedTime = 0f;
    private float normalizedTimee = 0f;
    private bool landing = false;
    private float hoverTimer;
    private float landMoveStartTime;


    private void Start()
    {
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
        normalizedTime += Time.deltaTime * moveSpeed;

        float xOffset = xTrajectoryCurve.Evaluate(normalizedTime);
        float yOffset = yTrajectoryCurve.Evaluate(normalizedTime);

        Vector3 newPosition = new Vector3(xOffset, yOffset, 0);
        transform.position = newPosition;

        if (normalizedTime >= 1.0f)
        {
            currentState = DroneState.Hover;
            normalizedTime = 0.0f;
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
        normalizedTimee += Time.deltaTime * moveSpeed; // You can use the same duration for consistency

        float xOffset = xMoveToFinalCurve.Evaluate(normalizedTimee);
        float yOffset = yMoveToFinalCurve.Evaluate(normalizedTimee);

        Vector3 newPosition = new Vector3(xOffset, yOffset, 0);

        transform.position = newPosition;

    }
}
