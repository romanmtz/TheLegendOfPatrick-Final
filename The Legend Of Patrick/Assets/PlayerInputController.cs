using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
  
    Player roman;


   
    void Awake()
    {
        roman = GetComponent<Player>();


    }

 

    void Update(){

        

        roman.Jump(Input.GetKeyDown(KeyCode.Space));

        
        
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
