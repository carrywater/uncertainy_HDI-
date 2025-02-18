//Working code for scenario 2: high-arc_land
using UnityEngine;

public class s2 : BaseS
{
    private Vector3 startPoint = new Vector3(-160.239f, 57f, -107.379f);
    private Vector3 centerPoint = new Vector3(-160.239f, 7f, -107.379f);
    private Vector3 endPoint = new Vector3(-110.239f, 7f, -107.379f);
    public Vector3 targetHoverPosition = new Vector3(-110.239f, 7f, -107.379f); // Hover position
    public Vector3 targetLandPosition = new Vector3(-110.239f, 0.35f, -107.379f); // Land position
    private Vector3 targetFinalPosition = new Vector3(-110.239f, 59f, -107.379f); // Final position
    //private bool hasMovedToInitialPosition = false; // Add a flag to track if the drone has moved to the initial position.


    public float hoverDuration = 5.0f; // The duration of hovering in seconds
    public float landMoveDuration = 5.0f; // The duration of landing movement
    // Define the deceleration/deceleration rate.
    private float acceleration = 2.05f; // 2.0535 m/s� for 2.7027 s from 11.1 m/s to 5.55 m/s to 0 m/s

    private float hoverTimer;
    private float landMoveStartTime;
    //private Animator animator;

    private float radius;
    private float rotationSpeed;
    private float angle = 0f;
    private float maxAngle = Mathf.PI / 2f; // End at 90 degrees (quarter rotation)

    private void Start()
    {
        //animator = GetComponent<Animator>(); // Get the Animator component
        radius = Vector3.Distance(centerPoint, startPoint);
        transform.position = new Vector3(-360.239f, 57f, -107.379f); // Initial position
    }

    private void Update()
    {
        switch (currentState)
        {
            case DroneState.MoveToInitial:
                MoveToInitial();
                break;
            case DroneState.Approach1:
                Approach();
                break;
            case DroneState.Hover2:
                Hover2();
                break;
            case DroneState.Land:
                Land();
                break;
            case DroneState.Hover3:
                Hover3();
                break;
            case DroneState.Return:
                Return();
                break;
            case DroneState.Hover4:
                Hover4();
                break;
            case DroneState.MoveToFinal:
                MoveToFinal();
                break;
        }
    }


    private void MoveToInitial()
    {
        Vector3 initialPosition = new Vector3(-160.239f, 57f, -107.379f);
        float moveSpeedToInitial = 11.1f; // Adjust this value for the speed of movement to initial position.

        // Calculate the step based on moveSpeedToInitial.
        float step = moveSpeedToInitial * Time.deltaTime;

        // Move the drone towards the initial position.
        transform.position = Vector3.MoveTowards(transform.position, initialPosition, step);

        // Check if the drone has reached the initial position.
        if (Vector3.Distance(transform.position, initialPosition) < 0.01f)
        {
            // Set the flag to indicate that the drone has reached the initial position.
            //hasMovedToInitialPosition = true;

            // Change the state to Approach.
            currentState = DroneState.Approach1;
        }
    }

    private void Approach()
    {
        if (angle < maxAngle)
        {
            if (moveSpeed > 5.55)
            {
                moveSpeed -= acceleration * Time.deltaTime;
            }
            float remainingDistance = Vector3.Distance(transform.position, targetHoverPosition);
            if (remainingDistance <= 7.5f && remainingDistance > 0.1f) // Added remainingDistance > 0.0f condition
            {

                // If the required deceleration is greater than the defined rate, reduce speed.
                if (moveSpeed > 0)
                {
                    moveSpeed -= acceleration * Time.deltaTime;
                }
            }
            rotationSpeed = (moveSpeed * 360) / (2 * radius * Mathf.PI);
            angle += rotationSpeed * Time.deltaTime * Mathf.Deg2Rad;

            float x = Mathf.Sin(angle) * radius + centerPoint.x;
            float y = Mathf.Cos(angle) * radius + centerPoint.y;

            Vector3 newPosition = new Vector3(x, y, transform.position.z);
            transform.position = newPosition;

            if (angle >= maxAngle)
            {
                currentState = DroneState.Hover2;
            }
        }
    }

    private void Hover2()
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
            currentState = DroneState.Hover3;
            //animator.enabled = false; // Disable the Animator component
            t = 0.0f;
            landMoveStartTime = Time.time;
            Package.SetParent(null);
        }
    }

    private void Hover3()
    {
        hoverTimer += Time.deltaTime;

        Vector3 newPosition = Vector3.Lerp(targetLandPosition, targetLandPosition, hoverTimer / (hoverDuration - 3));
        transform.position = newPosition;

        if (hoverTimer >= (hoverDuration - 3))
        {
            currentState = DroneState.Return;
            hoverTimer = 0.0f;
            landMoveStartTime = Time.time;
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
            currentState = DroneState.Hover4;
        }
    }

    private void Hover4()
    {
        hoverTimer += Time.deltaTime;

        Vector3 newPosition = Vector3.Lerp(targetHoverPosition, targetHoverPosition, hoverTimer / hoverDuration);
        transform.position = newPosition;

        if (hoverTimer >= hoverDuration)
        {
            currentState = DroneState.MoveToFinal;
            hoverTimer = 0.0f;
            landMoveStartTime = Time.time;
        }
    }

    private void MoveToFinal()
    {
        Vector3 finalPosition = new Vector3(-110.239f, 59f, -107.379f);

        // Calculate the step based on moveSpeedToInitial.
        float step = moveSpeedd * Time.deltaTime;

        // Move the drone towards the initial position.
        transform.position = Vector3.MoveTowards(transform.position, finalPosition, step);

        // Calculate the distance to the hover position.
        float remainingDistance = Vector3.Distance(transform.position, targetHoverPosition);

        // Check if the drone is in the last 15 meters and needs to decelerate.
        if (remainingDistance <= 15.0f && remainingDistance > 0.0f) // Added remainingDistance > 0.0f condition
        {

            // If the required deceleration is greater than the defined rate, reduce speed.
            if (moveSpeedd < 11.1f)
            {
                moveSpeedd += 4.107f * Time.deltaTime;
            }
            else
            {
                moveSpeedd = 11.1f;
            }
        }
        else
        {
            moveSpeedd = 11.1f;
        }

    }
}


