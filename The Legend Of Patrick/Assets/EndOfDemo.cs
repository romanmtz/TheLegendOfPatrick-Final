using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfDemo : MonoBehaviour
{

    public Dialogue dialogue;
    public LevelLoader levelLoader;

    private void Awake() {
        dialogue.StartDialogue();
    }
    

    private void Update() {
        if(!dialogue.gameObject.activeSelf)
        StartCoroutine(levelLoader.LoadLevel("Main Menu"));
    }


}
