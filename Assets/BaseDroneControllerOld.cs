using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDroneControllerOld : MonoBehaviour
{
    public float acceleration;
    public float maxSpeed;
    public Vector3 direction;

    [Header("Drone Visual Settings")]
    [SerializeField] private GameObject droneVisual;
    [SerializeField] private Vector3 minOrientation;
    [SerializeField] private Vector3 maxOrienration;

    [Header("Sound Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    
    //private bool inTween = false;

    public Vector3 velocity = Vector3.zero;
    public float Speed => velocity.magnitude;

    private void FixedUpdate() {      
        TruncateVelocity();
        UpdateAudioPitch();
        //Accelerate(direction);

        transform.position += velocity * Time.fixedDeltaTime;
    }

    public void Accelerate(Vector3 direction) {
        direction.Normalize();
        velocity += direction * acceleration;
        //Debug.Log("accelerating");

        AdjustPitch();
    }

    public void Deccelerate(float deccelerationRate = 0) {
        velocity -= Vector3.one.normalized * (deccelerationRate == 0 ? acceleration: deccelerationRate);
        if(velocity.magnitude <= .1f && velocity != Vector3.zero)
            velocity = Vector3.zero;

        AdjustPitch();
    }

    public void Stop() {
        velocity = Vector3.zero;
        AdjustPitch();
    }

    private void AdjustPitch() {
        droneVisual.transform.rotation = Quaternion.Euler(
            minOrientation.x + (maxOrienration.x - minOrientation.x) * (velocity.magnitude / maxSpeed),
            droneVisual.transform.rotation.eulerAngles.y,
            droneVisual.transform.rotation.eulerAngles.z
        );
    }




    private void UpdateAudioPitch() {
        audioSource.pitch = minPitch + ((maxPitch - minPitch) * (velocity.magnitude / maxSpeed));
    }

    private void TruncateVelocity() {
        if(velocity.sqrMagnitude <= maxSpeed * maxSpeed)
            return;

        velocity.Normalize();
        velocity *= maxSpeed;
        //Debug.Log("truncatevelocity");
    }
}
