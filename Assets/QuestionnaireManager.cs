using UnityEngine;
using UnityEngine.UI;

public class QuestionnaireManager : MonoBehaviour
{
    public GameObject drone; // Reference to your drone GameObject.
    public Transform finalTarget; // Reference to the final target position.
    public GameObject questionnaireCanvas; // Reference to your Questionnaire Canvas.
    private GameObject Package; // Reference to the Slider Measure Canvas.
    //private GameObject sliderMeasureCanvas; // Reference to the Slider Measure Canvas.
    public AudioSource droneAudioSource; // Reference to the AudioSource component of the drone.

    private bool droneReachedFinalTarget = false;

    private void Start()
    {
        questionnaireCanvas.SetActive(false); // Initially, disable the questionnaire canvas.
        Package = GameObject.Find("Package");
        Package.SetActive(true);
        //sliderMeasureCanvas = GameObject.Find("SliderMeasSliderCanvas");
        //sliderMeasureCanvas.SetActive(true); // Initially, enable the slider measure canvas.
    }

    private void Update()
    {
        if (!droneReachedFinalTarget && Vector3.Distance(drone.transform.position, finalTarget.position) < 0.5f)
        {
            // Drone has reached the final target position.
            droneReachedFinalTarget = true;
            ShowQuestionnaire();
            DisableDroneSound();
            //DisableSliderMeasureCanvas();
            //DisableSliderMeasureCanvas();
        }
    }

    private void ShowQuestionnaire()
    {
        // Enable the Questionnaire Canvas.
        questionnaireCanvas.SetActive(true);
    }

    private void DisableDroneSound()
    {
        // Disable the drone's audio source.
        if (droneAudioSource != null)
        {
            droneAudioSource.enabled = false;
        }
    }

    /* private void DisableSliderMeasureCanvas()
    {
        // Disable the Slider Measure Canvas.
        sliderMeasureCanvas.SetActive(false);
    } */
}
