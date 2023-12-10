using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    private float currentSteerAngle, currentbreakForce;

    private PlayerInput playerInput;

    public Rigidbody carRb;
    public float currentSpeed, topSpeed;
    [Range(0, 100)] public float acceleration;
    public float reverseMaxSpeed;
    private Vector2 input;
    public Vector3 _centerOfMass;

    [Header("Laps")]
    public int nextCheckpoint;
    public int currentLap;

    [Header("Settings")]
    [SerializeField] private float motorForce, breakForce, maxSteerAngle, desAcceleration;

    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    [Header("Wheels")]
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    [Header("Time lap")]
    public float lapTime, bestLapTime;

    public bool isBreaking;


    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
        UIManager.instance.lapCounterText.text = currentLap + "  - " + RaceManager.instance.totalLaps;
    }

    void Update()
    {
        input = playerInput.actions["Move"].ReadValue<Vector2>();
        if (!isBreaking)
        {
            lapTime += Time.deltaTime;
            var ts = TimeSpan.FromSeconds(lapTime);
            UIManager.instance.currentLapTimeText.text = string.Format("{0:00} : {1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds);
        }
    }
    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        RacerFinishied();
    }

    private void HandleMotor()
    {
        currentSpeed = 2 * 22 / 7 * frontLeftWheelCollider.radius * frontLeftWheelCollider.rpm * 60 / 1000;
        currentSpeed = Mathf.Round(currentSpeed);
        if (currentSpeed < topSpeed && currentSpeed > -reverseMaxSpeed && !isBreaking)
        {
            frontRightWheelCollider.motorTorque = input.y * (motorForce + acceleration);
            frontLeftWheelCollider.motorTorque = input.y * (motorForce + acceleration);
        }
        else
        {
            frontLeftWheelCollider.motorTorque = 0;
            frontRightWheelCollider.motorTorque = 0;
        }

        if (input.y == 0)
        {
            frontRightWheelCollider.brakeTorque = desAcceleration;
            frontLeftWheelCollider.brakeTorque = desAcceleration;
        }
        else
        {
            frontRightWheelCollider.brakeTorque = 0;
            frontLeftWheelCollider.brakeTorque = 0;
        }
    }

    public void ApplyBreaking(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            currentbreakForce = breakForce;
        }
        else
        {
            currentbreakForce = 0f;
        }
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * input.x;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }
    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    public void CheckpointHit(int cpNumber)
    {
        if (cpNumber == nextCheckpoint)
        {
            nextCheckpoint++;

            if (nextCheckpoint == RaceManager.instance.allCheckpoints.Length)
            {
                nextCheckpoint = 0;
                LapCompleted();
            }
        }
    }


    public void LapCompleted()
    {
        currentLap++;

        if (lapTime < bestLapTime || bestLapTime == 0)
        {
            bestLapTime = lapTime;
        }

        lapTime = 0f;

        var ts = TimeSpan.FromSeconds(bestLapTime);
        UIManager.instance.bestLapTimeText.text = string.Format("{0:00} : {1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds);

        UIManager.instance.lapCounterText.text = currentLap + "  - " + RaceManager.instance.totalLaps;

    }

    public void RacerFinishied()
    {
        if (currentLap == RaceManager.instance.totalLaps)
        {
            frontLeftWheelCollider.motorTorque = 0;
            frontRightWheelCollider.motorTorque = 0;
            frontLeftWheelCollider.steerAngle = 0;
            frontRightWheelCollider.steerAngle = 0;
            isBreaking = true;
            UIManager.instance.Gameplay.SetActive(false);
            if (SceneManager.GetActiveScene().name == "Racer1")
            {
                UIManager.instance.positionTextInfo.text = Game_Manager.instance.car.GetComponent<CheckpointChecker>().position.ToString();
                if (Game_Manager.instance.car.GetComponent<CheckpointChecker>().position == 1)
                {
                    UIManager.instance.NextButton.SetActive(true);
                    UIManager.instance.RetryButton.SetActive(false);
                }
                else
                {
                    UIManager.instance.NextButton.SetActive(false);
                    UIManager.instance.RetryButton.SetActive(true);
                }
            }
            if (SceneManager.GetActiveScene().name == "Racer2")
            {
                UIManager.instance.positionTextInfo.text = Game_Manager2.instance.car.GetComponent<CheckpointChecker>().position.ToString();
                if (Game_Manager2.instance.car.GetComponent<CheckpointChecker>().position == 1)
                {
                    UIManager.instance.NextButton.SetActive(true);
                    UIManager.instance.RetryButton.SetActive(false);
                }
                else
                {
                    UIManager.instance.NextButton.SetActive(false);
                    UIManager.instance.RetryButton.SetActive(true);
                }
            }


            UIManager.instance.panelinfo.SetActive(true);
        }
    }
}