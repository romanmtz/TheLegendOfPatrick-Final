using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerInputController : MonoBehaviour
{

    Player roman;
    public PauseMenu menu;
    public AbilityMenu amenu;

   



    void Awake()
    {

        roman = GetComponent<Player>();
       


    }




    void Update()
    {

        if (!PauseMenu.GameIsPaused)
            roman.Movement(Input.GetAxisRaw("Horizontal"), Input.GetButtonDown("Jump"), Input.GetButtonUp("Jump"));

        if (Input.GetButtonDown("Pause") && !AbilityMenu.GameIsChoose)
        {
            menu.Pause();
        }


        if (PauseMenu.GameIsPaused)
            Debug.Log("GameIsPaused");

    }


    void FixedUpdate()
    {

       
        
        if (!PauseMenu.GameIsPaused)
        {
            amenu.Choose(Input.GetButton("Abilities"));
        }
    }




}
