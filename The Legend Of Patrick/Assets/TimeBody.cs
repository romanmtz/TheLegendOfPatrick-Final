using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{

    
    
    public bool isStill = false;
    public bool isReversing = false;
    float originalGScale;
    public float stillTime = 1f;
    public float timer = 0;


    public List<PointInTime> points;
    Rigidbody2D rb;


    private void Awake()
    {
        points = new List<PointInTime>();
        rb = GetComponent<Rigidbody2D>();
        originalGScale = rb.gravityScale;
    }
    public void StartReverse()
    {


        isReversing = true;
        rb.isKinematic = true;
        timer = stillTime;
        Debug.Log("reversing");


    }
    public void StopReverse()
    {


        isReversing = false;
        timer = 0;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.isKinematic = false;
        Debug.Log("Stop Reversing");


    }


    public void Reverse()
    {


        if (timer > 0)
        {
            timer -= Time.fixedDeltaTime;

            isStill = true;

            rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {

            isStill = false;
            rb.constraints = RigidbodyConstraints2D.None;


        }


        if (!isStill)
        {

            if (points.Count > 0)
            {
                PointInTime point = points[0];
                rb.MovePosition(point.position);
                rb.MoveRotation(point.rotation);
                points.RemoveAt(0);
            }
            else
            {

                StopReverse();

            }
        }

    }
    public void Record()
    {
        if ((points.Count > Mathf.Round(10f / Time.fixedDeltaTime)))
            points.RemoveAt(points.Count - 1);

        points.Insert(0, new PointInTime(transform.position, transform.rotation));


    }

    void FixedUpdate()
    {
        if (isReversing)
        {

            Reverse();

        }
        else
        {

            Record();
        }


    }







}
