using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Tweenables.Primitives;

public class BaseDroneController : MonoBehaviour
{
    public float acceleration;
    [SerializeField] private float maxSpeed;
    // public Vector3 maxError = Vector3.zero;
    // [SerializeField] private float maxErrorSpeed = 1f;

    [Header("Drone Visual Settings")]
    [SerializeField] private GameObject droneVisual;
    [SerializeField] private Vector3 minOrientation;
    [SerializeField] private Vector3 maxOrientation;

    [Header("Sound Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    
    //private bool inTween = false;

    private Vector3 velocity = Vector3.zero;
    public Vector3 Direction => velocity.normalized;
    public float Speed => velocity.magnitude;

    private void Update() {      
        TruncateVelocity();

        transform.position += velocity * Time.deltaTime;
    }

    public void Accelerate(Vector3 direction) {
        direction.Normalize();
        velocity += direction * acceleration * Time.deltaTime;
        UpdateAudioPitch();
    }

    public void Deccelerate(Vector3 direction) {
        direction.Normalize();
        velocity -= direction * acceleration * Time.deltaTime;
        if(velocity.magnitude <= .1f && velocity != Vector3.zero)
            velocity = new Vector3(0.1f, 0.0f, 0.0f);
        UpdateAudioPitch();
    }

    public void Stop() {
        velocity = Vector3.zero;
        UpdateAudioPitch();
    }


    public void SetMaxSpeed(float maxSpeed)
    {
        this.maxSpeed = maxSpeed;
    }

    public void SetDirection(Vector3 direction) {
        float speed = Speed;
        velocity = direction * speed;
    }

    public float GetSpeed()
    {
        return velocity.magnitude;
    }

    private void UpdateAudioPitch() {
        audioSource.pitch = minPitch + ((maxPitch - minPitch) * (velocity.magnitude / 11.1f));
    }

    public void UpdateAudioPitch1(float speed) {
        audioSource.pitch = minPitch + ((maxPitch - minPitch) * (speed / 11.1f));
    }

    private void TruncateVelocity() {
        if(velocity.magnitude >= maxSpeed){
            velocity.Normalize();
            velocity *= maxSpeed;
        } else {
            return;
        }
    }
}





