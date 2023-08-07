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
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator DeathTransition()
    {


        transition.SetTrigger("Death");
        yield return new WaitForSeconds(transitionTime);

    }

}
