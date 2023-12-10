using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TMP_Text lapCounterText, bestLapTimeText, currentLapTimeText, positionText, countdownText, positionTextInfo;

    public GameObject panelinfo, Gameplay, NextButton, RetryButton, ButtonPause, PanelPause;

    private void Awake()
    {
        instance = this;
    }

}

