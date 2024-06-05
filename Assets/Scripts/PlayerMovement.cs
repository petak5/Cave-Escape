using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject dashIcon;

    private Vector3 startingPosition;
    private Vector2 movement;

    private bool isDead = false;
    private bool isOverWater = false;
    private bool canDash = true;
    private bool isDashing = false;
    [SerializeField] private float dashingPower = 10f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    private GameObject diamond;
    private GameObject enemy1;
    private GameObject enemy2;

    private void Start()
    {
        startingPosition = transform.position;
        panel.SetActive(false);
        diamond = GameObject.FindWithTag("Diamond");
        if (diamond != null)
        {
            diamond.GetComponent<Renderer>().enabled = true;
        }
        enemy1 = GameObject.FindWithTag("Enemy1");
        enemy2 = GameObject.FindWithTag("Enemy2");
        if (SceneManager.GetActiveScene().name == "SecondScene")
        {
            if (!GameManager.instance.hasDiamond)
            {
                if (enemy2 != null) { enemy2.SetActive(false); }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing || isDead)
        {
            return;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Space)) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        isOverWater = false;
        if (other.gameObject.CompareTag("Diamond")) CollectDiamond(other);
        if (other.gameObject.CompareTag("Enemy1") || other.gameObject.CompareTag("Enemy2"))
        {
            if (GameManager.instance.hasPowerUp)
            {
                GameObject enemy = GameObject.FindWithTag(other.gameObject.tag);
                if (enemy != null) { enemy.SetActive(false); }
                // Do not touch, I have no idea why but it needs to be here again, otherwise the player keeps dying when dashing into slime
                isOverWater = false;
            }
            else
            {
                Die();
            }
        }
        if (other.gameObject.CompareTag("Exit1")) GoToSecondScene();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isDashing)
        {
            isOverWater = true;
        }
        else if (other.gameObject.CompareTag("Diamond") || other.gameObject.CompareTag("Enemy1") || other.gameObject.CompareTag("Enemy2"))
        {
        }
        else
        {
            Die();
        }
    }

    private void CollectDiamond(Collider2D other)
    {
        other.GetComponent<Renderer>().enabled = false;
        GameManager.instance.GotDiamond();
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float x = 0;
        float y = 0;
        if (movement.x > 0) { x = 1; }
        else if (movement.x < 0) { x = -1; }
        if (movement.y > 0) { y = 1; }
        else if (movement.y < 0) { y = -1; }
        Vector2 originalMovement = movement;
        movement = Vector2.zero;
        rb.velocity = new Vector2(x, y).normalized * dashingPower;
        dashIcon.SetActive(false);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        movement = originalMovement;
        isDashing = false;
        if (isOverWater)
        {
            Die();
        }
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        dashIcon.SetActive(true);
    }

    private void Die()
    {
        isOverWater = false;
        isDead = true;
        movement = Vector2.zero;
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        panel.SetActive(true);
    }

    public void RestartLevel()
    {
        isDead = false;
        transform.position = startingPosition;
        if (SceneManager.GetActiveScene().name == "FirstScene")
        {
            GameManager.instance.RestartDiamond();
        }
        if (diamond != null)
        {
            diamond.GetComponent<Renderer>().enabled = true;
        }
        if (SceneManager.GetActiveScene().name == "SecondScene")
        {
            if (enemy1 != null) { enemy1.SetActive(true); }

            if (!GameManager.instance.hasDiamond)
            {
                if (enemy2 != null) { enemy2.SetActive(false); }
            }
            else
            {
                if (enemy2 != null) { enemy2.SetActive(true); }
            }
        }
        panel.SetActive(false);
    }

    public void RestartGame()
    {
        RestartLevel();
        GameManager.instance.RestartDiamond();
        if (SceneManager.GetActiveScene().name != "FirstScene")
        {
            SceneManager.LoadScene("Scenes/FirstScene");
        }
    }

    public void GoToAfterlife()
    {
        GameManager.instance.previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Scenes/Afterlife");
    }

    public void GoToSecondScene()
    {
        SceneManager.LoadScene("Scenes/SecondScene");
    }
}
