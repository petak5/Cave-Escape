using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private TrailRenderer tr;

    private Vector2 movement;

    private bool canDash = true;
    private bool isDashing = false;
    [SerializeField] private float dashingPower = 10f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
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
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
