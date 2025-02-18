using UnityEngine;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour
{
    public Slider slider;
    public float initialValue = 0f; // Define the initial value in the inspector.


    private void Start()
    {
        // Attach a listener to the slider's value changed event.
        //slider.onValueChanged.AddListener(OnSliderValueChanged);

        // Set the initial value to 0 (middle).
        slider.value = initialValue;
    }

    // This method is called when the slider's value changes.
    //private void OnSliderValueChanged(float value)
    //{
        // The 'value' variable now ranges from -1 to 1.
        // You can use this value as needed in your game.
        //Debug.Log("Slider Value: " + value);
    //}
}
