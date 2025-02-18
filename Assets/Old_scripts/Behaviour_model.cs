using System;
//using System.Collections.Generic;
using UnityEngine;

//make the drone move in a straight line
//make the drone move between coordinates in different shapes
//maintain speed and time


public class Behaviour_model : MonoBehaviour
{
    [SerializeField] private EvaulationType evaulationType = EvaulationType.Progress; //why evaluation type??????
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 targetPosition;

    [SerializeField] private float speed;
    [SerializeField] private float distance = 0f; //for equation
    [SerializeField] private float closingDistance = 1f; //clsong distance for what?????????

    private Vector3 position;
    private Vector3 prevSetPosition = Vector3.zero; //saving initial vector information

    private void Start()
    {
        position = transform.position; //assigining initial position to position variable
    }

    // Directly update values on editor interaction instead of on start
    private void OnValidate() //resetting positions
    {
        if (prevSetPosition != transform.position) 
        {
            prevSetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            startPosition = transform.position;
        }
        else if (prevSetPosition != startPosition)
        {
            transform.position = startPosition;
            prevSetPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z);
        }
    }

    private void Update() //updates at every frame but not inline with physics
    {
        distance = Vector3.Distance(transform.position, targetPosition);

        switch (evaulationType) //choosing the right evaluation type
        {
            case EvaulationType.Position:
                MoveOverEquation();
                break;
            case EvaulationType.Progress:
                MoveToTargetWithPercentage();
                break;
        }
    }

    private void MoveToTargetWithPercentage()
    {
        // Distance check for the target
        if (Vector3.Distance(transform.position, targetPosition) <= closingDistance)
        {
            transform.position = targetPosition;
            return;
        }

        // Calculate process progress/percentage
        position.x -= speed * Time.deltaTime;
        float percentageHorizontal = position.x / startPosition.x;

        // Use the percentage to determine the y position
        float evaluatedValue = EvaluateEquation(percentageHorizontal);
        float dY = (startPosition.y - targetPosition.y) * evaluatedValue;
        position.y = targetPosition.y + dY;

        // Update the players position
        transform.position = position;
    }

    private void MoveOverEquation()
    {
        if (transform.position.x - speed < targetPosition.x + closingDistance)
        {
            transform.position = targetPosition;
            return;
        }

        float newYPosition = EvaluateEquation(transform.position.x - speed);
        transform.position = new Vector3(transform.position.x - speed, newYPosition, 0);
    }

    /// <summary>
    /// This method contains your equation calculation
    /// </summary>
    /// <param name="t">t can be used as progress (percentage) or as position x</param>
    /// <returns>The value at position t</returns>
    private float EvaluateEquation(float t)
    {
        // Dividing t by 2 will make sure I won't go past 0
        // Since our desired value is at the bottom of the parabool
        float x = 1 * Mathf.Pow(t, 2) + 1 * t + 0;
        return x;
    }
}

public enum EvaulationType
{
    Progress,
    Position
}