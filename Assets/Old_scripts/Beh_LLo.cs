using UnityEngine;

public class Beh_LLo : MonoBehaviour
{
    public AnimationCurve xTrajectoryCurve; // Keyframes = [(0, 207), (0.5, 207), (1, 7)]
    public AnimationCurve yTrajectoryCurve; // Keyframes = [(0, 207), (0.5, 7), (1, 7)]
    public float moveSpeed = 0.02775f; // Adjust this value for speed
    public Vector3 targetCoordinates = new Vector3(7f, 7f, 0f);

    private float normalizedTime = 0f;

    void Update()
    {
        normalizedTime += Time.deltaTime * moveSpeed;

        float xOffset = xTrajectoryCurve.Evaluate(normalizedTime);
        float yOffset = yTrajectoryCurve.Evaluate(normalizedTime);

        Vector3 newPosition = new Vector3(xOffset, yOffset, 0);
        transform.position = newPosition;

        if (Vector3.Distance(transform.position, targetCoordinates) < 0.01f)
        {
            enabled = false; // Disable this script when target is reached
        }
    }
}



//trial to move forward
//{
//    public vector3 targetcoordinates;
//    public float movespeed = 11.1f;
//    // start is called before the first frame update
//    void start()
//    {

//    }

//    // update is called once per frame
//    void fixedupdate()
//    {
//        transform.position = vector3.movetowards(transform.position, targetcoordinates, movespeed * time.deltatime);
//    }
//}






