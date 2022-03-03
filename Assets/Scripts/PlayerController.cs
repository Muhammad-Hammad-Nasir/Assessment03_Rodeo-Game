using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public Animator playerAnim;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip crashSound;
    public AudioClip jumpSound;
    public float speed;
    public float jumpForce;
    public float gravityModifier;
    public bool isOnGround = true;
    public bool gameOver = false;
    public bool isOnAnimal;

    private GameObject animalControl;
    private Rigidbody playerRb;
    private AudioSource playerAudio;
    private float horizontal;
    private float zRange = 8.5f;
    private int i = 0;
    private int score = 0;


    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
        playerAnim.SetFloat("Speed_f", 3);
    }

    void Update()
    {
        if (!gameOver)
        {
            Movement();
            if (!isOnAnimal)
            {
                JumpMove();
            }
            else if (isOnAnimal)
            {
                AnimalMovement();
            }
        }
        else if (gameOver)
        {
            GameOver();
        }
        DisplayScore();
        OutOfBounds();
    }

    void Movement()
    {
        horizontal = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.right * Time.deltaTime * horizontal * speed);
    }

    void JumpMove()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && i == 0)
        {
            ScoreToAdd(25);
            StartCoroutine(TimeSlow());
            isOnGround = false;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerAnim.SetBool("Jump_b", true);
            StartCoroutine(JumpDelay());
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 1.0f);
            i++;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isOnGround)
        {
            Time.timeScale = 1;
            playerRb.AddForce(Vector3.down * jumpForce, ForceMode.Impulse);
        }
    }

    void AnimalMovement()
    {
        Time.timeScale = 1;
    }

    void GameOver()
    {
        isOnGround = true;
        playerAnim.SetBool("Death_a", true);
        playerAnim.SetInteger("DeathType_int", 2);
    }

    public void ScoreToAdd(int addScore)
    {
        score += addScore;
    }

    void DisplayScore()
    {
        scoreText.text = "Score:" + score;
    }

    void OutOfBounds()
    {
        if (transform.position.z < -zRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zRange);
        }
        else if (transform.position.z > zRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRange);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && i == 1)
        {
            Time.timeScale = 1;
            isOnGround = true;
            gameOver = true;
            Debug.Log("Game Over!");
            playerAnim.SetBool("Death_a", true);
            playerAnim.SetInteger("DeathType_int", 2);

        }
        else if (collision.gameObject.CompareTag("Animal"))
        {
            Time.timeScale = 1;
            isOnAnimal = true;
            animalControl = collision.gameObject;
            animalControl.transform.position = new Vector3(0, 0, transform.position.z);
            playerRb.transform.position = new Vector3(0, 2f, transform.position.z);
            transform.parent = animalControl.transform;
            animalControl.GetComponent<AnimalScript>().speed = 0;
            animalControl.GetComponent<AnimalController>().enabled = true;
            playerRb.GetComponent<PlayerController>().enabled = false;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameOver = true;
            dirtParticle.Stop();
            Debug.Log("Game Over!");
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            playerAudio.PlayOneShot(crashSound, 1.0f);
        }
    }

    IEnumerator TimeSlow()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0.2f;
    }

    IEnumerator JumpDelay()
    {
        yield return new WaitForSeconds(1f);
        playerAnim.SetBool("Jump_b", false);
        playerAnim.SetFloat("Speed_f", 1);
    }
}
