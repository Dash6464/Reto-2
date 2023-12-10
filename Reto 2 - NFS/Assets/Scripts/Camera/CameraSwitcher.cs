using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject[] cameras;
    private int currentCam;

    void Start()
    {

    }


    public void switcherCamera(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            currentCam++;

            if (currentCam >= cameras.Length)
            {
                currentCam = 0;
            }

            for (int i = 0; i < cameras.Length; i++)
            {
                if (i == currentCam)
                {
                    cameras[i].SetActive(true);
                }
                else
                {
                    cameras[i].SetActive(false);
                }
            }
        }
    }


}
