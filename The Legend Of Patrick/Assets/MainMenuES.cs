using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuES : MonoBehaviour
{
    EventSystem eventSystem;
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject settingsFirstSelectedGameObject;
    bool isMainMenu = true;
    bool isBlocked = false;

    private void Awake()
    {

        eventSystem = GetComponent<EventSystem>();


    }
    void Update()
    {


        if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Pause"))
        {

            if (GameObject.Find("Blocker") == null)
            {
                if (isMainMenu)
                    SettingsMenu();
                else
                    MainMenu();
            }


        }



    }
    public void SettingsMenu()
    {

        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(settingsFirstSelectedGameObject);
        isMainMenu = false;

    }
    public void MainMenu()
    {

        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
        isMainMenu = true;

    }

    public void StartGame()
    {
        SceneManager.LoadScene("Ascend Level");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
