using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBody : MonoBehaviour
{
    public bool isIced = false;
    public float freezeTime = 1;
    public float timer = 0;
    Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void IceObject()
    {
        timer = freezeTime;
    }

    void FixedUpdate()
    {

        if (timer > 0)
        {
            timer -= Time.fixedDeltaTime;
            isIced = true;

        }
        else
        {
            isIced = false;
        }

    }
}
