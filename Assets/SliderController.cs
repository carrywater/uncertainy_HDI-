using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SliderController : MonoBehaviour
{
    public string comPort = "COM3"; // Change this to match your Arduino's COM port.
    public int baudRate = 9600; // Baud rate (bits per second).
    public float readInterval = 0.01f; // Adjust this interval as needed.

    private SerialPort serialPort;
    private Slider slider;
    private float lastReadTime;
    private bool isReading;

    private void Start()
    {
        slider = GameObject.Find("Slider1").GetComponent<Slider>(); // Find the Slider in the scene by name.

        // Initialize the serial port.
        serialPort = new SerialPort(comPort, baudRate);

        // Open the serial port.
        //serialPort.Open();
        //OpenSerialPort();

        lastReadTime = Time.time;
        isReading = false;

        Application.quitting += OnApplicationQuit; // Subscribe to the quitting event.
        //SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

/*     private void OpenSerialPort()
    {
        try
        {
            // Open the serial port.
            serialPort.Open();
            Debug.Log("Serial port opened successfully.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error opening serial port: {ex.Message}");
        }
    } */


    private void Update()
{
    if (serialPort != null && !isReading)
    {
/*         // Check if the 'B' key is pressed to close the serial port.
        if (Input.GetKeyDown(KeyCode.B))
        {
            CloseSerialPort();
            return; // No need to continue checking for serial data if the port is closed.
        } */

        // Check if the port is closed and open it.
        if (!serialPort.IsOpen)
        {
            OpenSerialPort();
        }

        // Check if enough time has passed since the last read.
        if (Time.time - lastReadTime >= readInterval)
        {
            // Start reading data asynchronously to avoid blocking the main thread.
            isReading = true;
            ReadSerialDataAsync();
        }
    }
}

private void OpenSerialPort()
{
    try
    {
        // Open the serial port.
        serialPort.Open();
        Debug.Log("Serial port opened successfully.");
    }
    catch (System.Exception ex)
    {
        Debug.LogError($"Error opening serial port: {ex.Message}");
    }
}



    private async void ReadSerialDataAsync()
    {
        // Read the data from the Arduino asynchronously.
        string data = await Task.Run(() => serialPort.ReadLine());

        // Parse the received data as an integer.
        if (int.TryParse(data, out int mappedValue))
        {
            // Update the UI Slider's value with the mapped value.
            slider.value = mappedValue;
        }

        // Update the last read time and reset isReading.
        lastReadTime = Time.time;
        isReading = false;
    }

    /* private void OnSceneUnloaded(Scene scene)
    {
        // Close the serial port when the scene is unloaded.
        CloseSerialPort();
    } */

    private void OnApplicationQuit()
    {
        // Close the serial port when the application quits.
        CloseSerialPort();
    }

    public void CloseSerialPort()
    {
        // This public method calls the private CloseSerialPort.
        CloseSerialPortInternal();
    }

     private void CloseSerialPortInternal()
    {
        // Close the serial port.
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Serial port closed successfully.");
        }
    }
}
