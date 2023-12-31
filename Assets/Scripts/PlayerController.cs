using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    public Rigidbody2D rb;
    public float jumpForce;
    public bool isOnGround = true;
    public float speed;
    public bool hasPowerup;
    private float powerupStrength = 15.0f;
    //public GameObject powerupIndicator;
    public ParticleSystem powerupParticle;
    public AudioClip contactSound;
    public AudioSource backgroundAudio;
    public AudioClip jumpSound;
    public AudioSource playerAudio;
    public AudioClip powerupSound;
    public float deathHeight = -15;
    public AudioClip deathSound;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Horizontal Movement
        horizontalInput = Input.GetAxis("Horizontal");
        rb.AddForce(Vector2.right * speed * horizontalInput);


    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && isOnGround == true)
        {
            isOnGround = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            playerAudio.PlayOneShot(jumpSound);
        }
        if (transform.position.y < deathHeight)
        {
            StartCoroutine(Die());
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;

        }

        if (collision.gameObject.CompareTag("Powerup"))
        {
            hasPowerup = true;
            //powerupIndicator.gameObject.SetActive(true);
            Destroy(collision.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            powerupParticle.Play();
            playerAudio.PlayOneShot(powerupSound);
        }
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody2D enemyRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            Debug.Log("Collided with:" + collision.gameObject.name + "with powerup set to" + hasPowerup);
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode2D.Impulse);

            playerAudio.PlayOneShot(contactSound);


        }
        
    }
    

  

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        //powerupIndicator.gameObject.SetActive(false);
        powerupParticle.Stop();
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator Die()
    {
        playerAudio.PlayOneShot(deathSound);
        GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow = null;
        GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().LookAt = null;


        yield return new WaitForSeconds(5);  
        ResetScene();
    }
}
