using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    float length, startpos;

    public GameObject Camera;
    public float parallaxEffect;

    private void Awake() {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

    }

    void FixedUpdate() {
        float temp = (Camera.transform.position.x * (1 - parallaxEffect));
        float distance = Camera.transform.position.x * parallaxEffect;
        transform.position = new Vector3(startpos + distance, transform.position.y,transform.position.z);

        if(temp > startpos + length) startpos += length;
        else if(temp < startpos - length){
            startpos -= length;
        }
    }

}
