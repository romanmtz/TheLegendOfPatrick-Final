using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float horizontal;
    public float speed = 9f;
    public float jumpingPower = 3f;
    bool isFacingRight = true;


    bool isWallSliding;
    public float wallSlidingSpeed = 2f;

    bool isWallJumping;
    float wallJumpingDirection;
    public float wallJumpingTime = 0.2f;
    float wallJumpingCounter;
    public float wallJumpingDuration = 0.2f;
    Vector2 wallJumpingPower = new Vector2(10f, 20f);

    public LayerMask groundLayer;
    public LayerMask wallLayer;

    Transform groundCheck;
    Transform wallCheck;

    Rigidbody2D rb;

    public float ascendRange = 5f;
    public float kinesisRange = 20f;
    public Camera mainCamera;



    Rigidbody2D movingObject;



    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        groundCheck = GameObject.Find("Ground Check").GetComponent<Transform>();
        wallCheck = GameObject.Find("Wall Check").GetComponent<Transform>();

    }

    public void Movement(float move, bool jump, bool jumpCancel)
    {
        horizontal = move;

        if (jump && IsGrounded())
        {
            float jumpForce = Mathf.Sqrt(jumpingPower * -2 * (Physics2D.gravity.y * rb.gravityScale));
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        if (jumpCancel && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
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


    void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    void WallJump()
    {
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

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
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
    }

    void StopWallJumping()
    {
        isWallJumping = false;
    }

    void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void Ascend(bool ascend)
    {

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.up, ascendRange);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.up * ascendRange, Color.red);
        if (hit.collider != null)
        {

            if (hit.collider.CompareTag("Ascendable"))
            {

                Debug.Log("Ascendable");

                if (ascend)
                {

                    Debug.Log("Ascending");

                    Bounds colliderBounds = hit.collider.bounds;
                    float top = colliderBounds.max.y;
                    transform.position = new Vector2(transform.position.x, top);

                }
            }
        }

    }

    public void Kinesis(Vector3 mousePosition, bool hold)
    {

        Vector2 screenPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        Vector2 direction = (screenPosition - (Vector2)transform.position).normalized;
        float distance = Vector2.Distance(screenPosition, transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0f), direction, Mathf.Clamp(distance, 0, kinesisRange));
        Debug.DrawRay(transform.position + new Vector3(0.5f, 0f), direction * distance);


        if (hold)
        {
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Moveable"))
                {
                    Debug.Log("Moveable");
                    movingObject = hit.collider.attachedRigidbody;

                }

            }

            if (movingObject != null && distance < kinesisRange)
            {
                Vector2 targetPosition = screenPosition - movingObject.velocity * Time.deltaTime;
                movingObject.MovePosition(targetPosition);

            }

            if(movingObject != null)
            movingObject.velocity = Vector3.zero;


        }

        if (!hold)
        {
            movingObject = null;
        }
    }






}

