using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    private Vector2 moveDir;
    private Vector2 lastMoveDir;
    private Vector2 dashDir; // nová proměnná pro směr dáshování
    private const float SPEED = 8f;
    private Animator animator;
    private bool isDashButtonDown;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float moveX = 0f;
        float moveY = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveY = +1f;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) 
        {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveX = +1f;
        }

        // použití instance proměnné moveDir
        moveDir = new Vector2(moveX, moveY).normalized;

        bool isIdle = moveX == 0 && moveY == 0;
        if (isIdle)
        {
            rigidbody2D.velocity = Vector2.zero;
            animator.SetBool("isMoving", false);
        }
        else
        {
            lastMoveDir = moveDir;
            rigidbody2D.velocity = moveDir * SPEED;
            animator.SetFloat("horizontalMovement", moveDir.x);
            animator.SetFloat("verticalMovement", moveDir.y);
            animator.SetBool("isMoving", true);
        }

        // přidáno - detekce klávesy pro Dash a výpočet směru dáshování
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isDashButtonDown = true;
            dashDir = lastMoveDir;
        }
    }

    private void FixedUpdate()
    {
        // použití proměnné dashDir místo moveDir, pokud byla detekována klávesa pro Dash
        if (isDashButtonDown)
        {
            float dashAmount = 5f;
            rigidbody2D.MovePosition(rigidbody2D.position + dashDir * dashAmount);
            isDashButtonDown = false;
        }
    }
}
