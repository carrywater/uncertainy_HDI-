using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{

    [Header("Propeller Settings")]
    public bool rotatePropellers = false;
    [Range(0.0f, 100.0f)] public float rotationSpeed = 1f; // Maximum rotation speed
    [Range(0.0f, 1f)] public float windupTime = 2f; // Time to reach full speed
    [Range(0.0f, 1f)] public float winddownTime = 2f; // Time to stop
    private float currentRotationSpeed = 0f; // Current rotation speed
    private List<GameObject> propellers = new List<GameObject>(); //List to quick acess all propellers at the same time

    [Header("Screen Settings")]
    public bool screenIsEnabled = true;
    [Range(0, 2)] public int bezelSize = 0;
    [Range(1f, 1.5f)] public float screenSizeMultiplier = 1f;
    public bool screenIsOn = false;
    private List<GameObject> screens = new List<GameObject>();
    public bool screenIsVertical = false;
    public Color screenColor; //Base color of screen
    [Range(0.0f, 10.0f)] public float screenLightIntensity = 1f; //This affects the bloom effect
    private GameObject screenParent;
    private List<Material> screenMats = new List<Material>();

    [Header("LED Settings")]
    public bool ledsAreEnabled = true;
    public bool ledsAreOn = false;
    public Color LEDColor; //Base color of LEDS
    [Range(0f, 100f)] public List<float> LEDIntensities = new List<float> { 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f };
    [Range(1f, 3f)] public float ledSize = 1f;
    private GameObject ledParent;
    private List<GameObject> leds = new List<GameObject>();
    private List<Material> ledMats = new List<Material>();

    [Header("Blinker LED Settings")]
    public bool blinkerLedsAreEnabled = false;
    public bool blinkerLedsAreOn = true;
    public bool isAscending = true;
    [Range(1, 10)] public float blinkingSpeed = 2f;
    public Color ascendLedColor;
    public Color descendLedColor;
    [Range(0, 40f)] public float blinkerLedInstensity = 1f;
    private List<GameObject> blinkerLeds = new List<GameObject>();
    private GameObject blinkerParent;
    private Material ascendBlinkerMat;
    private Material descendBlinkerMat;

    [Header("Arrow Settings")]
    public bool arrowIsOn = true;
    [Range(.25f, .5f)] public float arrowHeadHeight = .5f;
    [Range(.25f, 1f)] public float arrowHeadWidth = .75f;
    [Range(.25f, 1f)] public float arrowBodyHeight = .75f;
    [Range(.15f, .75f)] public float arrowBodyWidth = .25f;
    public bool isAnimating = false;
    public bool pointedUp = false;
    [Range(0, 10)] public float animationSpeed = 2f;
    public Color arrowColor = Color.white;
    private GameObject arrowObject;
    private Material arrowMat;
    private Vector3 arrowObjectDefaultSize;
    private float arrowObjectDefaultY;

    [Header("Projector Settings")]
    public bool projectorEnabled = true;
    public bool projectorIsOn = true;
    public bool lightPhysics = false;
    [Range(0.2f, 1f)] public float minOpacity = 0.2f; //This is the minimum opacity value of the ground projection at maximum altitude
    [Range(10f, 100f)] public float minOpacityAltitude = 20f; //This is the maximum altitude of the drone where ground projection is at it's minimum opacity
    [Range(.1f, 4.0f)] public float projectionSizeMultiplier = 1f;
    public Transform groundObject;
    private GameObject projector;
    public GameObject landingPadObject;
    private Material projectorMat;
    private Material projectorDecalMat;

    /*[Header("Projection Settings")]
    [Range(0, .5f)] public float firstCircleOuter = .50f;
    [Range(0, .5f)] public float firstCircleInner = .45f;
    public Color firstCircleColor;
    [Range(0, .5f)] public float secondCircleOuter = .25f;
    [Range(0, .5f)] public float secondCircleInner = .35f;
    public Color secondCircleColor;
    public bool showH = true;
    [Range(1.0f, 2.5f)] public float sizeH;
    public Color colorH;*/



    void Start()
    {
        GetReferences(); //This method automatically gathers references mostly by looking them up by their name
    }

    void Update()
    {
        UpdatePropellerRotation(); //Handles propellers rotation, wind-up and down etc.
        UpdateScreen(); //Handles everything related with the Screen module
        UpdateArrow();
        UpdateLEDs(); //Handles everything related with the LED module
        UpdateBlinkers();
        UpdateProjector(); //Handles everything related with the Projector module
        UpdateProjection();
    }

    public void GetReferences()
    {
        propellers.Clear(); //Safety to avoid duplicates 
        leds.Clear();
        ledMats.Clear();

        FindNestedChildren(transform, "Propeller", propellers);
        FindNestedChildren(transform, "Led", leds);

        screenParent = gameObject.transform.Find("1-DISPLAYS").gameObject;

        foreach (GameObject led in leds) 
        {
            ledMats.Add(led.GetComponent<MeshRenderer>().materials[1]);
        }

        foreach (Transform child in screenParent.transform)
        {
            screens.Add(child.gameObject);
        }

        foreach (GameObject screen in screens)
        {
            screenMats.Add(screen.GetComponent<MeshRenderer>().materials[1]);
        }

        blinkerParent = gameObject.transform.Find("2.1-BLINKERLEDS").gameObject;

        foreach (Transform child in blinkerParent.transform)
        {
            blinkerLeds.Add(child.gameObject);
        }

        descendBlinkerMat = blinkerLeds[0].GetComponent<MeshRenderer>().materials[1];
        ascendBlinkerMat = blinkerLeds[1].GetComponent<MeshRenderer>().materials[1];

        //Below we find objects and components we need via their name


        arrowObject = gameObject.transform.Find("Arrow").gameObject;
        arrowMat = arrowObject.GetComponent<MeshRenderer>().material;

        ledParent = gameObject.transform.Find("2-LEDS").gameObject;

        projector = gameObject.transform.Find("3-PROJECTOR").gameObject;
        if (landingPadObject != null) { projectorDecalMat = landingPadObject.GetComponent<MeshRenderer>()?.material; }

        projectorMat = projector.GetComponent<MeshRenderer>().materials[1];

        arrowObjectDefaultSize = arrowObject.transform.localScale;
        arrowObjectDefaultY = arrowObject.transform.localPosition.y;
    }

    // Propeller Methods
    private void UpdatePropellerRotation()
    {
        // Adjust current rotation speed with windup and winddown effect
        if (rotatePropellers)
        {
            currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, rotationSpeed, Time.deltaTime / windupTime);
        }
        else
        {
            currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, 0f, Time.deltaTime / winddownTime);
        }

        RotatePropellers();
    }

    public void RotatePropellers()
    {
        foreach (GameObject propeller in propellers)
        {
            if (propeller != null)
            {
                propeller.transform.Rotate(0f, 0f, currentRotationSpeed * 20f * Time.deltaTime, Space.Self); //This makes propellers rotate by themselves on Z axis indefinetely
            }
        }
    }

    // Screen Methods
    private void UpdateScreen()
    {

        screenParent.SetActive(screenIsEnabled);

        foreach (Material screenMat in screenMats) { 

        if (screenIsOn)
        {
            screenMat.SetColor("_EmissionColor", screenColor * screenLightIntensity);
        }
        else
        {
            screenMat.SetColor("_EmissionColor", Color.black * screenLightIntensity);
        }
        }

        // Updates screen orientation depending on our selection
        screenParent.transform.localEulerAngles = screenIsVertical ? new Vector3(0, 90, 0) : new Vector3(0, 0, 0);

        for (int i = 0; i < screens.Count; i++)
        {
            if (i == bezelSize)
            {
                screens[i].SetActive(true); // Enable the selected screen
            }
            else
            {
                screens[i].SetActive(false); // Disable other screens
            }

            screens[i].transform.localScale = Vector3.one * screenSizeMultiplier;
        }

    }

