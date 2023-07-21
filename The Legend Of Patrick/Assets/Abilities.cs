using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    Player roman;
    public float ascendRange = 5f;

    //Raycast variables
    Vector2 direction;
    float distance;
    bool isRaycasting = false;


    // Kinesis variables
    public float kinesisRange = 20f;
    public GameObject virtualMouse;
    bool usingVirtualMouse = false;
    bool hasMovingObject = false;
    Vector2 previousScreenPosition;
    public float cursorSensitivity = 0.2f;
    Rigidbody2D movingObject;

    // Reverse variables
    public float reverseRange = 20f;
    GameObject reversingObject = null;
    TimeBody tb;
    GameObject previousReversingObject;
    bool hasTimeBody = false;
    bool isReversing = false;

    // Cryosis variables
    GameObject icedObject;
    IceBody ib;
    public float cryosisSpeed = 18f;
    public static bool onIce = false;
    bool normalCancel = false;
    public PhysicsMaterial2D[] material;
    bool hasIceBody = false;
    bool isSkating = false;
    bool isSkateJumping = false;

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
            hit = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0f), direction, distance);
        }
        else
        {
            Debug.DrawRay(transform.position + new Vector3(-0.5f, 0f), direction * distance);
            hit = Physics2D.Raycast(transform.position + new Vector3(-0.5f, 0f), direction, distance);
        }

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            roman.Flip();
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

        if (hit.collider != null && hit.collider.CompareTag("Moveable"))
        {
            Debug.Log("Moveable");
            movingObject = hit.collider.attachedRigidbody;
            hasMovingObject = true;

        }
        else
        {
            hasMovingObject = false;
        }

        if (hold)
        {
            AbilityMenu.MenuBlock = true;
            virtualMouse.SetActive(true);

            if (hasMovingObject && distance < kinesisRange)
            {

                Vector2 targetPosition = (Vector2)virtualMouse.transform.position;
                movingObject.transform.rotation = Quaternion.identity;
                movingObject.MovePosition(targetPosition);


                //add a speed limiter later

                if (movingObject.transform.rotation == Quaternion.identity)
                    movingObject.freezeRotation = true;
            }

        }
        else
        {
            if (hasMovingObject)
            {
                movingObject.velocity = Vector2.zero;
                movingObject.freezeRotation = false;
            }

            hasMovingObject = false;

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
            AbilityMenu.MenuBlock = true;
            virtualMouse.SetActive(true);
            if (hasTimeBody && !isReversing)
            {
                tb = reversingObject.GetComponent<TimeBody>();
                tb.StartReverse();
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
