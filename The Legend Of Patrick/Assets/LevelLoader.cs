using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class LevelLoader : MonoBehaviour
{

    public Animator transition;
    public float transitionTime = 1f;

    public IEnumerator LoadLevel(string sceneName)
    {
        AbilityMenu.MenuBlock = true;
        PauseMenu.MenuBlock = true;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        PauseMenu.MenuBlock = false;
        AbilityMenu.MenuBlock = false;

        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator DeathTransition()
    {
        AbilityMenu.MenuBlock = true;
        PauseMenu.MenuBlock = true;
        transition.SetTrigger("Death");
        yield return new WaitForSeconds(transitionTime + 0.5f);
        PauseMenu.MenuBlock = false;
        AbilityMenu.MenuBlock = false;

    }



}
