using UnityEngine;

public class SceneTransitionManager : MonoBehaviour
{
    public int participantID; // Input the participant ID in the inspector
    private int currentSceneIndex = 0;
    private GameObject currentSceneInstance;

    // Define the order of scenes (prefabs) for each participant
    private int[][] sceneOrders = new int[][]
    {
        new int[] {1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6},
        new int[] {1, 2, 4, 5, 3, 6, 4, 3, 1, 5, 2, 6, 5, 3, 2, 4, 1, 6},
        new int[] {4, 3, 1, 5, 2, 6, 5, 3, 2, 4, 1, 6, 2, 1, 5, 4, 3, 6},
        new int[] {5, 3, 2, 4, 1, 6, 2, 1, 5, 4, 3, 6, 4, 1, 3, 2, 5, 6},
        new int[] {2, 1, 5, 4, 3, 6, 4, 1, 3, 2, 5, 6, 3, 5, 4, 2, 1, 6},
        new int[] {4, 1, 3, 2, 5, 6, 3, 5, 4, 2, 1, 6, 2, 5, 1, 3, 4, 6},
        new int[] {3, 5, 4, 2, 1, 6, 2, 5, 1, 3, 4, 6, 1, 4, 2, 3, 5, 6},
        new int[] {2, 5, 1, 3, 4, 6, 1, 4, 2, 3, 5, 6, 3, 4, 5, 1, 2, 6},
        new int[] {1, 4, 2, 3, 5, 6, 3, 4, 5, 1, 2, 6, 5, 2, 3, 1, 4, 6},
        new int[] {3, 4, 5, 1, 2, 6, 5, 2, 3, 1, 4, 6, 1, 2, 4, 5, 3, 6},
        new int[] {5, 2, 3, 1, 4, 6, 1, 2, 4, 5, 3, 6, 4, 3, 1, 5, 2, 6},
        new int[] {1, 2, 4, 5, 3, 6, 4, 3, 1, 5, 2, 6, 5, 3, 2, 4, 1, 6},
        new int[] {4, 3, 1, 5, 2, 6, 5, 3, 2, 4, 1, 6, 2, 1, 5, 4, 3, 6},
        new int[] {5, 3, 2, 4, 1, 6, 2, 1, 5, 4, 3, 6, 4, 1, 3, 2, 5, 6},
        new int[] {2, 1, 5, 4, 3, 6, 4, 1, 3, 2, 5, 6, 3, 5, 4, 2, 1, 6},
        new int[] {4, 1, 3, 2, 5, 6, 3, 5, 4, 2, 1, 6, 2, 5, 1, 3, 4, 6},
        new int[] {3, 5, 4, 2, 1, 6, 2, 5, 1, 3, 4, 6, 1, 4, 2, 3, 5, 6},
        new int[] {2, 5, 1, 3, 4, 6, 1, 4, 2, 3, 5, 6, 3, 4, 5, 1, 2, 6},
        new int[] {1, 4, 2, 3, 5, 6, 3, 4, 5, 1, 2, 6, 5, 2, 3, 1, 4, 6},
        new int[] {3, 4, 5, 1, 2, 6, 5, 2, 3, 1, 4, 6, 1, 2, 4, 5, 3, 6},
        new int[] {5, 2, 3, 1, 4, 6, 1, 2, 4, 5, 3, 6, 4, 3, 1, 5, 2, 6},
        new int[] {1, 2, 4, 5, 3, 6, 4, 3, 1, 5, 2, 6, 5, 3, 2, 4, 1, 6},
        new int[] {4, 3, 1, 5, 2, 6, 5, 3, 2, 4, 1, 6, 2, 1, 5, 4, 3, 6},
        new int[] {5, 3, 2, 4, 1, 6, 2, 1, 5, 4, 3, 6, 4, 1, 3, 2, 5, 6},
        new int[] {2, 1, 5, 4, 3, 6, 4, 1, 3, 2, 5, 6, 3, 5, 4, 2, 1, 6},
        new int[] {4, 1, 3, 2, 5, 6, 3, 5, 4, 2, 1, 6, 2, 5, 1, 3, 4, 6},
        new int[] {3, 5, 4, 2, 1, 6, 2, 5, 1, 3, 4, 6, 1, 4, 2, 3, 5, 6},
        new int[] {2, 5, 1, 3, 4, 6, 1, 4, 2, 3, 5, 6, 3, 4, 5, 1, 2, 6},
        new int[] {1, 4, 2, 3, 5, 6, 3, 4, 5, 1, 2, 6, 5, 2, 3, 1, 4, 6},
        new int[] {3, 4, 5, 1, 2, 6, 5, 2, 3, 1, 4, 6, 1, 2, 4, 5, 3, 6},
        new int[] {5, 2, 3, 1, 4, 6, 1, 2, 4, 5, 3, 6, 4, 3, 1, 5, 2, 6},
        new int[] {1, 2, 4, 5, 3, 6, 4, 3, 1, 5, 2, 6, 5, 3, 2, 4, 1, 6},
        new int[] {4, 3, 1, 5, 2, 6, 5, 3, 2, 4, 1, 6, 2, 1, 5, 4, 3, 6},
        new int[] {5, 3, 2, 4, 1, 6, 2, 1, 5, 4, 3, 6, 4, 1, 3, 2, 5, 6},
        new int[] {2, 1, 5, 4, 3, 6, 4, 1, 3, 2, 5, 6, 3, 5, 4, 2, 1, 6},
        new int[] {4, 1, 3, 2, 5, 6, 3, 5, 4, 2, 1, 6, 2, 5, 1, 3, 4, 6},
        new int[] {3, 5, 4, 2, 1, 6, 2, 5, 1, 3, 4, 6, 1, 4, 2, 3, 5, 6},
        new int[] {2, 5, 1, 3, 4, 6, 1, 4, 2, 3, 5, 6, 3, 4, 5, 1, 2, 6},
        new int[] {1, 4, 2, 3, 5, 6, 3, 4, 5, 1, 2, 6, 5, 2, 3, 1, 4, 6},
        new int[] {3, 4, 5, 1, 2, 6, 5, 2, 3, 1, 4, 6, 1, 2, 4, 5, 3, 6},
        new int[] {5, 2, 3, 1, 4, 6, 1, 2, 4, 5, 3, 6, 4, 3, 1, 5, 2, 6},
        new int[] {1, 2, 4, 5, 3, 6, 4, 3, 1, 5, 2, 6, 5, 3, 2, 4, 1, 6},
        new int[] {4, 3, 1, 5, 2, 6, 5, 3, 2, 4, 1, 6, 2, 1, 5, 4, 3, 6},
        new int[] {5, 3, 2, 4, 1, 6, 2, 1, 5, 4, 3, 6, 4, 1, 3, 2, 5, 6},
        new int[] {2, 1, 5, 4, 3, 6, 4, 1, 3, 2, 5, 6, 3, 5, 4, 2, 1, 6},
        new int[] {4, 1, 3, 2, 5, 6, 3, 5, 4, 2, 1, 6, 2, 5, 1, 3, 4, 6},
        new int[] {3, 5, 4, 2, 1, 6, 2, 5, 1, 3, 4, 6, 1, 4, 2, 3, 5, 6},
        new int[] {2, 5, 1, 3, 4, 6, 1, 4, 2, 3, 5, 6, 3, 4, 5, 1, 2, 6},
        new int[] {1, 4, 2, 3, 5, 6, 3, 4, 5, 1, 2, 6, 5, 2, 3, 1, 4, 6},
        new int[] {3, 4, 5, 1, 2, 6, 5, 2, 3, 1, 4, 6, 1, 2, 4, 5, 3, 6},
        new int[] {5, 2, 3, 1, 4, 6, 1, 2, 4, 5, 3, 6, 4, 3, 1, 5, 2, 6},
        new int[] {1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6}
        // Add more arrays for other participant IDs if needed
    };



