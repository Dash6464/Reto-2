using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerPosition : MonoBehaviour
{
    public List<CheckpointChecker> cars = new List<CheckpointChecker>();

    public IEnumerable<CheckpointChecker> positions;
    CheckpointChecker carPlayer;
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Racer1")
        {
            carPlayer = Game_Manager.instance.car.GetComponent<CheckpointChecker>();
        }
        if (SceneManager.GetActiveScene().name == "Racer2")
        {
            carPlayer = Game_Manager2.instance.car.GetComponent<CheckpointChecker>();
        }
        cars.Insert(0, carPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        positions = cars.OrderByDescending(car => car.currentLap)
        .ThenByDescending(car => car.numberCheckpoint);
        cars = positions.ToList();
        for (int i = 0; i < cars.Count; i++)
        {
            int position = cars.IndexOf(cars[i]);
            if (position == 0)
            {
                cars[0].position = 1;
            }

            if (position == 1)
            {
                cars[1].position = 2;
            }

            if (position == 2)
            {
                cars[2].position = 3;
            }


        }
    }

}
