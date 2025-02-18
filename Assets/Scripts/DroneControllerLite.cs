using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneControllerLite : MonoBehaviour
{

    [Header("Propeller Settings")]
    public bool rotatePropellers = false;
    [Range(0.0f, 100.0f)] public float rotationSpeed = 1f;
    [Range(0.0f, 1f)] public float windupTime = 2f;
    [Range(0.0f, 1f)] public float winddownTime = 2f;
    private float currentRotationSpeed = 0f;
    private List<GameObject> propellers = new List<GameObject>();
    
    [Header("LED Settings")]
    public bool ledsAreOn = false;
    public Color LEDColor; //Base color of LEDS
    [Range(0f, 100f)] public List<float> LEDIntensities = new List<float> { 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f };
    [Range(1f, 3f)] public float ledSize = 1f;
    private GameObject ledParent;
    private List<GameObject> leds = new List<GameObject>();
    private List<Material> ledMats = new List<Material>();

    [Header("Blinker LED Settings")]
    public bool isAscending = false;
    public bool isDescending = false;
    [Range(0, 10)] public float blinkingSpeed = 2f;
    public bool useAlphaInstead = false;
    public Color ascendLedColor;
    public Color descendLedColor;
    [Range(0, 40f)] public float blinkerLedInstensity = 1f;
    private List<GameObject> blinkerLeds = new List<GameObject>();
    private GameObject blinkerParent;
    private Material ascendBlinkerMat;
    private Material descendBlinkerMat;

    [Header("Projector Settings")]
    public bool lightPhysics = false;
    [Range(0.2f, 1f)] public float minOpacity = 0.2f;
    [Range(10f, 100f)] public float minOpacityAltitude = 20f;
    [Range(.1f, 4.0f)] public float projectionSizeMultiplier = 1f;
    public Transform groundObject;
    private GameObject projector;
    public GameObject landingPadObject;
    private Material projectorDecalMat;

    void Start()
    {
        GetReferences();
    }

    void Update()
    {
        UpdatePropellerRotation();
        UpdateLEDs();
        UpdateBlinkers();
        UpdateProjection();
    }

    public void GetReferences()
    {
        propellers.Clear();

        FindNestedChildren(transform, "Propeller", propellers);
        
        FindNestedChildren(transform, "Led", leds);
        
        foreach (GameObject led in leds) 
        {
            ledMats.Add(led.GetComponent<MeshRenderer>().materials[1]);
        }


        blinkerParent = gameObject.transform.Find("2.1-BLINKERLEDS").gameObject;

        foreach (Transform child in blinkerParent.transform)
        {
            blinkerLeds.Add(child.gameObject);
        }

        ledParent = gameObject.transform.Find("2-LEDS").gameObject;

        descendBlinkerMat = blinkerLeds[0].GetComponent<MeshRenderer>().materials[1];
        ascendBlinkerMat = blinkerLeds[1].GetComponent<MeshRenderer>().materials[1];

        projector = gameObject.transform.Find("3-PROJECTOR").gameObject;
        if (landingPadObject != null) { projectorDecalMat = landingPadObject.GetComponent<MeshRenderer>()?.material; }
    }

    private void UpdatePropellerRotation()
    {
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
                propeller.transform.Rotate(0f, 0f, currentRotationSpeed * 20f * Time.deltaTime, Space.Self);
            }
        }
    }


    private void UpdateLEDs()
    {

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

        if (!useAlphaInstead) { 
            if (isAscending)
            {
                float blinkValue = Mathf.PingPong(Time.time * blinkingSpeed, 1f);
                ascendBlinkerMat.SetColor("_EmissionColor", ascendLedColor * blinkValue * blinkerLedInstensity);
            }
            else { ascendBlinkerMat.SetColor("_EmissionColor", Color.black * blinkerLedInstensity); }

            if(isDescending)
            { 
                float blinkValue = Mathf.PingPong(Time.time * blinkingSpeed, 1f);
                descendBlinkerMat.SetColor("_EmissionColor", descendLedColor * blinkValue * blinkerLedInstensity);
            }
            else { descendBlinkerMat.SetColor("_EmissionColor", Color.black * blinkerLedInstensity); }
        }

        else
        {
            if (isAscending)
            {
                float blinkValue = Mathf.PingPong(Time.time * blinkingSpeed, 1f);
                ascendBlinkerMat.SetColor("_EmissionColor", ascendLedColor * blinkValue);
            }
            else { ascendBlinkerMat.SetColor("_EmissionColor", Color.black * blinkerLedInstensity); }

            if (isDescending)
            {
                float blinkValue = Mathf.PingPong(Time.time * blinkingSpeed, 1f);
                descendBlinkerMat.SetColor("_EmissionColor", descendLedColor * blinkValue);
            }
            else { descendBlinkerMat.SetColor("_EmissionColor", Color.black * blinkerLedInstensity); }
        }
    }


    private void UpdateProjection()
    {
        if (landingPadObject == null)
        { return; }

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

            float altitude = gameObject.transform.position.y;
            float opacity = Mathf.Clamp01(1f - (altitude / minOpacityAltitude));

            projectorDecalMat.SetFloat("_masterOpacity", Mathf.Lerp(minOpacity, 1f, opacity));

    }

    private void FindNestedChildren(Transform parent, string targetName, List<GameObject> resultList)
    {
        foreach (Transform child in parent)
        {
            if (child.name == targetName)
            {
                resultList.Add(child.gameObject);
            }
            FindNestedChildren(child, targetName, resultList);
        }
    }

}