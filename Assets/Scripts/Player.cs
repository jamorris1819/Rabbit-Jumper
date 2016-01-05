﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MonoBehaviour
{
    enum State { NORMAL, JETPACK, BUBBLE, WINGS };
    Rigidbody2D rbody;
    int baseScore = 0;
    int extraScore = 0;
    bool isDead = false;
    public Sprite deadSprite;
    public new Transform camera;
    public Text scoreLabel;
    public float PLAYER_SPEED = 10f;
    public GameObject jetpack;
    public GameObject wings;
    public GameObject bubble;

    public AudioClip jetpackSound;
    public AudioClip bubbleSound;
    public AudioClip jumpSound;
    public AudioClip coinSound;
    public AudioClip biteSound;

    public GameObject phone;

    Animator animator;
    State state;

    float timer;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.isKinematic = true;
        state = State.NORMAL;
        timer = 5f;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (rbody.isKinematic && (Input.GetKey(KeyCode.Space) || Input.touches.Length > 0))
        {
            Destroy(phone);
            rbody.isKinematic = false;
        }
        if (transform.position.y - camera.transform.position.y < -11f)
            Die();
        if (isDead || rbody.isKinematic)
            return;
        baseScore = Mathf.RoundToInt(camera.position.y / 4);
        scoreLabel.text = "Score: " + (baseScore + extraScore) + "     FPS: " + (1f / Time.smoothDeltaTime);
        if(state != State.NORMAL)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if (state == State.JETPACK)
                {
                    jetpack.SetActive(false);
                    animator.SetTrigger("Shrink");
                    animator.ResetTrigger("Grow");
                }
                else if (state == State.WINGS)
                {
                    wings.SetActive(false);
                    rbody.gravityScale = 3f;
                }
                else if (state == State.BUBBLE)
                {
                    bubble.SetActive(false);
                    rbody.gravityScale = 3f;
                }
                state = State.NORMAL;
            }
        }
    }

    void FixedUpdate()
    {
        if (isDead || rbody.isKinematic)
            return;
#if UNITY_EDITOR
        transform.position += new Vector3(Input.GetAxis("Horizontal") * 0.5f * PLAYER_SPEED * Time.deltaTime, 0, 0);
#elif UNITY_ANDROID
        transform.position += new Vector3(Input.acceleration.x * 0.5f * PLAYER_SPEED * Time.deltaTime, 0, 0);
#endif
        if (rbody.velocity.y == 0) { 
            rbody.velocity += new Vector2(0, PLAYER_SPEED);
            camera.GetComponent<AudioManager>().Play(jumpSound);
        }
        if (state == State.JETPACK)
            rbody.velocity = new Vector2(0, PLAYER_SPEED * 1.5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead)
            return;
        if (other.tag == "Spring" && rbody.velocity.y <= 0 && state != State.BUBBLE)
        {
            rbody.velocity += new Vector2(0, PLAYER_SPEED * 1.5f);
        }
        else if (other.tag == "Spikes" && rbody.velocity.y <= 4f && (state == State.NORMAL || state == State.WINGS))
        {
            Die();
        }
        else if (other.tag == "Enemy" && rbody.velocity.y <= 4f && state != State.JETPACK)
        {
            Die();
        }
        else if (other.tag == "Coin")
        {
            extraScore += 2;
            Destroy(other.gameObject);
            camera.GetComponent<AudioManager>().Play(coinSound);
        } else if(other.tag == "Powerup" && state == State.NORMAL)
        {
            Powerup powerup = other.GetComponent<Powerup>();
            if (powerup.type == Powerup.Type.JETPACK)
            {
                jetpack.SetActive(true);
                timer = 3.5f;
                state = State.JETPACK;
                animator.SetTrigger("Grow");
                animator.ResetTrigger("Shrink");
                camera.GetComponent<AudioManager>().Play(jetpackSound);
                Destroy(other.gameObject);
            }
            else if (powerup.type == Powerup.Type.WINGS)
            {
                wings.SetActive(true);
                timer = 10f;
                state = State.WINGS;
                rbody.gravityScale = 1;
                Destroy(other.gameObject);
            }
            else if (powerup.type == Powerup.Type.BUBBLE)
            {
                bubble.SetActive(true);
                timer = 10f;
                state = State.BUBBLE;
                rbody.gravityScale = -0.00001f;
                rbody.velocity = new Vector2(0, PLAYER_SPEED);
                camera.GetComponent<AudioManager>().Play(bubbleSound);
                Destroy(other.gameObject);
            }
        } else if(other.tag == "Carrot")
        {
            AddCarrot();
            Destroy(other.gameObject);
        }
    }

    public void Die()
    {
        isDead = true;
        GetComponent<SpriteRenderer>().sprite = deadSprite;
        GetComponent<BoxCollider2D>().enabled = false;
        rbody.velocity += new Vector2(0, PLAYER_SPEED * 1.5f);
        PlayerPrefs.SetInt("highscore", baseScore + extraScore);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu");
    }

    public void AddCarrot()
    {
        int currentCarrots = PlayerPrefs.GetInt("carrotCount");
        PlayerPrefs.SetInt("carrotCount", currentCarrots + 1);
        camera.GetComponent<AudioManager>().Play(biteSound);
    }
}