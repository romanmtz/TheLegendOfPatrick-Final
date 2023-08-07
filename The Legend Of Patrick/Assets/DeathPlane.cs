using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    GameObject player;
    Rigidbody2D rb;
    GameObject[] objects;
    GameObject canvas;

    public LevelLoader levelLoader;

    private void OnTriggerEnter2D(Collider2D other)
    {


        if (other.CompareTag("Player"))
        {


            rb = other.attachedRigidbody;
            StartCoroutine(levelLoader.DeathTransition());
            Invoke(nameof(PlayerDeath), levelLoader.transitionTime - 0.2f);

        }



    }

    void PlayerDeath()
    {

        

        objects = GameObject.FindGameObjectsWithTag("Moveable");
        foreach (GameObject obj in objects)
        {
            obj.GetComponent<ObjectReset>().Reset();

        }

        rb.transform.position = Player.LastCheckpoint;
        rb.velocity = Vector2.zero;

    }

}
