using System;
using System.Collections.Generic;
using UnityEngine;

public class Horizontal : Controller {
    [SerializeField]
    PIDController controller;

    [SerializeField]
    float power;
    [SerializeField]
    Transform[] targets;
    [SerializeField]
    Transform center;

    float velocityThreshold = 0.01f;

    new Rigidbody rigidbody;
    Transform targetTransform;

    public override float Power {
        get {
            return power;
        }
        set {
            power = value;
        }
    }

    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        targetTransform = targets[UnityEngine.Random.Range(0, targets.Length)];
    }

    public override PIDController GetController() {
        return controller;
    }

    public override void SetTarget(int index) {
        targetTransform = targets[index];
    }


    void FixedUpdate() {
        float throttle = controller.Update(Time.fixedDeltaTime, rigidbody.position.z, targetTransform.position.z);
        rigidbody.AddForce(new Vector3(0, 0, throttle * power));

        if(controller.velocity < velocityThreshold && controller.velocity > -velocityThreshold && targetTransform == center) {
            targetTransform = targets[UnityEngine.Random.Range(0, targets.Length)];
            //Debug.Log("Setting new position");
        } else if(targetTransform != center && controller.velocity < velocityThreshold && controller.velocity > -velocityThreshold) {
            targetTransform = center;
            //Debug.Log("Setting position to center");
        } 
    }
}