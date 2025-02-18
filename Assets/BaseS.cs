using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class BaseS : MonoBehaviour
{
    
    //Reference to codes
    public BaseDroneController droneController;
    
    public Transform Package;
    public float moveSpeed = 11.1f; // Adjust this value for speed
    public float moveSpeedd = 1.0f;
    
    public enum DroneState { MoveToInitial, Approach1, Hover1, Approach2, Hover2, Land, Hover3, Return, Hover4, MoveToFinal, End }
    public DroneState currentState = DroneState.Approach2;

}
