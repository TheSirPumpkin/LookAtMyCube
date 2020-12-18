using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRoot : MonoBehaviour
{
    public GameObject[] Obstacles;
  
    void Start()
    {
        Obstacles[Random.RandomRange(0, Obstacles.Length)].SetActive(true);
    }
}
