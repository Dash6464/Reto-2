using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPause : MonoBehaviour
{
    public void Pause()
    {
        UIManager.instance.ButtonPause.SetActive(false);
        UIManager.instance.PanelPause.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        UIManager.instance.ButtonPause.SetActive(true);
        UIManager.instance.PanelPause.SetActive(false);
        Time.timeScale = 1;
    }
}
