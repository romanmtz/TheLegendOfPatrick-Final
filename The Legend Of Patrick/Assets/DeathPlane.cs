using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    GameObject player;
    Rigidbody2D rb;

    private void Awake() {
        player = GameObject.FindWithTag("Player");
        rb = player.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.CompareTag("Player")){
            rb.transform.position = Player.LastCheckpoint;
            other.attachedRigidbody.velocity = Vector2.zero;
        }


    }
}
