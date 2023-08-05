using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Vector2 LastCheckpoint;
    public AudioClip jumpSFX;
    public AudioClip skatingSFX;
    // Movement variables
    float horizontal;
    public float speed = 9f;
    float originalSpeed;
    bool isFacingRight = true;
    public PhysicsMaterial2D[] material;



    // Jumping variables

    public float jumpingPower = 3f;
    public float iceJumpingPower = 6f;

    // Wall sliding variables
    public AudioClip wallslideSFX;
    bool isWallSliding;
    public float wallSlidingSpeed = 2f;

    // Wall jumping variables
    bool isWallJumping;
    float wallJumpingDirection;
    public float wallJumpingTime = 0.2f;
    float wallJumpingCounter;
    public float wallJumpingDuration = 0.2f;
    Vector2 wallJumpingPower = new Vector2(10f, 23f);

    // Layer masks
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask objectLayer;

    // Checkpoints
    Transform groundCheck;
    Transform wallCheck;

    // Rigidbody
    Rigidbody2D rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheck = GameObject.Find("Ground Check").GetComponent<Transform>();
        wallCheck = GameObject.Find("Wall Check").GetComponent<Transform>();
        originalSpeed = speed;

    }

    void FixedUpdate()
    {
        MovePlayer();
        ChangeMaterial();

        if ((rb.velocity.x != 0) && IsOnIce())
        {
            AudioHandler.singleton.LoopSound(skatingSFX);
        }
        else
        {
            AudioHandler.singleton.StopLoop(skatingSFX);
        }


    }

    void MovePlayer()
    {
        if (!isWallJumping)
        {
            if (IsOnIce() == true || Abilities.isSkateJumping == true)
            {

                rb.AddForce(new Vector2(horizontal * speed, rb.velocity.y), ForceMode2D.Force);
            }
            else
            {

                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

            }
        }


    }



    public void Movement(float move, bool jump, bool jumpCancel)
    {
        horizontal = move;



        if (jump && (IsGrounded() || IsObjected()))
        {

            AudioHandler.singleton.PlaySound(jumpSFX);
            AudioHandler.singleton.StopLoop(skatingSFX);


            if (IsOnIce() == true || Abilities.isSkating)
            {

                float jumpForce = (Mathf.Abs(rb.velocity.x) / 2) + Mathf.Sqrt(jumpingPower * -2 * (Physics2D.gravity.y * rb.gravityScale));
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);


            }

            else
            {
                float jumpForce = Mathf.Sqrt(jumpingPower * -2 * (Physics2D.gravity.y * rb.gravityScale));
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }


        }

        if (jumpCancel && rb.velocity.y > 0f)
        {
            if (!Abilities.isSkateJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                Debug.Log("Normal jumping cancel");
            }
            else
            {
                Debug.Log("Ice Jumping Cancel");
                rb.AddForce(new Vector2(0f, -10f), ForceMode2D.Impulse);
            }
        }

        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Turn();
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    public bool IsObjected()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.01f, objectLayer);
    }

    public bool IsOnIce()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0f, -0.5f), Vector2.down, 1);


        if (hit.collider != null)
        {

            if (hit.collider.gameObject.TryGetComponent<IceBody>(out IceBody hinge))
            {
                IceBody check = hit.collider.gameObject.GetComponent<IceBody>();
                return check.isIced;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }



    }

    public bool IsFacingRight()
    {

        return isFacingRight;
    }

    void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {


            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            AudioHandler.singleton.LoopSound(wallslideSFX);
        }
        else
        {

            AudioHandler.singleton.StopLoop(wallslideSFX);
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

            AudioHandler.singleton.PlaySound(jumpSFX);

            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                Flip();
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    void StopWallJumping()
    {
        isWallJumping = false;
    }

    void Turn()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public float ChangeSpeed(float newSpeed)
    {

        if (newSpeed == 0)
        {
            return originalSpeed;
        }

        speed = newSpeed;
        return originalSpeed;

    }

    void ChangeMaterial()
    {

        if (IsObjected() && AbilityMenu.AbilityMode == "reverse")
        {
            rb.sharedMaterial = material[0];
        }
        else
        {
            rb.sharedMaterial = material[1];
        }


    }


}
