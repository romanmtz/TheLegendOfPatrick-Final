using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Movement variables
    float horizontal;
    public float speed = 9f;
    public float normalSpeed = 9f;
    public float cryosisSpeed = 18f;
    bool isFacingRight = true;

    // Jumping variables
    public float jumpingPower = 3f;
    public float iceJumpingPower = 6f;
    bool iceJumping = false;

    // Wall sliding variables
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

    // Ascend variables
    public float ascendRange = 5f;

    // Kinesis variables
    public float kinesisRange = 20f;
    public GameObject virtualMouse;
    bool usingVirtualMouse = false;
    Vector2 previousScreenPosition;
    public float cursorSensitivity = 0.2f;
    Rigidbody2D movingObject;

    // Reverse variables
    public float reverseRange = 20f;
    GameObject reversingObject;
    TimeBody tb;
    GameObject previousReversingObject;

    // Cryosis variables
    GameObject icedObject;
    IceBody ib;
    bool onIce = false;
    bool normalCancel = false;
    public PhysicsMaterial2D[] material;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheck = GameObject.Find("Ground Check").GetComponent<Transform>();
        wallCheck = GameObject.Find("Wall Check").GetComponent<Transform>();
        reversingObject = GetComponent<GameObject>();
    }

    void FixedUpdate()
    {
        MovePlayer();
        CheckIceJumping();
        CheckAbilityMenu();
    }

    void MovePlayer()
    {
        if (!isWallJumping && onIce == false)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        else if (!isWallJumping && onIce == true)
        {
            rb.AddForce(new Vector2(horizontal * speed, rb.velocity.y), ForceMode2D.Force);
        }
    }

    void CheckIceJumping()
    {
        if (iceJumping == false)
        {
            Debug.Log("Ice jumping is false");
        }
        else if (iceJumping == true)
        {
            Debug.Log("IceJumping is True");
        }
    }

    void CheckAbilityMenu()
    {
        if (AbilityMenu.GameIsChoose)
        {
            virtualMouse.SetActive(false);
        }
    }

    public void Movement(float move, bool jump, bool jumpCancel)
    {
        horizontal = move;

        if (IsObjected())
        {
            rb.sharedMaterial = material[0];
        }
        else
        {
            rb.sharedMaterial = material[1];
        }

        if (jump && (IsGrounded() || IsObjected()) && onIce == false)
        {
            float jumpForce = Mathf.Sqrt(jumpingPower * -2 * (Physics2D.gravity.y * rb.gravityScale));
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        else if (jump && (IsGrounded() || IsObjected()) && onIce == true)
        {
            float jumpForce = Mathf.Sqrt(iceJumpingPower * -2 * (Physics2D.gravity.y * rb.gravityScale));
            rb.AddForce(new Vector2(1, jumpForce), ForceMode2D.Impulse);
        }

        if (jumpCancel && rb.velocity.y > 0f && iceJumping == false)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            Debug.Log("Normal jumping cancel");
        }
        else if (jumpCancel && rb.velocity.y > 0f && iceJumping == true)
        {
            Debug.Log("Ice Jumping Cancel");
            rb.AddForce(new Vector2(0f, -10f), ForceMode2D.Impulse);
        }

        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
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

    bool IsObjected()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, objectLayer) && (movingObject != null || reversingObject != null);
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

    RaycastHit2D Raycast(Vector3 mousePosition, float horizontal, float vertical)
    {
        RaycastHit2D hit;
        virtualMouse.SetActive(true);
        Vector2 screenPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (horizontal != 0 || vertical != 0)
        {
            usingVirtualMouse = true;
            Debug.Log("Using virtual mouse");
        }

        if (screenPosition != previousScreenPosition)
        {
            usingVirtualMouse = false;
            Debug.Log("Using cursor");
        }

        previousScreenPosition = screenPosition;

        float distance = 0;
        Vector2 direction = Vector2.zero;

        if (!usingVirtualMouse && !AbilityMenu.GameIsChoose)
        {
            virtualMouse.transform.position = screenPosition;
            direction = (screenPosition - (Vector2)transform.position).normalized;
            distance = Vector2.Distance(screenPosition, transform.position);
        }

        if (usingVirtualMouse && !AbilityMenu.GameIsChoose)
        {
            direction = ((Vector2)virtualMouse.transform.position - (Vector2)transform.position).normalized;
            virtualMouse.transform.position += new Vector3(horizontal * cursorSensitivity, vertical * cursorSensitivity, 0);
            distance = Vector2.Distance(virtualMouse.transform.position, transform.position);
        }

        if (isFacingRight)
        {
            Debug.DrawRay(transform.position + new Vector3(0.5f, 0f), direction * distance);
            hit = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0f), direction, distance);
        }
        else
        {
            Debug.DrawRay(transform.position + new Vector3(-0.5f, 0f), direction * distance);
            hit = Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0f), direction, distance);
        }

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Debug.Log("Hitting player");
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }

        return hit;
    }

    public void Ascend(bool ascend)
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.up, ascendRange);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 0.5f), Vector2.up * ascendRange, Color.red);

        if (hit.collider != null && hit.collider.CompareTag("Ascendable"))
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

    public void Kinesis(Vector3 mousePosition, bool hold, float horizontal, float vertical)
    {
        RaycastHit2D hit = Raycast(mousePosition, horizontal, vertical);

        if (hold)
        {
            if (hit.collider != null && hit.collider.CompareTag("Moveable"))
            {
                Debug.Log("Moveable");
                movingObject = hit.collider.attachedRigidbody;
            }

            if (movingObject != null && hit.distance < kinesisRange)
            {
                Vector2 targetPosition = (Vector2)virtualMouse.transform.position;
                movingObject.transform.rotation = Quaternion.identity;
                movingObject.MovePosition(targetPosition);

                if (movingObject.transform.rotation == Quaternion.identity)
                    movingObject.freezeRotation = true;
            }
            else
            {
                movingObject.velocity = Vector2.zero;
                movingObject = null;
            }
        }
        else
        {
            if (movingObject != null)
            {
                movingObject.velocity = Vector2.zero;
                movingObject.freezeRotation = false;
                movingObject = null;
            }

            virtualMouse.transform.position = transform.position;
            virtualMouse.SetActive(false);
        }
    }

    public void Reverse(Vector3 mousePosition, bool hold, float horizontal, float vertical)
    {
        RaycastHit2D hit = Raycast(mousePosition, horizontal, vertical);

        if (hold)
        {
            if (hit.collider != null && hit.collider.CompareTag("Moveable"))
            {
                Debug.Log("Moveable");
                reversingObject = hit.collider.gameObject;
                tb = reversingObject.GetComponent<TimeBody>();
            }

            if (reversingObject != null && hit.distance < kinesisRange)
            {
                if (tb != null)
                    tb.StartReverse();
            }
        }
        else
        {
            if (reversingObject != null)
            {
                if (tb != null)
                    tb.StopReverse();
                reversingObject = null;
            }

            virtualMouse.transform.position = transform.position;
            virtualMouse.SetActive(false);
        }
    }

    public void Cryosis(bool hold)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0f, -0.5f), Vector2.down, 1);

        if (hit.collider != null)
        {
            icedObject = hit.collider.gameObject;
            ib = icedObject.GetComponent<IceBody>();

            if (ib != null)
                onIce = ib.isIced;
        }

        if (hold)
        {
            if (ib != null)
            {
                ib.IceObject();

                if (onIce)
                {
                    speed = cryosisSpeed;
                }

                if (IsWalled() || IsObjected())
                {
                    speed = normalSpeed;
                }
            }
        }
        else
        {
            speed = normalSpeed;
        }

        if (AbilityMenu.GameIsChoose || PauseMenu.GameIsPaused)
        {
            speed = normalSpeed;
        }
    }
}
