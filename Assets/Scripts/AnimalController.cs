using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public float speed;
    public float jumpForce;

    private PlayerController playerController;
    private GameObject player;
    private Rigidbody playerRb;
    private AudioSource playerAudio;
    private float horizontal;
    private float zRange = 8.5f;

    void Start()
    {
        player = GameObject.Find("Player");
        playerRb = player.GetComponent<Rigidbody>();
        playerController = player.GetComponent<PlayerController>();
        playerAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        Movement();
        Jump();
    }

    void Movement()
    {
        horizontal = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.right * Time.deltaTime * horizontal * speed);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerController.ScoreToAdd(25);
            playerAudio.PlayOneShot(playerController.jumpSound, 1.0f);
            playerController.playerAnim.SetBool("Jump_b", true);
            StartCoroutine(JumpDelay());
            playerController.isOnAnimal = false;
            player.transform.parent = null;
            StartCoroutine(TimeSlow());
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            player.GetComponent<PlayerController>().enabled = true;
            GetComponent<AnimalController>().enabled = false;
            GetComponent<AnimalScript>().speed = 15;
        }
    }

    IEnumerator TimeSlow()
    {
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 0.2f;
    }

    IEnumerator JumpDelay()
    {
        yield return new WaitForSeconds(0.5f);
        playerController.playerAnim.SetBool("Jump_b", false);
    }
}
