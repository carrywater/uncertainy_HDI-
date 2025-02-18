using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using System.ComponentModel.Composition.Primitives;
using Unity.Mathematics;
using UnityEngine.Rendering;

public class Beh_record : MonoBehaviour
{
    private Vector3 previousPosition;
    private Vector3 cameraPosition;
    private Vector3 cameraRotation;
    private float observationInterval = 0.01f; // Observe every 1/100th of a second
    private float lastObservationTime;
    private int participantID;
    private List<string> logMessages = new List<string>();
    private Slider slider; // Reference to your slider
    public int scenarioIdentifier = 1; // Specify the scenario identifier here
    public BaseS s2; // Reference to the s2.
    private SceneTransitionManager sceneTransitionManager;
    public BaseDroneController droneController;
    public GameObject currentScenePrefab; // Reference to the current scene prefab

    private void Start()
    {
        previousPosition = transform.position; // Set the initial position.
        slider = GameObject.Find("Slider1").GetComponent<Slider>();
        if (slider == null)
        {
            Debug.LogError("SliderS1 GameObject not found or does not have a Slider component!");
        }
        lastObservationTime = Time.time;

    }

    private void Update()
    {
        // Calculate the time since the last observation.
        float elapsedTime = Time.time - lastObservationTime;

        // Check if it's time for a new observation.
        if (elapsedTime >= observationInterval)
        {
            // Record the timestamp.
            lastObservationTime = Time.time;

            sceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
            participantID = sceneTransitionManager.PID();

            // Calculate the speed as the change in position over a fixed time interval.
            float speed;

            if ((scenarioIdentifier == 2 || scenarioIdentifier == 4) && s2.currentState == BaseS.DroneState.Approach1)
            {
                // Use s2's moveSpeed for the speed.
                speed = s2.moveSpeed;
            }
            else
            {
                // Access the speed from BaseDroneController.
                speed = droneController.GetSpeed();
            }

            // Get the current slider value.
            float sliderValue = (slider != null) ? slider.value : 0f;

            // Access the current state from the Scenario s2.
            BaseS.DroneState currentState = s2.currentState;

            cameraPosition = Camera.main.transform.position;
            Quaternion cameraRotate = Camera.main.transform.rotation;
            cameraRotation = cameraRotate.eulerAngles;

            // Separate drone position into x, y, and z coordinates.
            float droneX = transform.position.x;
            float droneY = transform.position.y;
            float droneZ = transform.position.z;

            // Separate camera position into x, y, and z coordinates.
            float cameraX = cameraPosition.x;
            float cameraY = cameraPosition.y;
            float cameraZ = cameraPosition.z;

            // Separate camera rotation into x, y, and z angles.
            float cameraRotationX = cameraRotation.x;
            float cameraRotationY = cameraRotation.y;
            float cameraRotationZ = cameraRotation.z;

            // Create a log message with timestamp, position, speed, and slider value.
            //string logMessage = $"ID:{participantID}, Timestamp:{Time.time}, DronePosition:{transform.position}, Speed:{speed}, CameraPosition:{cameraPosition}, CameraRotation:{cameraRotation}, SliderValue:{sliderValue}, Phase:{currentState}, {scenarioIdentifier}";
            string logMessage = $"ID,{participantID},Time,{Time.time},DronePosition,{droneX},{droneY},{droneZ},{speed},CameraPosition,{cameraX},{cameraY},{cameraZ},CameraRotation,{cameraRotationX},{cameraRotationY},{cameraRotationZ},CurrentState,{currentState},Slider,{sliderValue},Scenario,{scenarioIdentifier}";

            // Print the log message to the console.
            Debug.Log(logMessage);

            // Add the log message to the list.
            logMessages.Add(logMessage);

            // Update previousPosition for the next observation.
            previousPosition = transform.position;
        }
    }

    private void OnDestroy()
    {
        // Save the data to a CSV file when the scene is destroyed.
        ExportLogData();
    }

    private void ExportLogData()
    {
        // Generate a unique file name based on the scene name and timestamp.
        string sceneName = currentScenePrefab != null ? currentScenePrefab.name : "UnknownScenePrefab";
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd.HH.mm.ss");
        string fileName = $"Slider_{participantID}_{sceneName}_{timestamp}.csv"; // Modified this line

        // Define the folder path within the dataPath where you want to save the data.
        string folderPath = Path.Combine(Application.dataPath, "Behaviour data");

        // Ensure the folder exists, or create it if it doesn't.
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Define the full file path, combining the folder path and file name.
        string filePath = Path.Combine(folderPath, fileName);

        // Write the log messages to the CSV file.
        File.WriteAllLines(filePath, logMessages.ToArray());

        // Print a message to the Unity console indicating the export is complete.
        Debug.Log("Log data exported to CSV: " + filePath);
    }
}
