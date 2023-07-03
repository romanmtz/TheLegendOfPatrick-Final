using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
  
    Player roman;
    // Start is called before the first frame update
    void Awake()
    {
        roman = GetComponent<Player>();
    }

    // Update is called once per frame

    void Update(){

        if(Input.GetKeyDown(KeyCode.Space)){

            Debug.Log("Pressing Space!");
            roman.Jump();

        }

    }

    void FixedUpdate()
    {

        if(Input.GetKey(KeyCode.D)){

            Debug.Log("Pressing D!");
            roman.Move(1);

        }
        else if(Input.GetKey(KeyCode.A)){

            Debug.Log("Pressing A!");
            roman.Move(-1);

        }

        
    }




}
