using Unity.VisualScripting;
using UnityEngine;
using System.Collections; // Added to access IEnumerator for coroutines
using System.Collections.Generic; // Needed for List<T>

public class Study4_cable : BaseS
{
    public Transform Pseudo;
    public Transform Empty;
    public GameObject Scene;
    public Vector3 targetHoverPosition = new Vector3(-110.239f, 5f, -107.379f); // Hover position

    //Cable
    private bool isRetractingPseudo = false;
    private float pseudoRetractStartTime;

    private Vector3 initialPackagePosition;
    private Vector3 initialPseudoPosition;

    //Physics
    public float hoverDuration = 10.0f; // The duration of hovering in seconds
    public float landMoveDuration = 20.0f; // The duration of landing movement

    // Define the acceleration and deceleration rates
    private float acceleration = 1.6f; // 4.107 m/s²

    private float hoverTimer = 0.0f;
    private float landMoveStartTime;

    // Arrow reference
    private GameObject arrow;
    private Material arrowMaterial;

    // LandingPad reference
    private GameObject landingPad;
    private Material landingPadMaterial;

    // Public Interface input to control which properties to change
    public int Interface = 1; // 1: Arrow, 3: LandingPad, 4: Both

    // Reference to the DroneControllerLite
    private DroneControllerLite droneControllerLite;


    private void Start()
    {
        // Initialize droneControllerLite by finding the Drone object and getting the DroneControllerLite component
        GameObject droneObject = transform.Find("Drone").gameObject;
        droneControllerLite = droneObject.GetComponentInChildren<DroneControllerLite>();

        transform.position = new Vector3(-110.239f, 15.0f, -107.379f); // Initial position

        // Corrected hierarchy traversal for Arrow
        Transform arrowTransform = transform.Find("Drone/1-DISPLAYS/Arrow");
        if (arrowTransform != null)
        {
            arrow = arrowTransform.gameObject;
            arrowMaterial = arrow.GetComponent<Renderer>().material;

            // Ensure the Arrow is inactive at the start
            arrow.SetActive(false);
        }
        else
        {
            Debug.LogError("Arrow GameObject not found in the expected hierarchy.");
        }

        // Corrected hierarchy traversal for LandingPad
        Transform landingPadTransform = transform.Find("Drone/3-PROJECTOR/LandingPad");
        if (landingPadTransform != null)
        {
            landingPad = landingPadTransform.gameObject;
            landingPadMaterial = landingPad.GetComponent<Renderer>().material;

            // Ensure the LandingPad is inactive at the start
            landingPad.SetActive(false);
        }
        else
        {
            Debug.LogError("LandingPad GameObject not found in the expected hierarchy.");
        }
    }

    protected void Update()
    {
        switch (currentState)
        {
            case DroneState.MoveToInitial:
                MoveToInitial();
                arrow.SetActive(false);
                landingPad.SetActive(false);
                break;
            case DroneState.Approach1:
                Approach1();
                arrow.SetActive(false);
                landingPad.SetActive(false);
                break;
            case DroneState.Hover1:
                Hover1();
                arrow.SetActive(false);
                landingPad.SetActive(false);
                break;    
            case DroneState.Approach2:
                Approach2();
                arrow.SetActive(false);
                landingPad.SetActive(false);
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
        Vector3 targetPosition = new Vector3(-148.239f, 15.0f, -107.379f);
    
        // Set the acceleration in BaseDroneController
        droneController.acceleration = acceleration;

        // Calculate the remaining distance to the initial position.
        droneController.SetMaxSpeed(11.10f);
        droneController.Accelerate(targetPosition - transform.position);

        // Check if the drone has reached the initial position.
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentState = DroneState.Approach1;
        }

        // Update LED Blinker settings
        if (droneControllerLite != null)
        {
            droneControllerLite.isAscending = false;
            droneControllerLite.isDescending = false;
        }
    }

    private void Approach1()
    {
        Vector3 targetPosition = new Vector3(-110.239f, 15.0f, -107.379f);

        // Calculate the remaining distance to the deceleration point.
        float remainingDistance = Vector3.Distance(transform.position, targetPosition);

        // Set the acceleration in BaseDroneController
        droneController.acceleration = acceleration;

        // Calculate the remaining distance to the initial position.
        droneController.SetMaxSpeed(5.55f);

        // Check if the drone is within 15 meters of the deceleration point and needs to decelerate.
        if (remainingDistance <= 12.3f && remainingDistance > 0.1f)
        {
            droneController.Deccelerate(targetPosition - transform.position);
        }

        // Check if the drone has completed the deceleration and should hover.
        if (remainingDistance <= 0.1f && hoverTimer <= 2.0f)
        {  
            currentState = DroneState.Hover1;
            hoverTimer = 0f; // Reset hover timer for acceleration phase.
        }

        // Update LED Blinker settings
        if (droneControllerLite != null)
        {
            droneControllerLite.isAscending = false;
            droneControllerLite.isDescending = false;
        }
    }

