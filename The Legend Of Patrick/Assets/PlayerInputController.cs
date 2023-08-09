using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerInputController : MonoBehaviour
{

    Player roman;
    Abilities abilities;
    public PauseMenu menu;
    public AbilityMenu amenu;

    void Awake()
    {
        roman = GetComponent<Player>();
        abilities = GetComponent<Abilities>();
    }




    void Update()
    {
        if (Input.GetButtonDown("Pause") && !AbilityMenu.GameIsChoose && !Player.isDialogue)
        {
            menu.Pause();
        }


            if (!PauseMenu.GameIsPaused && !Player.isDialogue)
            {
                roman.Movement(Input.GetAxisRaw("Horizontal"), Input.GetButtonDown("Jump"), Input.GetButtonUp("Jump"));
            }

            if (AbilityMenu.AbilityMode == "ascend")
            {

                abilities.Ascend(Input.GetButtonUp("Fire"));

            }

            if (AbilityMenu.AbilityMode == "kinesis")
            {
                // Cursor.visible = false;
                abilities.Kinesis(Input.mousePosition, Input.GetButton("Fire"), Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            }

            if (AbilityMenu.AbilityMode == "reverse")
            {

                abilities.Reverse(Input.mousePosition, Input.GetButton("Fire"), Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            }
            if (AbilityMenu.AbilityMode == "cryosis")
            {

                abilities.Cryosis(Input.GetButton("Fire"));

            }

        

        if (PauseMenu.GameIsPaused)
            Debug.Log("GameIsPaused");

    }


    void FixedUpdate()
    {


        if (!PauseMenu.GameIsPaused && !Player.isDialogue)
        {
            amenu.Choose(Input.GetButton("Abilities"));
        }
        else
        {

            amenu.Choose(false);

        }

    }

}
