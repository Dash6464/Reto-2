using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Botones_inicio : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("CarSelect");
    }
    public void HowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }
    public void About()
    {
        SceneManager.LoadScene("About");
    }
    public void Back()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }


    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif

    }
}