    private void Hover1()
    {
        hoverTimer += Time.deltaTime;
        droneController.Stop();

        if (hoverTimer >= 2.0f)
        {
            currentState = DroneState.Approach2;
            hoverTimer = 0f;
        }

        // Update LED Blinker settings
        if (droneControllerLite != null)
        {
            droneControllerLite.isAscending = false;
            droneControllerLite.isDescending = false;
        }
    }

    private void Approach2()
    {
        Vector3 targetPosition = new Vector3(-110.239f, 5f, -107.379f);

        droneController.acceleration = acceleration;
        droneController.SetMaxSpeed(4.0f);

        // Calculate the remaining distance to the targetHoverPosition.
        float remainingDistance2 = Vector3.Distance(transform.position, targetPosition);

        if (remainingDistance2 >= 5.0f)
        {
            droneController.Accelerate(targetPosition - transform.position);
        }

        if (remainingDistance2 <= 5.0f && remainingDistance2 > 0.2f)
        {
            droneController.Deccelerate(targetPosition - transform.position);
        }

        if (remainingDistance2 <= 0.2f)
        {   
            currentState = DroneState.Hover2;
        }

        // Update LED Blinker settings
        if (droneControllerLite != null)
        {
            droneControllerLite.isAscending = false;
            droneControllerLite.isDescending = false;
        }

    }

    private void Hover2()
    {
        hoverTimer += Time.deltaTime;
        droneController.Stop();

        if (arrow != null && !arrow.activeSelf)
        {
            // Only change Arrow properties if Interface is 1 or 4
            if (Interface == 1 || Interface == 4)
            {
                arrow.SetActive(true);
                arrowMaterial.SetFloat("_isAnimating", 0); // false
                arrowMaterial.SetFloat("_pointedUP", 0); // false
            }
        }

        if (landingPad != null && !landingPad.activeSelf)
        {
            // Only change LandingPad properties if Interface is 3 or 4
            if (Interface == 3 || Interface == 4)
            {
                landingPad.SetActive(true);
                landingPadMaterial.SetFloat("_animatedRingsOn", 0); // false
                landingPadMaterial.SetFloat("_outwardsAnimation", 0); // false
            }
        }

        if (hoverTimer >= hoverDuration)
        {
            currentState = DroneState.Land;
            hoverTimer = 0.0f;
            landMoveStartTime = Time.time;
        }

        // Update LED Blinker settings for Hover2 phase
        if (droneControllerLite != null && (Interface == 2 || Interface == 4))
        {
            droneControllerLite.isDescending = true;
            droneControllerLite.isAscending = true;
            droneControllerLite.blinkingSpeed = 0.1f;
        }
    }

    private void Land()
    {
        initialPackagePosition = Empty.position; 
        initialPseudoPosition = Empty.position; 
        float elapsedTime = Time.time - landMoveStartTime;
        float t = Mathf.Clamp01(elapsedTime / landMoveDuration);
        Package.position = Vector3.Lerp(initialPackagePosition, new Vector3(-110.239f, 0.137f, transform.position.z), t);
        Pseudo.position = Vector3.Lerp(initialPackagePosition, new Vector3(-110.239f, 0.137f, transform.position.z), t);
        Package.SetParent(null);

        droneController.Stop();

        if (arrow != null)
        {
            // Only change Arrow properties if Interface is 1 or 4
            if (Interface == 1 || Interface == 4)
            {
                arrowMaterial.SetFloat("_isAnimating", 1); // true
                arrowMaterial.SetFloat("_pointedUP", 0); // false
            }
        }

        if (landingPad != null)
        {
            // Only change LandingPad properties if Interface is 3 or 4
            if (Interface == 3 || Interface == 4)
            {
                landingPadMaterial.SetFloat("_animatedRingsOn", 1); // true
                landingPadMaterial.SetFloat("_outwardsAnimation", 1); // true
            }
        }

        // Update LED Blinker settings for Land phase
        if (droneControllerLite != null && (Interface == 2 || Interface == 4))
        {
            droneControllerLite.isDescending = true;
            droneControllerLite.isAscending = false;
            droneControllerLite.blinkingSpeed = 4.09f;
        }

        if (t >= 1.0f)
        {
            currentState = DroneState.Hover3;
            hoverTimer = 0.0f;
            droneControllerLite.isAscending = false;
            droneControllerLite.isDescending = false;
        }
    }

