using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityMenu : MonoBehaviour
{

    // public GameObject selectedButton;
    public EventSystem pauseEventSystem;
    public GameObject selectedMode;
    public GameObject abilityMenuUI;
    public GameObject abilityFirstButton;
    public GameObject ascendButton;
    public GameObject kinisisButton;
    public GameObject reverseButton;
    public GameObject cryosisButton;
    public MenuInputSwitcher menuInputSwitcher;
    public static bool GameIsChoose = false;



    Rigidbody2D rb;

    private void Awake()
    {

        abilityMenuUI.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
    }

    public void Choose(bool choose)
    {

        if (choose)
        {
            if (Time.timeScale != 0.1f)
            {
                abilityMenuUI.SetActive(true);
                Time.timeScale = 0.1f;
                Time.fixedDeltaTime = Time.timeScale * .02f;
                GameIsChoose = true;

                menuInputSwitcher.SwitchToAbilityEventSystem();

            }

        }
        else
        {

            abilityMenuUI.SetActive(false);
            Time.timeScale = 1;
            Time.fixedDeltaTime = .02f;
            GameIsChoose = false;

        }


    }

    bool isButtonSelected(GameObject comparedButton)
    {

        return selectedMode == comparedButton;

    }


    private void Update()
    {

        if(EventSystem.current != null && EventSystem.current != pauseEventSystem){
        if (EventSystem.current.currentSelectedGameObject != abilityFirstButton )
            selectedMode = EventSystem.current.currentSelectedGameObject;
        }


        if (Input.GetAxisRaw("Mouse X") == 0 && Input.GetAxisRaw("Mouse Y") == 0 && GameIsChoose)
        {

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(abilityFirstButton);

        }

        if (isButtonSelected(ascendButton))
        {

            Debug.Log("Ascend Mode");

        }

        if (isButtonSelected(kinisisButton))
        {

            Debug.Log("Kinisis Mode");

        }
        if (isButtonSelected(cryosisButton))
        {

            Debug.Log("Cryosis Mode");

        }
        if (isButtonSelected(reverseButton))
        {

            Debug.Log("Reverse Mode");

        }

        if(!GameIsChoose){
            menuInputSwitcher.TurnOffEventSystems(true);
        }

   


    }



}
