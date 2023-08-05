using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public static AudioHandler singleton;
    AudioSource audioSource;

    void Awake()
    {
        if (singleton != null)
            Destroy(this.gameObject);
        else
            singleton = this;

    }

    public void PlaySound(AudioClip ac)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = ac;
        audioSource.Play();
    }
    public void LoopSound(AudioClip ac)
    {

        audioSource = GetComponent<AudioSource>();
        if (audioSource.isPlaying == false)
        {
            
            audioSource.clip = ac;
            audioSource.Play();
        }

    }
    public void StopLoop(AudioClip ac)
    { 
        audioSource = GetComponent<AudioSource>();
        if(audioSource.clip == ac)
            audioSource.Stop();
        
    }


}
