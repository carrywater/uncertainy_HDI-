//Working code for scenario 2: high-arc_land
using UnityEngine;

public class S2_new : BaseS
{

    public GameObject Scene;
    private Vector3 startPoint = new Vector3(-148.239f, 45f, -107.379f);
    private Vector3 centerPoint = new Vector3(-148.239f, 7f, -107.379f);


    public Vector3 targetHoverPosition = new Vector3(-110.239f, 7f, -107.379f);
    public Vector3 targetLandPosition = new Vector3(-110.239f, 0.061f, -107.379f);
    
    public float hoverDuration = 5.0f;
    public float landMoveDuration = 5.0f; 

    private float acceleration = 5.0f;

    private float hoverTimer = 0.0f;
    private float landMoveStartTime;

    private float radius;
    private float rotationSpeed;
    private float angle = 0f;
    private float maxAngle = Mathf.PI / 2f;

    private void Start()
    {
        radius = Vector3.Distance(centerPoint, startPoint);
        transform.position = new Vector3(-355.239f, 45f, -107.379f);
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
    
        droneController.acceleration = acceleration;

        droneController.SetMaxSpeed(11.10f);
        droneController.Accelerate(targetPosition - transform.position);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentState = DroneState.Approach1;
        }
    }

    private void Approach1()
    {
        if (angle < maxAngle)
        {
            if (moveSpeed > 5)
            {
                moveSpeed -= acceleration * Time.deltaTime;
            }
            float remainingDistance = Vector3.Distance(transform.position, targetHoverPosition);
            if (remainingDistance <= 2.46f && remainingDistance > 0.1f) // Added remainingDistance > 0.0f condition
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
            droneController.SetMaxSpeed(0.0001f);
            droneController.UpdateAudioPitch1(moveSpeed);
        }
        else
        {
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
        float elapsedTime = Time.time - landMoveStartTime;
        float t = Mathf.Clamp01(elapsedTime / landMoveDuration);
        
        droneController.acceleration = acceleration;

        droneController.SetMaxSpeed(1.34f);
        droneController.Accelerate(targetLandPosition - transform.position);

        if (t >= 1.0f)
        {
            currentState = DroneState.Hover3;
            t = 0.0f;
            landMoveStartTime = Time.time;
        }
    }

    private void Hover3()
    {
        hoverTimer += Time.deltaTime;

        droneController.Stop();

        if (hoverTimer >= 1.0f && hoverTimer < 2.0f)
        {
            var rigidBody = Package.GetComponent<Rigidbody>();
            rigidBody.useGravity = true;
            rigidBody.isKinematic = false;
            Package.SetParent(null);
            Package.transform.SetParent(Scene.transform);
        }
        if (hoverTimer >= 2.0f)
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

        droneController.acceleration = acceleration;

        droneController.SetMaxSpeed(1.34f);
        droneController.Accelerate(targetHoverPosition - transform.position);

        if (t >= 1.0f)
        {
            currentState = DroneState.Hover4;
        }
    }

    private void Hover4()
    {
        hoverTimer += Time.deltaTime;

        droneController.Stop();

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

        float remainingDistance = Vector3.Distance(transform.position, targetHoverPosition);

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


