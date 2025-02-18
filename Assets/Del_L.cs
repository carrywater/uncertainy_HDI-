using UnityEngine;

public class Del_L : MonoBehaviour
{
    private Vector3 startPosition = new Vector3(7, 7, 0);
    private Vector3 targetPosition = new Vector3(7, 0, 0);
    public float moveDuration = 5.0f;

    private float startTime;
    private bool isMoving = false;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
        transform.position = startPosition;
        StartMove(); // Automatically start the movement
    }

    private void Update()
    {
        if (isMoving)
        {
            float elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / moveDuration);

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            if (t >= 1.0f)
            {
                isMoving = false;
                animator.enabled = false; // Disable the Animator component
            }
        }
    }

    private void StartMove()
    {
        startTime = Time.time;
        isMoving = true;
    }
}