private void UpdateArrow()
    {
        // Update arrow visibility depending on the states of other screen toggles
        arrowObject.SetActive(arrowIsOn && screenIsEnabled && screenIsOn);
        arrowObject.transform.localScale = arrowObjectDefaultSize * screenSizeMultiplier;
        arrowObject.transform.localPosition = new Vector3(0, arrowObjectDefaultY + (screenSizeMultiplier / 10), 0);

        arrowMat.SetFloat("_arrowHeight", arrowHeadHeight);
        arrowMat.SetFloat("_arrowWidth", arrowHeadWidth);
        arrowMat.SetFloat("_bodyHeight", arrowBodyHeight);
        arrowMat.SetFloat("_bodyWidth", arrowBodyWidth);
        arrowMat.SetFloat("_animationSpeed", animationSpeed);
        arrowMat.SetFloat("_isAnimating", isAnimating ? 1.0f : 0.0f);
        arrowMat.SetFloat("_pointedUP", pointedUp ? 1.0f : 0.0f);
        arrowMat.SetColor("_arrowColor", arrowColor);
    }


    // LED Methods
private void UpdateLEDs()
{
    // Enable or disable the parent object of the LEDs
    ledParent.SetActive(ledsAreEnabled);

    // Iterate through the materials and set their emission color based on intensity
    for (int i = 0; i < ledMats.Count; i++)
    {
        float LEDLightIntensity = LEDIntensities[i]; // Get the corresponding intensity

        if (ledsAreOn)
        {
            ledMats[i].SetColor("_EmissionColor", LEDColor * LEDLightIntensity);
        }
        else
        {
            ledMats[i].SetColor("_EmissionColor", Color.black * LEDLightIntensity);
        }
    }

    // Update the scale of each LED GameObject
    foreach (GameObject led in leds)
    {
        led.transform.localScale = new Vector3(ledSize, ledSize, ledSize);
    }
}

    private void UpdateBlinkers()
    {
        // Ensure the parent object for blinkers is enabled or disabled
        blinkerParent.SetActive(blinkerLedsAreEnabled);

        if (blinkerLedsAreOn)
        {
            // Check if ascending
            if (isAscending)
            {
                // Blink the ascend blinker
                float blinkValue = Mathf.PingPong(Time.time * blinkingSpeed, 1f); // Creates a blinking effect (0 to 1)
                ascendBlinkerMat.SetColor("_EmissionColor", ascendLedColor * blinkValue * blinkerLedInstensity);

                // Turn off the descend blinker
                descendBlinkerMat.SetColor("_EmissionColor", Color.black * blinkerLedInstensity);
            }
            else
            {
                // Blink the descend blinker
                float blinkValue = Mathf.PingPong(Time.time * blinkingSpeed, 1f); // Creates a blinking effect (0 to 1)
                descendBlinkerMat.SetColor("_EmissionColor", descendLedColor * blinkValue * blinkerLedInstensity);

                // Turn off the ascend blinker
                ascendBlinkerMat.SetColor("_EmissionColor", Color.black * blinkerLedInstensity);
            }
        }
        else
        {
            // If blinkers are off, disable both
            ascendBlinkerMat.SetColor("_EmissionColor", Color.black * blinkerLedInstensity);
            descendBlinkerMat.SetColor("_EmissionColor", Color.black * blinkerLedInstensity);
        }
    }

    // Projector Methods
    private void UpdateProjector()
    {
        projector.SetActive(projectorEnabled);

        if (projectorIsOn)
        {

            Color emissionColor = LEDColor * 2;
            projectorMat.SetColor("_EmissionColor", emissionColor);
        }
        else
        {
            projectorMat.SetColor("_EmissionColor", Color.black * 2);
        }
    }

    private void UpdateProjection()
    {
        if(landingPadObject == null)
        {  return; }

        // Update projector position and scale based on drone altitude
        landingPadObject.transform.position = new Vector3(
            gameObject.transform.position.x,
            groundObject.transform.position.y + 0.1f,
            projector.transform.position.z
        );

        if (lightPhysics)
        {
            landingPadObject.transform.localScale = new Vector3(
                gameObject.transform.position.y,
                gameObject.transform.position.y,
                gameObject.transform.position.y
            ) / 10 * projectionSizeMultiplier;
        }
        else
        {
            landingPadObject.transform.localScale = Vector3.one * projectionSizeMultiplier;
        }

        if (projectorIsOn)
        {
            landingPadObject.SetActive(true);

            float altitude = gameObject.transform.position.y;
            float opacity = Mathf.Clamp01(1f - (altitude / minOpacityAltitude)); // Calculate opacity with our exposed altitude and minOpacity values

            // Adjust opacity value of the projection
            projectorDecalMat.SetFloat("_masterOpacity", Mathf.Lerp(minOpacity, 1f, opacity));
        }
        else
        {
            landingPadObject.SetActive(false);
        }
    }

    private void FindNestedChildren(Transform parent, string targetName, List<GameObject> resultList)
    {
        foreach (Transform child in parent)
        {
            if (child.name == targetName)
            {
                resultList.Add(child.gameObject);
            }

            // Recursively search in the child's children
            FindNestedChildren(child, targetName, resultList);
        }
    }


}