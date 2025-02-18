using UnityEngine;

public class Beh_LLo__Hover__L : MonoBehaviour
{
    // Public variables that can be set in the Unity Inspector
    public AnimationCurve xTrajectoryCurve; // Keyframes for x-axis trajectory
    public AnimationCurve yTrajectoryCurve; // Keyframes for y-axis trajectory
    public float moveSpeed = 0.02775f; // Adjust this value for speed
    public Vector3 targetHoverPosition = new Vector3(7f, 7f, 0f); // Hover position
    public Vector3 targetLandPosition = new Vector3(7f, 0f, 0f); // Land position

    public float hoverDuration = 5.0f; // The duration of hovering in seconds
    public float landMoveDuration = 5.0f; // The duration of landing movement

    // Private variables for internal use
    private float normalizedTime = 0f;
    private bool reachedHoverTarget = false;
    private bool hovering = false;
    private bool landing = false;
    private float hoverTimer;
    private float landMoveStartTime;
    private Animator animator;

    private void Start()
    {
        // Get the Animator component from the same GameObject
        animator = GetComponent<Animator>();

        // Set the initial position of the drone
        transform.position = new Vector3(207f, 207f, 0f);
    }

    private void Update()
    {
        if (!reachedHoverTarget)
        {
            // Move the drone using approach behavior until it reaches the target hover position
            normalizedTime += Time.deltaTime * moveSpeed;
            float xOffset = xTrajectoryCurve.Evaluate(normalizedTime);
            float yOffset = yTrajectoryCurve.Evaluate(normalizedTime);
            Vector3 newPosition = new Vector3(xOffset, yOffset, 0);
            transform.position = newPosition;

            if (normalizedTime >= 1.0f)
            {
                reachedHoverTarget = true;
                normalizedTime = 0.0f;
            }
        }
        else if (!hovering)
        {
            // Hover at the target hover position
            Hover();
        }
        else if (!landing)
        {
            // Attempt to land after hovering
            float elapsedTime = Time.time - landMoveStartTime;
            float t = Mathf.Clamp01(elapsedTime / landMoveDuration);

            // Interpolate between the target hover position and the target land position
            transform.position = Vector3.Lerp(targetHoverPosition, targetLandPosition, t);

            if (t >= 1.0f)
            {
                landing = true;
                animator.enabled = false; // Disable the Animator component
            }
        }
    }

    private void Hover()
    {
        // Increment the hover timer
        hoverTimer += Time.deltaTime;

        // Calculate the new position for hovering at the target coordinates
        Vector3 newPosition = Vector3.Lerp(targetHoverPosition, targetHoverPosition, hoverTimer / hoverDuration);

        // Apply the new position
        transform.position = newPosition;

        // Check if hovering duration is reached
        if (hoverTimer >= hoverDuration)
        {
            hovering = true; // Start landing after hovering
            hoverTimer = 0.0f; // Reset hover timer
            landMoveStartTime = Time.time; // Record the start time for landing movement
        }
    }
}
