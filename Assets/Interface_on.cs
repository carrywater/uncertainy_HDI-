using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interface_on : MonoBehaviour
{
    // Duration for the LEDs to stay on and off in seconds
    [SerializeField] private float onTime = 1.0f;
    [SerializeField] private float offTime = 1.0f;

    // Lists of GameObjects for "on" and "off" states
    [SerializeField] private List<GameObject> onLEDs;
    [SerializeField] private List<GameObject> offLEDs;

    // Coroutine to handle the LED blinking
    private Coroutine ledCoroutine;

    private void Start()
    {
        // Start the coroutine for blinking LEDs
        ledCoroutine = StartCoroutine(ToggleLEDs());
    }

    private IEnumerator ToggleLEDs()
    {
        while (true)
        {
            // Turn on designated LEDs and turn off the others
            SetLEDState(true);
            yield return new WaitForSeconds(onTime);

            // Turn off designated LEDs and turn on the others
            SetLEDState(false);
            yield return new WaitForSeconds(offTime);
        }
    }

    // Method to set the active state of the "on" and "off" LED lists
    private void SetLEDState(bool isOn)
    {
        // Activate/deactivate objects in the "onLEDs" list based on the current state
        foreach (GameObject led in onLEDs)
        {
            led.SetActive(isOn);
        }

        // Activate/deactivate objects in the "offLEDs" list based on the opposite state
        foreach (GameObject led in offLEDs)
        {
            led.SetActive(!isOn);
        }
    }

    private void OnDisable()
    {
        // Stop the coroutine if the object is disabled
        if (ledCoroutine != null)
        {
            StopCoroutine(ledCoroutine);
        }
    }
}
