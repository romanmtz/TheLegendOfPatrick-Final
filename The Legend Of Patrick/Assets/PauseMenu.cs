using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
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
