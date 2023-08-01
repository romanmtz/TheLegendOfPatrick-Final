using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBody : MonoBehaviour
{
    public bool isIced = false;
    public float freezeTime = 1;
    public float timer = 0;

    Rigidbody2D rb;
    SpriteRenderer sr;

    Color originalColor;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        originalColor = new Color(sr.material.color.r,sr.material.color.g,sr.material.color.b,sr.material.color.a);
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
            sr.material.SetColor("_Color", new Color(0,1.57f,1.95f,0.5f));


        }
        else
        {

            sr.material.SetColor("_Color", originalColor);
            isIced = false;
        }

    }
}
