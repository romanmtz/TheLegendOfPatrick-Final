using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;


    public float speed =  3.0f;
    public float buttonTime = 0.5f;
    public float jumpHeight = 5;
    public float cancelRate = 100;
    public Transform wallCheck;
    public LayerMask wallLayer;

    float isWallSliding;
    float wallSlidingSpeed = 2f;

    float jumpTime;
    bool jumping;
    bool jumpCancelled;
    bool isGrounded;
    
    
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    

    public void Move(float direction){

        
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
         //wall slide
        
    

    }

    public void Jump(bool jump){
        
        if(jump && isGrounded){
        float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        jumping = true;
        jumpCancelled = false;
        jumpTime = 0;
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

    // bool IsWalled(){

    //     return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);

    // }

    // void WallSlide(){

    //     if(IsWalled() && !isGrounded() && )

    // }


    void Update(){

        if(rb.velocity.y == 0){
            isGrounded = true;
        }
        else{
            isGrounded = false;
        }
    }

    

    void FixedUpdate()
    {

        if(jumpCancelled && jumping && rb.velocity.y > 0)
        {
            rb.AddForce(Vector2.down * cancelRate);
        }
    }

}