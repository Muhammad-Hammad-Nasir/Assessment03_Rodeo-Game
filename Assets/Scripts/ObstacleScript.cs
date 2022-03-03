using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public float speed = 20;

    private PlayerController playerControllerScript;
    private GameObject player;
    private Rigidbody playerRb;
    private float leftBound = -10;

    void Start()
    {
        player = GameObject.Find("Player");
        playerControllerScript = player.GetComponent<PlayerController>();
        playerRb = player.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (playerControllerScript.gameOver == false)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        if (transform.position.x < leftBound)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Animal") && playerControllerScript.isOnAnimal == true)
        {
            Debug.Log("GameOver");
            playerControllerScript.gameOver = true;
            player.transform.parent = null;
            playerRb.constraints = RigidbodyConstraints.None;
            playerRb.AddForce(Vector3.forward * 5, ForceMode.Impulse);
            Destroy(gameObject);
        }
    }
}
