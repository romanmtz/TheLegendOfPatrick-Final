using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Transform groundCheck;
    Transform wallCheck;

    public float speed = 3.0f;
    public float buttonTime = 0.5f;
    public float jumpHeight = 5;
    public float cancelRate = 100;

    public LayerMask groundLayer;
    public LayerMask wallLayer;

    bool isFacingRight = true;

    float jumpTime;
    bool jumping;
    bool jumpCancelled;

    bool isWallSliding;
    float wallSlidingSpeed = 2f;
    float movement;

    bool isWallJumping;
    float wallJumpingDirection;
    float wallJumpingTime = 0.2f;
    float wallJumpingCounter;
    float wallJumpingDuration = 0.4f;
    Vector2 wallJumpingPower = new Vector2(10f, 20f);


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheck = GameObject.Find("Ground Check").GetComponent<Transform>();
        wallCheck = GameObject.Find("Wall Check").GetComponent<Transform>();

    }


    public void Move(float input)
    {

        movement = input;


        if (!isWallJumping)
        {
            rb.velocity = new Vector2(movement * speed, rb.velocity.y);
        }

    }

    public void Jump(bool jump)
    {

        if (jump && IsGrounded())
        {
            float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumping = true;
            jumpCancelled = false;
            jumpTime = 0;
        }

        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (jump && wallJumpingCounter > 0f)
        {

            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;

            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }

        if (jumping)
        {
            jumpTime += Time.deltaTime;
            if (Input.GetKeyUp(KeyCode.Space))
            {
                jumpCancelled = true;
            }
            if (jumpTime > buttonTime)
            {
                jumping = false;
            }
        }
    }

    bool IsGrounded()
    {

        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

    }

    bool IsWalled()
    {

        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);

    }

    void StopWallJumping()
    {

        isWallJumping = false;

    }

    void WallSlide()
    {

        if (IsWalled() && !IsGrounded() && movement != 0f)
        {


            Debug.Log("WallSliding!");

            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));

        }

        else
        {

            isWallSliding = false;

        }

    }



    void Flip()
    {

        if (isFacingRight && movement < 0f || !isFacingRight && movement > 0f)
        {

            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

        }

    }

    void Update()
    {
        WallSlide();
        if (!isWallJumping)
        {
            Flip();
        }
    }


    void FixedUpdate()
    {

        if (jumpCancelled && jumping && rb.velocity.y > 0)
        {
            rb.AddForce(Vector2.down * cancelRate);
        }
    }

}
