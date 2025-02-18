using UnityEngine;
using UnityEngine.UI;

public class SliderRest : MonoBehaviour
{
    public Slider slider;
    public float resetDelay = 0.2f; // Time in seconds to wait before resetting

    private float lastInteractionTime;

    private void Start()
    {
        // Initialize the last interaction time to the current time.
        lastInteractionTime = Time.time;

        slider.onValueChanged.AddListener(UpdateTime);
    }

    private void UpdateTime(float time) 
    {
        lastInteractionTime = Time.time;
        Debug.Log("Updated interaction time");
    }

    private void Update()
    {
            // Check if it's time to reset the slider.
            if (Time.time >= lastInteractionTime + resetDelay)
            {
                // Reset the slider to zero.
                slider.value = 0.0f;
            }
    }
}
