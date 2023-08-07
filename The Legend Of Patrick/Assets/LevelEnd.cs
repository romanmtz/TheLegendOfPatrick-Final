using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{

    public LevelLoader levelLoader;

    public string levelName = "Kinesis Level";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(levelLoader.LoadLevel(levelName));

        }
    }


}
