using System;
using System.Collections.Generic;
using UnityEngine;

public class Cross : Controller {
    [SerializeField]
    PIDController controller;
    [SerializeField]
    float power;
    [SerializeField]
    Transform[] targets;
    [SerializeField]
    Transform center;
    //GameObject flame;
    //[SerializeField]
    //float flameSize;
    float velocityThreshold = 0.01f;
    new Rigidbody rigidbody;
    Transform targetTransform;
    //List<Vector3> targetPositions;
    //Vector3 targetPosition;

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
        //targetPositions = new List<Vector3>();
        //foreach (var target in targets) {
        //    targetPositions.Add(target.position);
        }

        //SetTarget(0);

    public override PIDController GetController() {
        return controller;
    }

    public override void SetTarget(int index) {
        targetTransform = targets[index];
    }

    //void SetScale(GameObject go, float scale) {
    //    scale = Mathf.Clamp(scale, 0, 1);

    //    if (scale < 0.1f) {
    //        go.SetActive(false);
    //    } else {
    //        go.SetActive(true);
    //        go.GetComponent<Transform>().localScale = new Vector3(scale, scale, scale) * flameSize;
    //    }
    //}

    void FixedUpdate() {
        float throttle = controller.Update(Time.fixedDeltaTime, rigidbody.position.x, targetTransform.position.x);
        rigidbody.AddForce(new Vector3(throttle * power, 0, 0));

        if(controller.velocity < velocityThreshold && controller.velocity > -velocityThreshold && targetTransform == center) {
            targetTransform = targets[UnityEngine.Random.Range(0, targets.Length)];
            //Debug.Log("Setting new position");
        } else if(targetTransform != center && controller.velocity < velocityThreshold && controller.velocity > -velocityThreshold) {
            targetTransform = center;
            //Debug.Log("Setting position to center");
        } 
    }
}
