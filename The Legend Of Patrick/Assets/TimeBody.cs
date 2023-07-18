using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{

    public bool isReversing = false;

    List<PointInTime> points;
    Rigidbody2D rb;


    private void Awake()
    {
        points = new List<PointInTime>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void StartReverse()
    {


        isReversing = true;
        rb.isKinematic = true;
        Debug.Log("reversing");


    }
    public void StopReverse()
    {


        isReversing = false;
        rb.isKinematic = false;
        Debug.Log("Stop Reversing");


    }

    public void Reverse()
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
