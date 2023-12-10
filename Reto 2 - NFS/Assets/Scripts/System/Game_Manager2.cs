using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager2 : MonoBehaviour
{

    public static Game_Manager2 instance;

    public int index;

    public GameObject[] cars;

    public GameObject car;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
        index = PlayerPrefs.GetInt("carIndex");
        car = Instantiate(cars[index], new Vector3(23.27f, 0.1135193f, -26f), Quaternion.identity);
    }
    void Start()
    {

    }
}
