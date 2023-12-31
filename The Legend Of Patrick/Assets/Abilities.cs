using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    Player roman;

    public AudioClip ascendSFX;
    public AudioClip kinesisSFX;
    public AudioClip reverseSFX;
    public LayerMask objectLayer;
    //Ascend variables
    public float ascendRange = 5f;


    //Raycast variables
    Vector2 direction;
    float distance;
    bool isRaycasting = false;
    public GameObject virtualMouse;
    public float cursorSensitivity = 0.002f;
   


    // Kinesis variables
    Rigidbody2D movingObject;
    bool hasMovingObject = false;
    public float kinesisRange = 20f;
    
    
    
    

    // Reverse variables
    TimeBody tb;
    GameObject reversingObject = null;
    bool hasTimeBody = false;
    bool isReversing = false;
    public float reverseRange = 20f;

    // Cryosis variables
    GameObject icedObject;
    IceBody ib;
    bool hasIceBody = false;
    public float cryosisSpeed = 18f;
    public static bool onIce = false;
    public static bool isSkating = false;
    public static bool isSkateJumping = false;


    private void Awake()
    {
        roman = GetComponent<Player>();
    }

    private void FixedUpdate()
    {

        if (isRaycasting == false)
        {
            virtualMouse.transform.position = transform.position;
            virtualMouse.SetActive(false);

        }


    }


    RaycastHit2D Raycast(Vector3 mousePosition, float horizontal, float vertical)
    {
        RaycastHit2D hit;

        isRaycasting = true;



        if (!AbilityMenu.GameIsChoose)
        {
            virtualMouse.transform.position += new Vector3(horizontal * cursorSensitivity, vertical * cursorSensitivity, 0);
            direction = ((Vector2)virtualMouse.transform.position - (Vector2)transform.position).normalized;
            distance = Vector2.Distance(virtualMouse.transform.position, transform.position);
        }

        if (roman.IsFacingRight())
        {
            Debug.DrawRay(transform.position + new Vector3(0.5f, 0f), direction * distance);
            hit = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0f), direction, distance, objectLayer);

        }
        else
        {
            Debug.DrawRay(transform.position + new Vector3(-0.5f, 0f), direction * distance);
            hit = Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0f), direction, distance, objectLayer);

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
                AudioHandler.singleton.PlaySound(ascendSFX);
                Bounds colliderBounds = hit.collider.bounds;
                float top = colliderBounds.max.y;
                transform.position = new Vector2(transform.position.x, top);
            }
        }
    }

    public void Kinesis(Vector3 mousePosition, bool hold, float horizontal, float vertical)
    {

        RaycastHit2D hit = Raycast(mousePosition, horizontal, vertical);

        if (hit.collider != null && hit.collider.CompareTag("Moveable") && !hasMovingObject)
        {
            Debug.Log("Moveable");
            movingObject = hit.collider.attachedRigidbody;
            hasMovingObject = true;

        }
        else
        {
            if(hit.collider == null)
            hasMovingObject = false;
        }

        if (hold)
        {

            AudioHandler.singleton.LoopSound(kinesisSFX);
            AbilityMenu.MenuBlock = true;
            virtualMouse.SetActive(true);

            if (hasMovingObject && distance < kinesisRange)
            {


                Vector2 targetPosition = (Vector2)virtualMouse.transform.position;
                movingObject.transform.rotation = Quaternion.identity;

                if (!roman.IsObjected())
                {
                    movingObject.MovePosition(targetPosition);
                }


                if (movingObject.transform.rotation == Quaternion.identity)
                    movingObject.freezeRotation = true;
            }

        }
        else
        {

            AbilityMenu.MenuBlock = false;
            if (hasMovingObject)
            {
                movingObject.velocity = Vector2.zero;
                movingObject.freezeRotation = false;
            }

            hasMovingObject = false;
            isRaycasting = false;

        }
    }

    public void Reverse(Vector3 mousePosition, bool hold, float horizontal, float vertical)
    {
        RaycastHit2D hit = Raycast(mousePosition, horizontal, vertical);


        if (hit.collider != null && hit.collider.CompareTag("Moveable"))
        {
            Debug.Log("Moveable");
            reversingObject = hit.collider.gameObject;

            if (reversingObject.TryGetComponent<TimeBody>(out TimeBody hinge))
            {
                hasTimeBody = true;
            }
            else
            {
                hasTimeBody = false;
            }

        }

        if (hold)
        {

            AudioHandler.singleton.LoopSound(kinesisSFX);


            AbilityMenu.MenuBlock = true;
            virtualMouse.SetActive(true);
            if (hasTimeBody && !isReversing)
            {

                tb = reversingObject.GetComponent<TimeBody>();

                tb.StartReverse();
                AudioHandler.singleton.PlaySound(reverseSFX);
                isReversing = true;
            }

        }
        else
        {
            AbilityMenu.MenuBlock = false;

            if (hasTimeBody)
                tb.StopReverse();

            hasTimeBody = false;
            isReversing = false;
            isRaycasting = false;

        }
    }

    public void Cryosis(bool hold)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0f, -0.5f), Vector2.down, 1);

        if (hit.collider != null)
        {
            icedObject = hit.collider.gameObject;

            if (icedObject.TryGetComponent<IceBody>(out IceBody hinge))
            {
                hasIceBody = true;
            }
            else
            {
                hasIceBody = false;
            }
        }


        bool isGrounded = roman.IsGrounded();
        bool isWalled = roman.IsWalled();
        bool isObjected = roman.IsObjected();
        float originalSpeed = roman.ChangeSpeed(0);

        if (hold)
        {
            AbilityMenu.MenuBlock = true;

            if (hasIceBody)
            {
                ib = icedObject.GetComponent<IceBody>();
                ib.IceObject();

                if (isGrounded)
                {
                    roman.ChangeSpeed(cryosisSpeed);
                    isSkating = true;
                    isSkateJumping = false;
                }
                else if (!isSkateJumping && !isWalled && !isObjected)
                {
                    roman.ChangeSpeed(originalSpeed);
                    isSkateJumping = true;
                    isSkating = false;
                }
            }
            else
            {
                if (!isSkateJumping)
                    roman.ChangeSpeed(originalSpeed);

                if (isGrounded)
                {
                    isSkating = false;
                    isSkateJumping = false;
                }
            }
        }
        else
        {
            AbilityMenu.MenuBlock = false;

            if (!isSkateJumping)
                roman.ChangeSpeed(originalSpeed);

            if (isGrounded)
            {
                isSkating = false;
                isSkateJumping = false;
            }
        }

        // If the character is walled or objected, reset skating states and speed.
        if (isWalled || isObjected)
        {
            isSkating = false;
            isSkateJumping = false;
            roman.ChangeSpeed(originalSpeed);
        }
    }
}
