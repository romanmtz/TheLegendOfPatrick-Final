using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuInputSwitcher : MonoBehaviour
{

    public GameObject PauseEventSystem;
    public GameObject AbilityEventSystem;


    // private void Awake() {
    //     SwitchToPauseEventSystem();
    // }
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

}
