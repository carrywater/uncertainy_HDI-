using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{
    public LineRenderer line;
    private Transform pos1; // Drone
    private Transform pos2; // Pseudo
    private Vector3 targetPosition = new Vector3(-110.239f, 0.114f, -107.379f);

    private bool isRetracting = false;
    private Vector3 initialPos2;
    private float retractStartTime;

    // Start is called before the first frame update
    void Start()
    {
        line.positionCount = 2;
        // Use GameObject.Find to locate the objects in the scene
        pos1 = GameObject.Find("Drone27102023").transform;
        initialPos2 = new Vector3(-110.239f, 7f, -107.379f);
        retractStartTime = Time.time + 7f; // Start retracting after 7 seconds
    }

    // Update is called once per frame
    void Update()
    {
        // Find the objects every frame in case they are destroyed or created
        pos1 = GameObject.Find("Drone27102023").transform;
        pos2 = GameObject.Find("Pseudo").transform;

        if (pos2 != null)
        {
            if (!isRetracting && pos2.position == targetPosition && Time.time >= retractStartTime)
            {
                isRetracting = true;
            }

            if (isRetracting)
            {
                float normalizedTime = (Time.time - retractStartTime) / 7f; // 7 seconds duration
                pos2.position = Vector3.Lerp(new Vector3(pos1.transform.position.x, 0.114f, pos1.transform.position.z), new Vector3(pos1.transform.position.x, 7f, pos1.transform.position.z), normalizedTime);

                line.SetPosition(0, pos1.position);

                // Check if pos2 is not null before accessing its position
                if (pos2 != null)
                {
                    line.SetPosition(1, pos2.position);
                }

                if (pos2.position == new Vector3(pos1.transform.position.x, 7f, pos1.transform.position.z))
                {
                    isRetracting = false;
                }
            }
            else
            {
                line.SetPosition(0, pos1.position);

                // Check if pos2 is not null before accessing its position
                if (pos2 != null)
                {
                    line.SetPosition(1, pos2.position);
                }
            }
        }
    }
}
