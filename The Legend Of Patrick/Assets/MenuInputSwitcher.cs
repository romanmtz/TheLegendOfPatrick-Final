using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuInputSwitcher : MonoBehaviour
{

    public GameObject PauseEventSystem;
    public GameObject AbilityEventSystem;


    private void Awake()
    {
        PauseEventSystem.SetActive(false);
        AbilityEventSystem.SetActive(false);
    }
    public void SwitchToAbilityEventSystem()
    {

        PauseEventSystem.SetActive(false);
        AbilityEventSystem.SetActive(true);
    }

    public void SwitchToPauseEventSystem()
    {
        PauseEventSystem.SetActive(true);
        AbilityEventSystem.SetActive(false);

    }
    public void TurnOffEventSystems(bool option)
    {

        if (AbilityEventSystem != null && option == true)
            AbilityEventSystem.SetActive(false);
        else{
            PauseEventSystem.SetActive(false);
        }


    }

}
