using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class QuestionnaireData : MonoBehaviour
{
    public TMP_Text questionText; // Reference to the TMP text element displaying the question.
    public Slider slider; // Reference to the UI slider element.
    public Button nextButton; // Reference to the "Next" button.
    public Button submitButton; // Reference to the "Submit" button.
    public Button nextSceneButton; // Reference to the button that transitions to the next scene.
    public Button backButton; // Reference to the "Back" button.
    public GameObject currentScenePrefab; // Reference to the current scene prefab

    private SceneTransitionManager sceneTransitionManager;
    private int participantID;

    private List<float> sliderValues = new List<float>();
    private int currentQuestionIndex = 0;
    private bool isLastQuestion = false; // Flag to track if it's the last question.

    private List<string> questions = new List<string>
    {
        "1: I felt uncertain about what the drone was going to do (in terms of behaviour).",
        "2: The drone behaviour was always clear to me.",
        "3: I was able to understand why things happended (in terms of the way the drone behaved).",
        "4: The drone behaved unpredictably.",
        "5: It was difficult to identify what the drone will do next.",
        "6: I felt certain about what the drone was going to do (in terms of behaviour).",
        "7: I trusted the drone behaviour.",
        "8: I relied on the drone behaviour.",
        "Thank you! Please press submit to record your responses and then notify the researcher."
    };

    public string scenarioIdentifier = "1"; // Specify the scenario identifier here.

    private void Start()
    {
        nextButton.onClick.AddListener(NextQuestion);
        submitButton.onClick.AddListener(SaveDataAndActivateNextSceneButton);
        backButton.onClick.AddListener(PreviousQuestion);
        sceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
        participantID = sceneTransitionManager.PID();

        nextButton.gameObject.SetActive(true); // Show the "Next" button initially.
        submitButton.gameObject.SetActive(false); // Hide the "Submit" button initially.
        nextSceneButton.gameObject.SetActive(false); // Hide the scene transition button initially.
        backButton.gameObject.SetActive(false); // Hide the "Back" button initially.

        // Initialize the first question.
        SetQuestion(0);
    }

    private void NextQuestion()
    {
        // Check if it's the last question before proceeding.
        if (isLastQuestion)
        {
            // All questions answered, show the "Submit" button.
            nextButton.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(true);

            // Deactivate the slider when displaying the final message.
            slider.gameObject.SetActive(false);
            
            // Hide the "Back" button.
            backButton.gameObject.SetActive(false);
        }
        else
        {
            // Record the slider value for the current question.
            float sliderValue = slider.value;
            if (currentQuestionIndex < sliderValues.Count)
            {
                sliderValues[currentQuestionIndex] = sliderValue;
            }
            else
            {
                sliderValues.Add(sliderValue);
            }

            // Reset the slider.
            slider.value = 3f;

            // Move to the next question.
            currentQuestionIndex++;

            // Check if all questions have been answered.
            if (currentQuestionIndex < questions.Count - 1)
            {
                SetQuestion(currentQuestionIndex);
                
                // Show the "Back" button for questions 2 and beyond.
                backButton.gameObject.SetActive(currentQuestionIndex >= 1);
            }
            else
            {
                // Flag that it's the last question.
                isLastQuestion = true;

                // Set the last question text.
                SetQuestion(currentQuestionIndex);

                nextButton.gameObject.SetActive(false); // Hide the "Next" button initially.
                submitButton.gameObject.SetActive(true); // Show the "Submit" button initially.
                nextSceneButton.gameObject.SetActive(false); // Hide the scene transition button initially.
                backButton.gameObject.SetActive(false); // Hide the "Back" button for the "Thank you" message.

                slider.gameObject.SetActive(false);
            }
        }
    }

    private void PreviousQuestion()
    {
        if (currentQuestionIndex > 0)
        {
            currentQuestionIndex--;

            // Display the previous question.
            SetQuestion(currentQuestionIndex);

            // Enable the "Next" button again.
            nextButton.gameObject.SetActive(true);

            // Disable the "Submit" button.
            submitButton.gameObject.SetActive(false);

            // Show the "Back" button for questions other than the first one.
            backButton.gameObject.SetActive(currentQuestionIndex > 0);

            // Set the slider value to the previous response.
            if (currentQuestionIndex < sliderValues.Count)
            {
                slider.value = sliderValues[currentQuestionIndex];
            }
            else
            {
                slider.value = 3f; // If no previous response is available, set the slider to the default value.
            }
        }
    }

    private void SetQuestion(int index)
    {
        // Set the question text.
        questionText.text = questions[index];
    }
    
    private void SaveDataAndActivateNextSceneButton()
    {
        // Create a CSV-formatted string with the collected data.
        string csvData = "Question,Slider Value,Scenario\n";

        for (int i = 0; i < questions.Count - 1; i++)
        {
            csvData += $"{questions[i]},{sliderValues[i]},{scenarioIdentifier}\n";
        }

        // Get the current scene's name.
        string sceneName = currentScenePrefab != null ? currentScenePrefab.name : "UnknownScenePrefab";

        // Get the current timestamp as a string.
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd.HH.mm.ss");

        // Construct the file name using the scene name and timestamp.
        string fileName = $"Questionnaire_{participantID}_{sceneName}_{timestamp}.csv";

        // Define the folder path within the dataPath where you want to save the data.
        string folderPath = Path.Combine(Application.dataPath, "Questionnaire data");

        // Create the full file path by combining the folder path and file name.
        string filePath = Path.Combine(folderPath, fileName);

        // Write the CSV data to the file.
        File.WriteAllText(filePath, csvData);

        // Log a message indicating that the data has been saved.
        Debug.Log($"Questionnaire data saved to: {filePath}");

        // Deactivate the "Submit" button.
        submitButton.gameObject.SetActive(false);

        // Activate the button for transitioning to the next scene.
        nextSceneButton.gameObject.SetActive(true);
    }
}