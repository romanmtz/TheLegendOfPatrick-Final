using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject pauseFirstButton;

    public MenuInputSwitcher menuInputSwitcher;

    public static bool GameIsPaused = false;
    // Start is called before the first frame update

    void Awake()
    {

        pauseMenuUI.SetActive(false);

    }
    public void Pause()
    {


        if (Time.timeScale != 0)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0;
            GameIsPaused = true;

            menuInputSwitcher.SwitchToPauseEventSystem();
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseFirstButton);

        }
        else
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1;
            GameIsPaused = false;
        }

    }
    public void MainMenu(){

        Debug.Log("Switching to Main Menu Scene!");

    }
}
