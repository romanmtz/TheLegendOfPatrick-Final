using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public Toggle toggle;


    public AudioHandler audioHandler;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    void Awake()
    {


        if(Screen.fullScreen == true){

            toggle.isOn = true;

        }


        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentRefresh = Screen.currentResolution.refreshRate;
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {

            string option = resolutions[i].width + " x " + resolutions[i].height + "@" + resolutions[i].refreshRate + "hz";
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                currentResolutionIndex = i;

        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }
    public void SetFullscreen(bool isFullscreen)
    {

        Screen.fullScreen = isFullscreen;

    }

    public void SetResolution(int resolutionIndex)
    {

        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetMasterVolume(float volume)
    {

        audioHandler.audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        Save();

    }
    public void SetMusicVolume(float volume)
    {

        audioHandler.audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        Save();

    }
    public void SetSFXVolume(float volume)
    {

        audioHandler.audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        Save();

    }

    public void Load()
    {
        float masterVolume = PlayerPrefs.GetFloat("masterVolume");
        float musicVolume = PlayerPrefs.GetFloat("musicVolume");
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume");


        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);

        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;



    }
    private void Save()
    {
        PlayerPrefs.SetFloat("masterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);

    }


}
