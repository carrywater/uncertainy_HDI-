using UnityEngine;

public class Beh_record_initiater : MonoBehaviour
{
    public Beh_record behRecordScript; // Reference to the Beh_record script.

    private bool scriptEnabled = false; // Indicates if the script is enabled.

    private void Start()
    {
        // Disable the Beh_record script at the beginning.
        behRecordScript.enabled = false;
    }

    private void Update()
    {
        // Check if the script should be enabled.
        if (!scriptEnabled && Time.time > 0.00001f)
        {
            // Enable the Beh_record script.
            behRecordScript.enabled = true;

            // Set the flag to indicate that the script is enabled.
            scriptEnabled = true;
        }
    }
}
