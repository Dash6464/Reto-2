using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarIA : MonoBehaviour
{
    public Transform path;

    private List<Transform> nodes;
    private int currentNode = 0;

    public Rigidbody carRb;
    public Vector3 _centerOfMass;

    public bool isBreaking;

    public int nextCheckpoint, currentLap;

    private float currentSteerAngle;

    public float currentSpeed, topSpeed;
    [Range(0, 100)] public float acceleration;


    [Header("Settings")]
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;


    [Header("Wheels")]
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;


    // Start is called before the first frame update
    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;

        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();

        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        HandleMotor();
        HandleSteering();
        UpdateWheels();
        CheckWayPointDistance();
        RacerFinishied();
    }

    private void HandleMotor()
    {
        currentSpeed = 2 * 22 / 7 * frontLeftWheelCollider.radius * frontLeftWheelCollider.rpm * 60 / 1000;
        currentSpeed = Mathf.Round(currentSpeed);
        if (currentSpeed < topSpeed && !isBreaking)
        {
            frontRightWheelCollider.motorTorque = motorForce + acceleration;
            frontLeftWheelCollider.motorTorque = motorForce + acceleration;
        }
        else
        {
            frontLeftWheelCollider.motorTorque = 0;
            frontRightWheelCollider.motorTorque = 0;
        }

    }

    private void HandleSteering()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        currentSteerAngle = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
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

    private void CheckWayPointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 4.5f)
        {
            if (currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
        //print(currentNode);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PointBreak")
        {
            frontRightWheelCollider.brakeTorque = breakForce;
            frontLeftWheelCollider.brakeTorque = breakForce;
            rearLeftWheelCollider.brakeTorque = breakForce;
            rearRightWheelCollider.brakeTorque = breakForce;
            isBreaking = true;
        }

    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PointBreak")
        {
            frontRightWheelCollider.brakeTorque = 0;
            frontLeftWheelCollider.brakeTorque = 0;
            rearLeftWheelCollider.brakeTorque = 0;
            rearRightWheelCollider.brakeTorque = 0;
            isBreaking = false;
        }

    }

    public void CheckpointHit(int cpNumber)
    {
        if (cpNumber == nextCheckpoint)
        {
            nextCheckpoint++;

            if (nextCheckpoint == RaceManager.instance.allCheckpoints.Length)
            {
                nextCheckpoint = 0;
                currentLap++;
            }
        }
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
        }
    }
}
