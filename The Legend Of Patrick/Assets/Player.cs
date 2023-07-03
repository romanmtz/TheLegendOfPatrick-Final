using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int healthPoints = 3;
    public float speed =  3.0f;
    public float jumpforce = 5f;

    Rigidbody2D rb;
    //Awake is called before start
    void Awake()
    {

        Debug.Log("Awake is Called");

        rb = GetComponent<Rigidbody2D>();
       
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start is Called");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("new frame");

    }

    public void Move(int direction){

        // transform.position += direction * speed * Time.deltaTime;
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

    }

    public void Jump(){

       rb.AddForce(Vector3.up * jumpforce, ForceMode2D.Impulse);

    }

}
