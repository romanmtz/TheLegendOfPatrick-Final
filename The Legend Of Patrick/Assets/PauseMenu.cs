using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject pauseFirstButton;
    public MenuInputSwitcher menuInputSwitcher;
    public static bool GameIsPaused = false;
    public GameObject settingsMenu;
    public LevelLoader levelLoader;

    void Awake()
    {

        pauseMenuUI.SetActive(false);
        if (GameIsPaused)
            Pause();

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
            settingsMenu.SetActive(false);
            GameIsPaused = false;
        }

    }


    public void MainMenu()
    {

        Pause();
        StartCoroutine(levelLoader.LoadLevel("Main Menu"));

    }

    private void Update()
    {
        if (!GameIsPaused)
        {

            menuInputSwitcher.TurnOffEventSystems(false);

        }
    }

}
