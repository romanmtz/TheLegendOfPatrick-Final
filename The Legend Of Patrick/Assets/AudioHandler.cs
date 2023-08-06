using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public static AudioHandler singleton;
    public AudioMixer audioMixer;
    public SettingsMenu settingsMenu;

    AudioSource audioSource;

    void Awake()
    {
        if (singleton != null)
            Destroy(this.gameObject);
        else
            singleton = this;

        if (!PlayerPrefs.HasKey("masterVolume"))
        {
            PlayerPrefs.SetFloat("masterVolume", 1f);
            PlayerPrefs.SetFloat("musicVolume", 0.5f);
            PlayerPrefs.SetFloat("sfxVolume", 0.5f);

            settingsMenu.Load();
        }
        else
        {
            settingsMenu.Load();
        }

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
        if (audioSource.clip == ac)
            audioSource.Stop();

    }



}
