//Working code for scenario 1: high-straight_land
using UnityEngine;

public class s1 : BaseS
{
    public Vector3 targetHoverPosition = new Vector3(-110.239f, 7f, -107.379f); // Hover position
    public Vector3 targetLandPosition = new Vector3(-110.239f, 0.35f, -107.379f); // Land position
    private Vector3 targetFinalPosition = new Vector3(-160.239f, 59f, -107.379f); // Final position
    //private bool hasMovedToInitialPosition = false; // Add a flag to track if the drone has moved to the initial position.

    public float hoverDuration = 5.0f; // The duration of hovering in seconds
    public float landMoveDuration = 5.0f; // The duration of landing movement
    // Define the deceleration/deceleration rate.
    private float acceleration = 4.107f; // 4.107 m/s�

    private float hoverTimer;
    private float landMoveStartTime;
    //private Animator animator;

    private void Start()
    {
        //animator = GetComponent<Animator>(); // Get the Animator component
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
                Approach1();
                break;
            case DroneState.Approach2:
                Approach2();
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

        // Calculate the distance to move.
        //float distanceToInitial = Vector3.Distance(transform.position, initialPosition);

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

    private void Approach1()
    {
        Vector3 initialPosition = new Vector3(-110.239f, 57f, -107.379f);
        // Calculate the remaining distance to the deceleration point (-110.239, 57, -107.379).
        float remainingDistance = Vector3.Distance(transform.position, initialPosition);

        // Check if the drone is within 15 meters of the deceleration point and needs to decelerate.
        if (remainingDistance <= 15.0f && remainingDistance > 0.1f)
        {
            // If the required deceleration is greater than the defined rate, reduce speed.
            if (moveSpeed > 0)
            {
                moveSpeed -= acceleration * Time.deltaTime;
                //Debug.Log("decelerating: " + moveSpeed);
            }
        }

        // Calculate the step based on moveSpeedToInitial.
        float step = moveSpeed * Time.deltaTime;

        // Move the drone towards the initial position.
        transform.position = Vector3.MoveTowards(transform.position, initialPosition, step);

        // Check if the drone has completed the deceleration and should hover.
        if (remainingDistance <= 0.1f && hoverTimer <= (hoverDuration-3.3f))
        {
            // Increment the hover timer.
            hoverTimer += Time.deltaTime;
            Vector3 newPosition = Vector3.Lerp(initialPosition, initialPosition, hoverTimer / (hoverDuration-3.3f));
            transform.position = newPosition;
            //Debug.Log("hover");
            // Continue hovering until the hover duration is reached.
            if (hoverTimer >= (hoverDuration-3.3f))
            {
                currentState = DroneState.Approach2;
                hoverTimer = 0f; // Reset hover timer for acceleration phase.
                moveSpeed = 1.34f;
            }

        }
    }

    private void Approach2()
    {
            // Calculate the remaining distance to the targetHoverPosition.
        float remainingDistance2 = Vector3.Distance(transform.position, targetHoverPosition);
        //Debug.Log("remaining distance: "+remainingDistance2);

        // Check if the drone is within 15 meters of the final position and needs to acceleration.
        if (remainingDistance2 >= 35.0f)
        {
            // If the current speed is less than the desired speed, acceleration.
            if (moveSpeed < 11.1f)
            {
                moveSpeed += acceleration * Time.deltaTime;
                //Debug.Log("accelerating");
            }
            else
            {
                moveSpeed = 11.1f;
            }
        }

        else if (remainingDistance2 < 35.0f && remainingDistance2 > 15.0f)
        {
                moveSpeed = 11.1f;
        }

        // Check if the drone is in the last 15 meters and needs to decelerate.
        if (remainingDistance2 <= 15.0f && remainingDistance2 > 0.1f) // Added remainingDistance > 0.0f condition
        {

            // If the required deceleration is greater than the defined rate, reduce speed.
            if (moveSpeed > 0)
            {
                moveSpeed -= acceleration * Time.deltaTime;
            }
        }

        // Otherwise, set speed to 0 to stop.
        float step2 = moveSpeed * Time.deltaTime;
        // Move the drone towards the initial position.
        transform.position = Vector3.MoveTowards(transform.position, targetHoverPosition, step2);

        // Check if the drone has reached the initial position.
        if (remainingDistance2 < 0.01f)
        {
            // Set the flag to indicate that the drone has reached the initial position.
            //hasMovedToInitialPosition = true;

            // Change the state to Approach.
            currentState = DroneState.Hover2;
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
                moveSpeedd += acceleration * Time.deltaTime;
            }
            // Otherwise, set speed to 0 to stop.
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


