//Working code for scenario 3: high-straight_cable-land
using Unity.VisualScripting;
using UnityEngine;

public class S3_new : BaseS
{
    public Transform Pseudo;
    public Transform Empty;
    public GameObject Scene;
    public Vector3 targetHoverPosition = new Vector3(-110.239f, 7f, -107.379f); // Hover position
    //public Vector3 targetLandPosition = new Vector3(-110.239f, 0.061f, -107.379f); // Land position

    //Cable
    private bool isRetractingPseudo = false;
    private float pseudoRetractStartTime;

    private Vector3 initialPackagePosition;
    private Vector3 initialPseudoPosition;

    //Physics
    public float hoverDuration = 5.0f; // The duration of hovering in seconds
    public float landMoveDuration = 6.0f; // The duration of landing movement

    // Define the acceleration and deceleration rates
    private float acceleration = 5.0f; // 4.107 m/s�

    private float hoverTimer = 0.0f;
    private float landMoveStartTime;

    private void Start()
    {
        transform.position = new Vector3(-355.239f, 45.0f, -107.379f); // Initial position
    }

    protected void Update()
    {
        switch (currentState)
        {
            case DroneState.MoveToInitial:
                MoveToInitial();
                break;
            case DroneState.Approach1:
                Approach1();
                break;
            case DroneState.Hover1:
                Hover1();
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
            case DroneState.End:
                End();
                break;
        }
    }

    private void MoveToInitial()
    {
        Vector3 targetPosition = new Vector3(-148.239f, 45.0f, -107.379f);
    
        // Set the acceleration in BaseDroneController
        droneController.acceleration = acceleration;

        // Calculate the remaining distance to the initial position.
        droneController.SetMaxSpeed(11.10f);
        droneController.Accelerate(targetPosition - transform.position);

        // Check if the drone has reached the initial position.
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Change the state to Approach.
            currentState = DroneState.Approach1;
            //Debug.Log("Entered");
        }
    }

    private void Approach1()
    {
        Vector3 targetPosition = new Vector3(-110.239f, 45.0f, -107.379f);

        // Calculate the remaining distance to the deceleration point (-110.239, 57, -107.379).
        float remainingDistance = Vector3.Distance(transform.position, targetPosition);

        // Set the acceleration in BaseDroneController
        droneController.acceleration = acceleration;

        // Calculate the remaining distance to the initial position.
        droneController.SetMaxSpeed(11.1f);

        // Check if the drone is within 15 meters of the deceleration point and needs to decelerate.
        if (remainingDistance <= 12.3f && remainingDistance > 0.1f)
        {
            //droneController.SetDirection(targetPosition - transform.position);
            droneController.Deccelerate(targetPosition - transform.position);
        }

        // Check if the drone has completed the deceleration and should hover.
        if (remainingDistance <= 0.1f && hoverTimer <= 2.0f)
        {  
            currentState = DroneState.Hover1;
            hoverTimer = 0f; // Reset hover timer for acceleration phase.
        }
    }

    private void Hover1()
    {
        // Increment the hover timer.
        hoverTimer += Time.deltaTime;

        droneController.Stop();
        // Continue hovering until the hover duration is reached.
        if (hoverTimer >= 2.0f)
        {
            currentState = DroneState.Approach2;
            hoverTimer = 0f; // Reset hover timer for acceleration phase.
        }
    }

    private void Approach2()
    {
        Vector3 targetPosition = new Vector3(-110.239f, 7f, -107.379f);

        droneController.acceleration = acceleration;

        // Calculate the remaining distance to the targetHoverPosition.
        float remainingDistance2 = Vector3.Distance(transform.position, targetPosition);

        // Check if the drone is within 15 meters of the final position and needs to acceleration.
        if (remainingDistance2 >= 25.6f)
        {
            droneController.Accelerate(targetPosition - transform.position);
            droneController.SetMaxSpeed(11.1f);
        }

        else if (remainingDistance2 < 25.6f && remainingDistance2 > 12.0f)
        {
            droneController.SetMaxSpeed(11.1f);
        }

        // Check if the drone is in the last 15 meters and needs to decelerate.
        if (remainingDistance2 <= 12.0f && remainingDistance2 > 0.1f)
        {
            droneController.Deccelerate(targetPosition - transform.position);
        }

        // Check if the drone has reached the initial position.
        if (remainingDistance2 <= 0.1f)
        {   
            // Change the state to Approach.
            currentState = DroneState.Hover2;
        }

    }

    private void Hover2()
    {
        hoverTimer += Time.deltaTime;

           droneController.Stop();

        if (hoverTimer >= hoverDuration)
        {
            currentState = DroneState.Land;
            hoverTimer = 0.0f;
            landMoveStartTime = Time.time;
        }
    }


    private void Land()
    {
        initialPackagePosition = Empty.position; // Use world position instead of localPosition
        initialPseudoPosition = Empty.position; // Use world position instead of localPosition
        float elapsedTime = Time.time - landMoveStartTime;
        float t = Mathf.Clamp01(elapsedTime / landMoveDuration);
        Package.position = Vector3.Lerp(initialPackagePosition, new Vector3(-110.239f, 0.061f, transform.position.z), t);
        Pseudo.position = Vector3.Lerp(initialPackagePosition, new Vector3(-110.239f, 0.061f, transform.position.z), t);
        Package.SetParent(null); // Detach the Package from its parent immediately

        droneController.Stop();

        if (t >= 1.0f)
        {
            currentState = DroneState.Hover3;
            hoverTimer = 0.0f;
        }
    }

    private void Hover3()
    {
        hoverTimer += Time.deltaTime;
        Pseudo.position = Vector3.Lerp(new Vector3(-110.239f, 0.061f, transform.position.z), new Vector3(-110.239f, 0.061f, transform.position.z), 2.0f);
        droneController.Stop();

        if (hoverTimer >= 2.0f)
        {
            currentState = DroneState.Return;
            hoverTimer = 0.0f;
            landMoveStartTime = Time.time;
        }
    }

    private void Return()
    {
        if (!isRetractingPseudo) // Check if retraction is not already initiated
        {
            isRetractingPseudo = true;
            pseudoRetractStartTime = Time.time; // Start the timer for pseudo retraction
        }

        float pseudoNormalizedTime = (Time.time - pseudoRetractStartTime) / landMoveDuration;
        Vector3 pseudoNewPosition = Vector3.Lerp(new Vector3(-110.239f, 0.061f, transform.position.z), Empty.position, pseudoNormalizedTime);
        Pseudo.position = pseudoNewPosition;

        if (pseudoNormalizedTime >= 1.0f)
        {
            currentState = DroneState.Hover4;
            landMoveStartTime = Time.time;
            hoverTimer = 0;
            Package.SetParent(null);
        }
    }


    private void Hover4()
    {
        hoverTimer += Time.deltaTime;

        droneController.Stop();
        Package.transform.SetParent(Scene.transform);

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

        // Calculate the distance to the hover position.
        float remainingDistance = Vector3.Distance(transform.position, targetHoverPosition);

        // Check if the drone is in the last 15 meters and needs to decelerate.
        if (remainingDistance <= 12.5f && remainingDistance > 0.0f)
        {

            droneController.acceleration = acceleration;
            droneController.Accelerate(finalPosition - transform.position);
            droneController.SetMaxSpeed(11.1f);
        }
        else if (remainingDistance > 12.5f && remainingDistance <= 52.0f){
            droneController.SetMaxSpeed(11.1f);

        }
        else{
            currentState = DroneState.End;
        }
    }

    private void End()
    {
        droneController.Stop();
    }
}


