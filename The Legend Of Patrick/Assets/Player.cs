using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float horizontal;
    public float speed = 9f;
    public float normalSpeed = 9f;
    public float cryosisSpeed = 18f;
    
    

    public float jumpingPower = 3f;
    public float iceJumpingPower = 10f;
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

    public LayerMask objectLayer;

    Transform groundCheck;
    Transform wallCheck;

    Rigidbody2D rb;

    public float ascendRange = 5f;
    public float kinesisRange = 20f;
    public float reverseRange = 20f;
    public GameObject virtualMouse;

    Vector2 previousScreenPosition;


    bool usingVirtualMouse = false;
    // public Camera mainCamera;

    Vector2 idleScreenPosition;

    public float cursorSensitivity = 0.2f;
    GameObject reversingObject;

    Rigidbody2D movingObject;

    GameObject previousReversingObject;
    GameObject icedObject;

    bool onIce = false;
bool iceJumping = false;
    public PhysicsMaterial2D[] material;

    TimeBody tb;
    IceBody ib;




    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        groundCheck = GameObject.Find("Ground Check").GetComponent<Transform>();
        wallCheck = GameObject.Find("Wall Check").GetComponent<Transform>();
        reversingObject = GetComponent<GameObject>();

    }

    public void Movement(float move, bool jump, bool jumpCancel)
    {
        horizontal = move;
        

        if (IsObjected())
            rb.sharedMaterial = material[0];
        else
            rb.sharedMaterial = material[1];

        if (jump && (IsGrounded() || IsObjected()) && onIce == false)
        {
            float jumpForce = Mathf.Sqrt(jumpingPower * -2 * (Physics2D.gravity.y * rb.gravityScale));
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            iceJumping = false;
        }
        else if(jump && (IsGrounded() || IsObjected()) && onIce == true){
            float jumpForce = Mathf.Sqrt(iceJumpingPower * -2 * (Physics2D.gravity.y * rb.gravityScale));
            rb.AddForce(new Vector2(1, jumpForce), ForceMode2D.Impulse);
            iceJumping = true;
        }


        if (jumpCancel && rb.velocity.y > 0f && iceJumping == false)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        if (jumpCancel && rb.velocity.y > 0f && iceJumping == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.005f);
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
        if (!isWallJumping && onIce == false)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        else if(!isWallJumping && onIce == true){
            rb.AddForce(new Vector2(horizontal * speed, rb.velocity.y), ForceMode2D.Force);
        }



        if (AbilityMenu.GameIsChoose)
        {

            virtualMouse.SetActive(false);

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
            Debug.Log("using virtual mouse");

        }

        if (screenPosition != previousScreenPosition)
        {

            usingVirtualMouse = false;
            Debug.Log("using cursor");
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

        if (hit.collider != null)
        {

            if (hit.collider.CompareTag("Player"))
            {

                Debug.Log("hitting player");
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;

            }
        }

        return hit;


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



    public void Kinesis(Vector3 mousePosition, bool hold, float horizontal, float vertical)
    {


        RaycastHit2D hit = Raycast(mousePosition, horizontal, vertical);


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

            if (movingObject != null)
            {

                if (hit.distance < kinesisRange)
                {
                    Vector2 targetPosition = (Vector2)virtualMouse.transform.position;

                    movingObject.transform.rotation = Quaternion.identity;
                    movingObject.MovePosition(targetPosition);

                    if ((movingObject.transform.rotation == Quaternion.identity))
                        movingObject.freezeRotation = true;



                }
                else
                {

                    movingObject.velocity = Vector2.zero;
                    movingObject = null;

                }
            }
        }

        if (!hold)
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
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Moveable"))
                {
                    Debug.Log("Moveable");
                    reversingObject = hit.collider.gameObject;
                    tb = reversingObject.GetComponent<TimeBody>();

                }

            }

            if (reversingObject != null)
            {

                if (hit.distance < kinesisRange)
                {
                    if (tb != null)
                        tb.StartReverse();

                }

            }
        }

        if (!hold)
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
        if(hit.collider != null){

            icedObject = hit.collider.gameObject;
            ib = icedObject.GetComponent<IceBody>();
            if(ib != null)
            onIce = ib.isIced;

        }
        else{

            if(!iceJumping)
            onIce = false;
        }



        if (hold)
        {
            speed = cryosisSpeed;
            if(ib != null)
            ib.IceObject();
            
            
        }
        if (!hold)
        {
            
            speed = normalSpeed;
            

        }
        if(AbilityMenu.GameIsChoose || PauseMenu.GameIsPaused){

            speed = normalSpeed;

        }
        
    }

}




