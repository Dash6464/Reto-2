using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RacerInfo : MonoBehaviour
{

    // Update is called once per frame
    public void TyrRacer(string racer)
    {
        SceneManager.LoadScene(racer);
    }

    public void NextRacer(string racer)
    {
        SceneManager.LoadScene(racer);
    }
}
