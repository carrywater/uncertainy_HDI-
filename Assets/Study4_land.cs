using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Study4_land : BaseS
{
    public Vector3 targetHoverPosition = new Vector3(-110.239f, 5f, -107.379f); // Hover position
    public Vector3 targetLandPosition = new Vector3(-110.239f, 0.43f, -107.379f); // Land position
    public GameObject Scene;

    // Physics
    public float hoverDuration = 10.0f; // Duration of hovering
    public float landMoveDuration = 10.0f; // Duration of landing movement
    private float acceleration = 1.6f; // Acceleration rate

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
        Vector3 targetPosition = new Vector3(-148.239f, 10.0f, -107.379f);

        droneController.acceleration = acceleration;
        droneController.SetMaxSpeed(11.10f);
        droneController.Accelerate(targetPosition - transform.position);

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

        float remainingDistance = Vector3.Distance(transform.position, targetPosition);
        droneController.acceleration = acceleration;
        droneController.SetMaxSpeed(11.1f);

        if (remainingDistance <= 12.3f && remainingDistance > 0.1f)
        {
            droneController.Deccelerate(targetPosition - transform.position);
        }

        if (remainingDistance <= 0.1f && hoverTimer <= 2.0f)
        {
            currentState = DroneState.Hover1;
            hoverTimer = 0f;
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
        float elapsedTime = Time.time - landMoveStartTime;
        float t = Mathf.Clamp01(elapsedTime / landMoveDuration);
        droneController.acceleration = acceleration;
        droneController.SetMaxSpeed(0.5f);
        droneController.Accelerate(targetLandPosition - transform.position);

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

        if (t >= 1.0f)
        {
            currentState = DroneState.Hover3;
            t = 0.0f;
            landMoveStartTime = Time.time;
        }

        // Update LED Blinker settings for Land phase
        if (droneControllerLite != null && (Interface == 2 || Interface == 4))
        {
            droneControllerLite.isDescending = true;
            droneControllerLite.isAscending = false;
            droneControllerLite.blinkingSpeed = 4.09f;
        }
    }

    private void Hover3()
    {
        hoverTimer += Time.deltaTime;
        droneController.Stop();

        if (hoverTimer >= 0.01f && hoverTimer < 1.0f)
        {
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
        }

        if (hoverTimer >= 1.0f && hoverTimer < 5.0f)
        {
            var rigidBody = Package.GetComponent<Rigidbody>();
            rigidBody.useGravity = true;
            rigidBody.isKinematic = false;
            Package.SetParent(null);
            Package.transform.SetParent(Scene.transform);

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
            
        }

        if (hoverTimer >= 5.0f)
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
        droneController.SetMaxSpeed(0.5f);
        droneController.Accelerate(targetHoverPosition - transform.position);

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

        if (t >= 1.0f)
        {
            currentState = DroneState.MoveToFinal;
        }

        // Update LED Blinker settings for Return phase
        if (droneControllerLite != null && (Interface == 2 || Interface == 4))
        {
            droneControllerLite.isAscending = true;
            droneControllerLite.isDescending = false;
            droneControllerLite.blinkingSpeed = 4.09f;
        }
    }

    private void MoveToFinal()
    {
        Vector3 finalPosition = new Vector3(-110.239f, 15f, -107.379f);
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
        else if (remainingDistance > 5f && remainingDistance <= 10f)
        {
            droneController.SetMaxSpeed(4.0f);
        }
        else
        {
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