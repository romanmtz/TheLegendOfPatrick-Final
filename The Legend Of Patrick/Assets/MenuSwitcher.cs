using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuSwitcher : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject settingsFirstSelectedGameObject;
    public LevelLoader levelLoader;
    bool isMainMenu = true;
    bool isSettingsMenu = false;
    Scene m_Scene;
    string sceneName;

    private void Awake()
    {

        
        m_Scene = SceneManager.GetActiveScene();
        string sceneName = m_Scene.name;
        Cursor.visible = false;
       


    }
    void Update()
    {

        

        if ((Input.GetButtonDown("Cancel") || Input.GetButtonDown("Pause")) && sceneName == "Main Menu")
        {

            if (GameObject.Find("Blocker") == null)
            {
                if (isMainMenu)
                    SettingsMenu();
                else 
                    MainMenu();
            }


        }
        else if(Input.GetButtonDown("Cancel") || Input.GetButtonDown("Pause")){
            
            if(isSettingsMenu)
            MainMenu();

        }



    }
    public void SettingsMenu()
    {

        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(settingsFirstSelectedGameObject);
        isMainMenu = false;
        isSettingsMenu = true;

    }
    public void MainMenu()
    {

        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
        isMainMenu = true;
        isSettingsMenu = false;
        

    }


    public void StartGame()
    {
        
        StartCoroutine(levelLoader.LoadLevel("Ascend Level"));

    }
    public void Quit()
    {
        Application.Quit();
    }
}
