using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReset : MonoBehaviour
{

    Vector2 initPosition;
    Quaternion initRotation;
    Rigidbody2D rb;
    TimeBody tb;


    void Start()
    {

        initPosition = transform.position;
        initRotation = transform.rotation;
        rb = GetComponent<Rigidbody2D>();
        tb = GetComponent<TimeBody>();

    }

    public void Reset()
    {



        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        transform.position = initPosition;
        transform.rotation = initRotation;
        tb.points = new List<PointInTime>();


    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeathPlane"))
            Reset();
    }


}
