using UnityEngine;

public class Del_C : MonoBehaviour
{
    public Transform Package; // Reference to the child object named "Package"
    public Vector3 targetPosition = new Vector3(7, 0, 0);
    public float moveDuration = 5.0f;

    private Vector3 initialPackagePosition;
    private float startTime;
    private bool isMoving = false;

    private void Start()
    {
        initialPackagePosition = Package.position; // Use world position instead of localPosition
        Package.SetParent(null); // Detach the Package from its parent immediately
        StartMove(); // Automatically start the movement
    }

    private void Update()
    {
        if (isMoving)
        {
            float elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / moveDuration);

            Package.position = Vector3.Lerp(initialPackagePosition, targetPosition, t);

            if (t >= 1.0f)
            {
                isMoving = false;
            }
        }
    }

    public void StartMove()
    {
        startTime = Time.time;
        isMoving = true;
    }
}
