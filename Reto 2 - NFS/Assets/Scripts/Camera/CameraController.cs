using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CarController target;
    private Vector3 offsetDir;

    public float minDistance, maxDistance;
    private float activeDistance;


    void Start()
    {
        offsetDir = transform.position - target.transform.position;

        activeDistance = minDistance;

        offsetDir.Normalize();
    }


    void Update()
    {
        activeDistance = minDistance + ((maxDistance - minDistance) * (target.carRb.velocity.magnitude / target.topSpeed));

        transform.position = target.transform.position + (offsetDir * activeDistance);
    }
}
