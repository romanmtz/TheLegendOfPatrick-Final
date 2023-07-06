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

       roman.Move(Input.GetAxisRaw("Horizontal"));

    }




}
