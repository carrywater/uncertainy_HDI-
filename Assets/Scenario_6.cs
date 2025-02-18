//#Scenario 6: Low straight-parachute

using UnityEngine;

public class Scenario_6 : MonoBehaviour
{
    public AnimationCurve xTrajectoryCurve; // (207, 207, 7)
    public AnimationCurve yTrajectoryCurve; // (207, 7, 7)
    public AnimationCurve xMoveToFinalCurve; // (7, 7, 207)
    public AnimationCurve yMoveToFinalCurve; // (7, 207, 207)
    public float moveSpeed = 0.02775f; // Adjust this value for speed
    public Vector3 targetHoverPosition = new Vector3(7f, 7f, 0f); // Hover position
    public Vector3 targetLandPosition = new Vector3(7f, 0f, 0f); // Land position

    public float hoverDuration = 10.0f; // The duration of hovering in seconds
    public float landMoveDuration = 5.0f; // The duration of landing movement

    public Animator parachuteAnimator;

    private enum DroneState
    {
        Approach,
        Hover,
        Deliver,
        MoveToFinal
    }

    private DroneState currentState = DroneState.Approach;

    //parachute credentials
    public Transform parachute; // Reference to the parachute GameObject
    float mass = 1.0f; // Assume a mass of 1 kg for demonstration purposes
    float gravity = 9.81f; // Acceleration due to gravity in m/s�
    float distance = 7.0f; // Vertical distance in meters
    float freeFallTime = 0.5f; // Free fall time in seconds
    float totalTime = 4.7f; // Total time in seconds (takes 4.98 seconds to settle in scene)

    private float normalizedTime = 0f;
    private float normalizedTimee = 0f;
    private bool landing = false;
    private float hoverTimer;
    private float currentTime;
    private float landMoveStartTime;

    private bool parachuteDropped = false; // Flag to track parachute state

