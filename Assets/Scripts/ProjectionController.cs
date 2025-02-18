using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionController : MonoBehaviour
{

    public GameObject landingPadReference;

    [Header("Projection Settings")]

    [Range(0.0f, 1.0f)] public float outerRingSize = 1f;
    [Range(0.0f, 0.5f)] public float outerRingThickness = .1f;
    public Color outerRingColor = Color.blue;

    [Range(0.0f, 1.0f)] public float innerRingSize = .25f;
    [Range(0.0f, 1.0f)] public float innerRingThickness = .5f;
    public Color innerRingColor = Color.blue;

    public bool showHLeter = false;
    [Range(-2.0f, 3.0f)] public float sizeOfH = 3f;
    public Color colorOfH = Color.white;

    public bool animatedRingsAreEnabled = true;
    [Range(0.0f, 10.0f)] public float animationSpeed = 2f;
    public bool outwardsAnimation = false;
    [Range(0.0f, 0.2f)] public float animatedRingsThickness = .05f;
    [Range(0.0f, 1.0f)] public float animatedRingsSpacing = .4f;
    public Color animatedRingsColor = Color.white;

    private Material projectionMat;

    private void Start()
    {
        projectionMat = landingPadReference.GetComponent<MeshRenderer>().material;
    }

    public void Update()
    {
        UpdateProjection();
    }

    private void UpdateProjection()
    {
        projectionMat.SetFloat("_ring1L", outerRingSize);
        projectionMat.SetFloat("_ring1T", outerRingThickness);
        projectionMat.SetColor("_ring1C", outerRingColor);

        projectionMat.SetFloat("_ring2L", innerRingSize);
        projectionMat.SetFloat("_ring2T", innerRingThickness);
        projectionMat.SetColor("_ring2C", innerRingColor);

        projectionMat.SetFloat("_showH", showHLeter ? 1.0f : 0.0f);
        projectionMat.SetFloat("_sizeH", 4f - sizeOfH);
        projectionMat.SetColor("_colorH", colorOfH);

        projectionMat.SetFloat("_animatedRingsOn", animatedRingsAreEnabled ? 1.0f : 0.0f);
        projectionMat.SetFloat("_animSpeed", animationSpeed);
        projectionMat.SetFloat("_outwardsAnimation", outwardsAnimation ? 1.0f : 0.0f);
        projectionMat.SetFloat("_animRingT", animatedRingsThickness);
        projectionMat.SetFloat("_animRingS", animatedRingsSpacing);
        projectionMat.SetColor("_animRingC", animatedRingsColor);


    }
}
