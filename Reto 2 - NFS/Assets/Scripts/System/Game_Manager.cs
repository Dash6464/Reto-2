using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{

    public static Game_Manager instance;

    public int index;

    public GameObject[] cars;

    public GameObject car;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
        index = PlayerPrefs.GetInt("carIndex");
        car = Instantiate(cars[index], new Vector3(59.73f, 0.1135231f, -25.93f), Quaternion.identity);
    }
    void Start()
    {

    }
}
