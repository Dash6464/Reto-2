using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    public int countDownTime;
    public List<CarIA> carIAs;
    public CarController carPlayer;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Racer1")
        {
            carPlayer = Game_Manager.instance.car.GetComponent<CarController>();
        }
        if (SceneManager.GetActiveScene().name == "Racer2")
        {
            carPlayer = Game_Manager2.instance.car.GetComponent<CarController>();
        }
        StartCoroutine(CountDown());
    }

    // Update is called once per frame
    IEnumerator CountDown()
    {
        while (countDownTime > 0)
        {
            UIManager.instance.countdownText.text = countDownTime.ToString();
            yield return new WaitForSeconds(1f);
            countDownTime--;
            carPlayer.isBreaking = true;
            for (int i = 0; i < carIAs.Count; i++)
            {
                carIAs[i].isBreaking = true;
            }
        }
        UIManager.instance.countdownText.text = "YA";
        yield return new WaitForSeconds(1f);
        UIManager.instance.countdownText.text = "";
        UIManager.instance.ButtonPause.SetActive(true);
        carPlayer.isBreaking = false;
        for (int i = 0; i < carIAs.Count; i++)
        {
            carIAs[i].isBreaking = false;
        }
    }
}
