using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class AbilityMenu : MonoBehaviour
{

    public EventSystem pauseEventSystem;
    public GameObject selectedMode;
    public GameObject abilityMenuUI;
    public GameObject abilityFirstButton;
    public GameObject ascendButton;
    public GameObject kinesisButton;
    public GameObject reverseButton;
    public GameObject cryosisButton;
    public MenuInputSwitcher menuInputSwitcher;
    public TextMeshProUGUI abilitytext;
    public static bool GameIsChoose = false;
    public static bool MenuBlock = false;
    public static string AbilityMode;

    Rigidbody2D rb;

    private void Awake()
    {

        abilityMenuUI.SetActive(false);
        rb = GetComponent<Rigidbody2D>();

    }

    public void Choose(bool choose)
    {

        if (!MenuBlock)
        {
            if (choose)
            {
                if (Time.timeScale != 0.1f)
                {
                    abilityMenuUI.SetActive(true);
                    Time.timeScale = 0.1f;
                    GameIsChoose = true;

                    menuInputSwitcher.SwitchToAbilityEventSystem();

                }

            }
            else
            {

                abilityMenuUI.SetActive(false);
                Time.timeScale = 1;
                GameIsChoose = false;

            }
        }


    }

    bool isButtonSelected(GameObject comparedButton)
    {

        return selectedMode == comparedButton;

    }


    private void Update()
    {

        if (EventSystem.current != null && EventSystem.current != pauseEventSystem)
        {
            if (EventSystem.current.currentSelectedGameObject != abilityFirstButton)
                selectedMode = EventSystem.current.currentSelectedGameObject;
        }


        if (Input.GetAxisRaw("Mouse X") == 0 && Input.GetAxisRaw("Mouse Y") == 0 && GameIsChoose)
        {

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(abilityFirstButton);

        }

        if (isButtonSelected(ascendButton))
        {
            abilitytext.text = "ascend mode";
            abilitytext.color = new Color(0.6981132f,0.3187611f,0.6594009f,1f);
            AbilityMode = "ascend";
        }

        if (isButtonSelected(kinesisButton))
        {

            abilitytext.text = "kinesis mode";
            abilitytext.color = new Color(0.8113208f,0.2264098f,0.125525f,1f);
            AbilityMode = "kinesis";

        }
        if (isButtonSelected(cryosisButton))
        {

            abilitytext.text = "cryosis mode";
            abilitytext.color = new Color(0.1803922f,0.5744293f,0.8196079f,1f);
            AbilityMode = "cryosis";

        }
        if (isButtonSelected(reverseButton))
        {

            abilitytext.text = "reverse mode";
            abilitytext.color = new Color(0.1353685f,0.6132076f,0.226305f,1f);
            AbilityMode = "reverse";

        }

        if (!GameIsChoose)
        {
            menuInputSwitcher.TurnOffEventSystems(true);
        }




    }



}
