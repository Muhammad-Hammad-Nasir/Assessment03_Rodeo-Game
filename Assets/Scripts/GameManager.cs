using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] obstaclePrefab;
    public GameObject[] animalPrefab;

    private PlayerController playerControllerScript;
    private Vector3 spawnPos;
    private float startDelay = 2;
    private float repeatRate = 1.5f;
    private float startDelay1 = 1;
    private float repeatRate1 = 2.5f;
    private float zRange = 8.5f;
    private float randomRange;
    private int randomNum;
    private float randomAnimalRange;
    private int randomAnimalNum;

    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
        InvokeRepeating("SpawnAnimal", startDelay1, repeatRate1);
    }

    void SpawnObstacle()
    {
        randomNum = Random.Range(0, obstaclePrefab.Length);
        randomRange = Random.Range(-zRange, zRange);
        spawnPos = new Vector3(35, 0.5f, randomRange);

        if (playerControllerScript.gameOver == false)
        {
            Instantiate(obstaclePrefab[randomNum], spawnPos, obstaclePrefab[randomNum].transform.rotation);
        }
    }

    void SpawnAnimal()
    {
        randomAnimalNum = Random.Range(0, obstaclePrefab.Length);
        randomAnimalRange = Random.Range(-zRange, zRange);
        spawnPos = new Vector3(30, 0.5f, randomAnimalRange);

        if (playerControllerScript.gameOver == false)
        {
            Instantiate(animalPrefab[randomAnimalNum], spawnPos, animalPrefab[randomAnimalNum].transform.rotation);
        }
    }
}
