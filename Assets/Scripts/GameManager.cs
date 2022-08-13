using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public GameObject[] obstaclePrefab;
    public GameObject[] animalPrefab;
    public GameObject titleText;
    public GameObject background;
    public GameObject startButton;
    public GameObject exitButton;
    public GameObject gameoverText;
    public GameObject restartButton;

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
    private int i;

    private void Awake()
    {
        Time.timeScale = 0;
    }

    void Start()
    {
        i = 0;
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
        InvokeRepeating("SpawnAnimal", startDelay1, repeatRate1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0;
            titleText.SetActive(true);
            background.SetActive(true);
            startButton.SetActive(true);
            exitButton.SetActive(true);
        }

        if (playerControllerScript.gameOver && i == 0)
        {
            gameoverText.SetActive(true);
            restartButton.SetActive(true);
            exitButton.SetActive(true);
            i++;
        }
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

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        titleText.SetActive(false);
        background.SetActive(false);
        startButton.SetActive(false);
        exitButton.SetActive(false);
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