    private void Start()
    {
        transform.position = new Vector3(207f, 207f, 0f); // Initial position
        parachuteAnimator.enabled = false;
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
        currentTime += Time.deltaTime;
        Vector3 newPosition = Vector3.Lerp(targetHoverPosition, targetHoverPosition, currentTime / hoverDuration);
        transform.position = newPosition;

        var rigidBody = parachute.GetComponent<Rigidbody>();
        rigidBody.useGravity = true;
        rigidBody.isKinematic = false;

        parachuteAnimator.enabled = true;
        parachuteAnimator.SetTrigger("Open");

        if (currentTime >= freeFallTime && !parachuteDropped)
        {
            float d = (2 * mass * gravity * distance) / ((totalTime - freeFallTime) * (totalTime - freeFallTime));

            // Assign the drag to the parachute itself
            rigidBody.drag = d;
            rigidBody.isKinematic = false;
            parachuteDropped = true;
            parachute.SetParent(null);
        }
        else if (!landing && currentTime >= hoverDuration + hoverDuration)
        {
            parachuteAnimator.SetTrigger("Close");
            currentState = DroneState.MoveToFinal;
            landing = true;
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


////Old code and NOT working code for scenario 6: low-straight_parachute
////Issues are with: parachute not working and drone is also underfree fall
//using UnityEngine;

//public class Scenario_6 : MonoBehaviour
//{

//    public AnimationCurve xTrajectoryCurve; // (207, 207, 7)
//    public AnimationCurve yTrajectoryCurve; // (207, 7, 7)
//    public AnimationCurve xMoveToFinalCurve; // (7, 7, 207)
//    public AnimationCurve yMoveToFinalCurve; // (7, 207, 207)
//    public float moveSpeed = 0.02775f; // Adjust this value for speed
//    public Vector3 targetHoverPosition = new Vector3(7f, 7f, 0f); // Hover position
//    public Vector3 targetLandPosition = new Vector3(7f, 0f, 0f); // Land position

//    public float hoverDuration = 5.0f; // The duration of hovering in seconds
//    public float landMoveDuration = 5.0f; // The duration of landing movement

//    private enum DroneState { Approach, Hover, Deliver, MoveToFinal }
//    private DroneState currentState = DroneState.Approach;

//    //parachute credentials
//    public Transform parachute; // Reference to the parachute GameObject
//    float mass = 1.0f; // Assume a mass of 1 kg for demonstration purposes
//    float gravity = 9.81f; // Acceleration due to gravity in m/s�
//    float distance = 7.0f; // Vertical distance in meters
//    float freeFallTime = 0.5f; // Free fall time in seconds
//    float totalTime = 4.7f; // Total time in seconds (takes 4.98seconds to settle in scene)

//    private float normalizedTime = 0f;
//    private float normalizedTimee = 0f;
//    private bool landing = false;
//    private float hoverTimer;
//    private float currentTime;
//    private float landMoveStartTime;


//    private void Start()
//    {
//        transform.position = new Vector3(207f, 207f, 0f); // Initial position
//    }

//    private void Update()
//    {
//        switch (currentState)
//        {
//            case DroneState.Approach:
//                Approach();
//                break;
//            case DroneState.Hover:
//                Hover();
//                break;
//            case DroneState.Deliver:
//                Deliver();
//                break;
//            case DroneState.MoveToFinal:
//                MoveToFinal();
//                break;
//        }
//    }

//    private void Approach()
//    {
//        normalizedTime += Time.deltaTime * moveSpeed;

//        float xOffset = xTrajectoryCurve.Evaluate(normalizedTime);
//        float yOffset = yTrajectoryCurve.Evaluate(normalizedTime);

//        Vector3 newPosition = new Vector3(xOffset, yOffset, 0);
//        transform.position = newPosition;

//        if (normalizedTime >= 1.0f)
//        {
//            currentState = DroneState.Hover;
//            normalizedTime = 0.0f;
//        }
//    }

//    private void Hover()
//    {
//        hoverTimer += Time.deltaTime;

//        Vector3 newPosition = Vector3.Lerp(targetHoverPosition, targetHoverPosition, hoverTimer / hoverDuration);
//        transform.position = newPosition;

//        if (hoverTimer >= hoverDuration)
//        {
//            currentState = DroneState.Deliver;
//            hoverTimer = 0.0f;
//            landMoveStartTime = Time.time;
//        }
//    }

//    private void Deliver()
//    {
//        currentTime += Time.deltaTime;
//        Debug.Log("Time: " + currentTime);
//        Debug.Log("Position: " + transform.position);

//        if (currentTime <= freeFallTime)
//        {
//            // Simulate parachute falling during the initial 0.5 seconds
//            Vector3 parachutePosition = parachute.position;
//            parachutePosition.y -= gravity * Time.deltaTime;
//            parachute.position = parachutePosition;
//        }
//        else
//        {
//            float d = (2 * mass * gravity * distance) / ((totalTime - freeFallTime) * (totalTime - freeFallTime));
//            Rigidbody rb = parachute.GetComponent<Rigidbody>(); // Get the parachute's Rigidbody
//            rb.drag = d;

//            if (!landing)
//            {
//                // Land the drone (stop its vertical motion)
//                Vector3 dronePosition = transform.position;
//                dronePosition.y = targetLandPosition.y;
//                transform.position = dronePosition;

//                // Attach the parachute to the drone
//                parachute.SetParent(transform);

//                landing = true;
//            }

//            if (currentTime >= totalTime)
//            {
//                currentState = DroneState.MoveToFinal;
//            }
//        }
//    }


//    private void MoveToFinal()
//    {
//        normalizedTimee += Time.deltaTime * moveSpeed; // You can use the same duration for consistency

//        float xOffset = xMoveToFinalCurve.Evaluate(normalizedTimee);
//        float yOffset = yMoveToFinalCurve.Evaluate(normalizedTimee);

//        Vector3 newPosition = new Vector3(xOffset, yOffset, 0);

//        transform.position = newPosition;

//    }
//}