    private void Hover3()
    {
        hoverTimer += Time.deltaTime;
        Pseudo.position = Vector3.Lerp(new Vector3(-110.239f, 0.137f, transform.position.z), new Vector3(-110.239f, 0.137f, transform.position.z), 5.0f);
        droneController.Stop();

        if (arrow != null)
            {
                // Only change Arrow properties if Interface is 1 or 4
                if (Interface == 1 || Interface == 4)
                {
                    arrowMaterial.SetFloat("_isAnimating", 0); // false
                    arrowMaterial.SetFloat("_pointedUP", 1); // true
                }
            }
        
            if (landingPad != null)
            {
                // Only change LandingPad properties if Interface is 3 or 4
                if (Interface == 3 || Interface == 4)
                {
                    landingPadMaterial.SetFloat("_animatedRingsOn", 0); // false
                    landingPadMaterial.SetFloat("_outwardsAnimation", 0); // false
                    landingPadMaterial.SetFloat("_ring2Radius", 0); //removes inner circle (but also for the rest of the phases)
                }
            }
            
            // Update LED Blinker settings for Hover3 phase
            if (droneControllerLite != null && (Interface == 2 || Interface == 4))
            {
                droneControllerLite.isAscending = true;
                droneControllerLite.isDescending = true;
                droneControllerLite.blinkingSpeed = 0.01f;
                droneControllerLite.blinkerLedInstensity = 30.0f;
            }

        if (hoverTimer >= hoverDuration)
        {
            currentState = DroneState.Return;
            hoverTimer = 0.0f;
            landMoveStartTime = Time.time;
        }
    }

    private void Return()
    {
        if (!isRetractingPseudo)
        {
            isRetractingPseudo = true;
            pseudoRetractStartTime = Time.time;
        }

        float pseudoNormalizedTime = (Time.time - pseudoRetractStartTime) / landMoveDuration;
        Vector3 pseudoNewPosition = Vector3.Lerp(new Vector3(-110.239f, 0.137f, transform.position.z), Empty.position, pseudoNormalizedTime);
        Pseudo.position = pseudoNewPosition;

        if (arrow != null)
        {
            // Only change Arrow properties if Interface is 1 or 4
            if (Interface == 1 || Interface == 4)
            {
                arrowMaterial.SetFloat("_isAnimating", 1); // true
                arrowMaterial.SetFloat("_pointedUP", 1); // true
            }
        }

        if (landingPad != null)
        {
            // Only change LandingPad properties if Interface is 3 or 4
            if (Interface == 3 || Interface == 4)
            {
                landingPadMaterial.SetFloat("_animatedRingsOn", 1); // true
                landingPadMaterial.SetFloat("_outwardsAnimation", 0); // false
            }
        }

        if (pseudoNormalizedTime >= 1.0f)
        {
            currentState = DroneState.MoveToFinal;
            landMoveStartTime = Time.time;
            hoverTimer = 0;
            Package.SetParent(null);
        }

        // Update LED Blinker settings for Return phase
        if (droneControllerLite != null && (Interface == 2 || Interface == 4))
        {
            droneControllerLite.isAscending = true;
            droneControllerLite.isDescending = false;
            droneControllerLite.blinkingSpeed = 4.09f;
            droneControllerLite.blinkerLedInstensity = 15.0f;
        }
    }

    private void MoveToFinal()
    {
        Vector3 finalPosition = new Vector3(-110.239f, 15f, -107.379f);
        Package.transform.SetParent(Scene.transform);

        float remainingDistance = Vector3.Distance(transform.position, targetHoverPosition);

        // Deactivate arrow and landing pad if needed.
        if (Interface == 1 || Interface == 4)
        {
            arrow.SetActive(false);
        }
        if (Interface == 3 || Interface == 4)
        {
            landingPad.SetActive(false);
        }

        // Update LED Blinker settings for final phase
        if (droneControllerLite != null && (Interface == 2 || Interface == 4))
        {
            droneControllerLite.isAscending = false;
            droneControllerLite.isDescending = false;
            droneControllerLite.blinkingSpeed = 0.1f; // Adjust blinking speed for final phase
        }

        if (remainingDistance <= 5f && remainingDistance > 0.0f)
        {
            droneController.acceleration = acceleration;
            droneController.Accelerate(finalPosition - transform.position);
            droneController.SetMaxSpeed(4.0f);
        }
        else if (remainingDistance > 5f && remainingDistance <= 10f){
            droneController.SetMaxSpeed(4.0f);

        }
        else{
            currentState = DroneState.End;
        }
    }

    private void End()
    {
        droneController.Stop();
        arrow.SetActive(false);
        landingPad.SetActive(false);
    }
}
