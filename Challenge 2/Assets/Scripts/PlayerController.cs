using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rd2d;
    private float hozMovement;
    private float verMovement;
    private int lives;
    private int score;
    private bool facingRight = true;

    Animator anim;
    AudioSource audioSource;
    public AudioClip winEffect;
    public AudioClip getCoin;
    public AudioClip loseEffect;
    public AudioClip enemyEffect;
    public Text scoreText;
    public Text livesText;
    public Text resultText;
    public float speed;

    private bool isOnGround;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask allGround;


    void Start()
    {
        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        score = 0;
        lives = 3;
        resultText.text = "";
        SetScoreText();
        SetLivesText();
    }

    void Update()
    {
        // left input
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            anim.SetInteger("State", 0);
        }

        // right input
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            anim.SetInteger("State", 0);
        }

        // jump
        /*if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetInteger("State", 2);
        }*/
    }

    void FixedUpdate()
    {
        hozMovement = Input.GetAxisRaw("Horizontal");
        verMovement = Input.GetAxisRaw("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));

        isOnGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, allGround);

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            audioSource.PlayOneShot(getCoin);
            score += 1;
            SetScoreText();
            Destroy(collision.gameObject);

            if (score == 4)
            {
                transform.position = new Vector2(89.55f, 2f);
                lives = 3;
                SetLivesText();
            }
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            audioSource.PlayOneShot(enemyEffect);
            lives -= 1;
            SetLivesText();
            Destroy(collision.gameObject);
        }

        if (score == 8)
        {
            SetResultText();
        }

        if (lives <= 0)
        {
            SetResultText();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
            { 
                rd2d.AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
                anim.SetBool("isInAir", true);
                anim.SetInteger("State", 2);
            }
            else
            {
                anim.SetBool("isInAir", false);
                anim.SetInteger("State", 0);
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    void SetScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    void SetLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();
    }

    void SetResultText()
    {
        if (score == 8)
        {
            audioSource.PlayOneShot(winEffect);
            resultText.text = "You win! Game created by Kayla Hoyte.";
        }

        if (lives <= 0)
        {
            audioSource.PlayOneShot(loseEffect);
            resultText.text = "You lose!";
            Destroy(this);
        }
    }
}
