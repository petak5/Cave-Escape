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

    private Vector2 movement;

    private bool isDead = false;
    private bool isOverWater = false;
    private bool canDash = true;
    private bool isDashing = false;
    [SerializeField] private float dashingPower = 10f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    private GameObject diamond;

    private void Start()
    {
        panel.SetActive(false);
        diamond = GameObject.FindWithTag("Diamond");
        diamond.GetComponent<Renderer>().enabled = true;
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
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
        if (other.gameObject.CompareTag("Diamond")) other.GetComponent<Renderer>().enabled = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isDashing)
        {
            isOverWater = true;
        }
        else if (other.gameObject.CompareTag("Diamond"))
        {
            
        }
        else
        {
            Die();
        }
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
    }

    private void Die()
    {
        isOverWater = false;
        isDead = true;
        movement = Vector2.zero;
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        diamond.GetComponent<Renderer>().enabled = true;
        panel.SetActive(true);
    }
    public void RestartGame()
    {
        isDead = false;
        transform.position = new Vector3(0, -3, -1);
        panel.SetActive(false);
    }

    public void GoToAfterlife()
    {
        SceneManager.LoadScene("Scenes/Afterlife");
    }
}
