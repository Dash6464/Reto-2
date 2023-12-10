using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointChecker : MonoBehaviour
{
    public CarController theCar;
    public CarIA theCarIA;

    public int currentLap, numberCheckpoint, position;
    public float distanceCheckpoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Checkpoint")
        {
            //Debug.Log("Hit cp " + other.GetComponent<Checkpoint>().cpNumber); 

            if (theCar.enabled == true)
            {
                theCar.CheckpointHit(other.GetComponent<Checkpoint>().cpNumber);
                currentLap = theCar.currentLap;
                numberCheckpoint = theCar.nextCheckpoint;
                UIManager.instance.positionText.text = position.ToString();
            }

            if (theCarIA.enabled == true)
            {
                theCarIA.CheckpointHit(other.GetComponent<Checkpoint>().cpNumber);
                currentLap = theCarIA.currentLap;
                numberCheckpoint = theCarIA.nextCheckpoint;
            }
        }
    }
}
