//Working code for scenario 3: high-straight_cable-land
using Unity.VisualScripting;
using UnityEngine;

public class Filler : BaseS
{
    private float acceleration = 5.0f;


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
        Vector3 targetPosition = new Vector3(-155.239f, 45.0f, -107.379f);
    
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
        }
    }

    private void Approach1()
    {
        Vector3 targetPosition = new Vector3(-110.239f, 45.0f, -107.379f);

        // Set the acceleration in BaseDroneController
        droneController.acceleration = acceleration;

        // Calculate the remaining distance to the initial position.
        droneController.SetMaxSpeed(11.1f);
        
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentState = DroneState.MoveToFinal;
        }
    }

    private void MoveToFinal()
    {
        Vector3 targetPosition = new Vector3(-20.5f, 45.0f, -107.379f);

        // Set the acceleration in BaseDroneController
        droneController.acceleration = acceleration;

        // Calculate the remaining distance to the initial position.
        droneController.SetMaxSpeed(11.1f);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentState = DroneState.End;
        }
    }

    private void End()
    {
        droneController.Stop();
    }
}