    public GameObject[] scenePrefabs; // Assign the prefabs for all participants

    void Start()
    {
        LoadNextScene();
    }

     void Update()
    {
        // Example: Press 'N' to load the next scene
        if (Input.GetKeyDown(KeyCode.N))
        {
            CloseSerialPortAndLoadNextScene();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            CloseSerialPortAndLoadPreviousScene();
        }
    }

    void CloseSerialPortAndLoadNextScene()
    {
        // Load the next scene
        LoadNextScene();
    }

    void CloseSerialPortAndLoadPreviousScene()
{
    // Load the previous scene
    LoadPreviousScene();
}

    void LoadNextScene()
    {
        // Check if there are scenes left for the current participant
        if (currentSceneIndex < sceneOrders[participantID - 1].Length)
        {
            int sceneNumber = sceneOrders[participantID - 1][currentSceneIndex];
            GameObject scenePrefab = scenePrefabs[sceneNumber - 1]; // Assuming scene numbers start from 1

            if (scenePrefab != null)
            {
                // Destroy the current prefab instance if it exists
                if (currentSceneInstance != null)
                {
                    Destroy(currentSceneInstance);
                }

                // Instantiate the new prefab
                currentSceneInstance = Instantiate(scenePrefab);

                Debug.Log($"Loading prefab: {scenePrefab.name}"); // Print prefab name to console for debugging

                currentSceneIndex++;
            }
            else
            {
                Debug.LogError($"Prefab not assigned for scene {sceneNumber}");
            }
        }
        else
        {
            Debug.Log("All prefabs for this participant have been loaded.");
        }
    }

    void LoadPreviousScene()
    {
        // Check if there are scenes to go back to for the current participant
        if (currentSceneIndex > 0)
        {
            currentSceneIndex--;

            int sceneNumber = sceneOrders[participantID - 1][currentSceneIndex];
            GameObject scenePrefab = scenePrefabs[sceneNumber - 1]; // Assuming scene numbers start from 1

            if (scenePrefab != null)
            {
                // Destroy the current prefab instance if it exists
                if (currentSceneInstance != null)
                {
                    Destroy(currentSceneInstance);
                }

                // Instantiate the new prefab
                currentSceneInstance = Instantiate(scenePrefab);

                Debug.Log($"Loading prefab: {scenePrefab.name}"); // Print prefab name to console for debugging
            }
            else
            {
                Debug.LogError($"Prefab not assigned for scene {sceneNumber}");
            }
        }
        else
        {
            Debug.Log("Already at the first scene for this participant.");
        }
    }

    public int PID()
    {
        return participantID;
    }
}
