using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public Dropdown textureDropdown;
    public Dropdown aaDropdown;
    public Slider volumeSlider;
    float currentVolume;
    Resolution[] resolutions;

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        currentVolume = volume;
        SaveSettings();
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        SaveSettings();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        SaveSettings();
    }

    public void SetTextureQuality(int textureIndex)
    {
        QualitySettings.masterTextureLimit = textureIndex;
        qualityDropdown.value = 6;
        SaveSettings();
    }

    public void SetAntiAliasing(int aaIndex)
    {
        QualitySettings.antiAliasing = aaIndex;
        qualityDropdown.value = 6;
        SaveSettings();
    }

    public void SetQuality(int qualityIndex)
    {
        if (qualityIndex != 6) // if the user is not using 
                               //any of the presets
            QualitySettings.SetQualityLevel(qualityIndex);
        switch (qualityIndex)
        {
            case 0: // quality level - very low
                textureDropdown.value = 3;
                aaDropdown.value = 0;
                break;
            case 1: // quality level - low
                textureDropdown.value = 2;
                aaDropdown.value = 0;
                break;
            case 2: // quality level - medium
                textureDropdown.value = 1;
                aaDropdown.value = 0;
                break;
            case 3: // quality level - high
                textureDropdown.value = 0;
                aaDropdown.value = 0;
                break;
            case 4: // quality level - very high
                textureDropdown.value = 0;
                aaDropdown.value = 1;
                break;
            case 5: // quality level - ultra
                textureDropdown.value = 0;
                aaDropdown.value = 2;
                break;
        }

        qualityDropdown.value = qualityIndex;
        SaveSettings();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySetting",
                   qualityDropdown.value);
        PlayerPrefs.SetInt("Resolution",
                   resolutionDropdown.value);
        PlayerPrefs.SetInt("TextureQuality",
                   textureDropdown.value);
        PlayerPrefs.SetInt("AntiAliasing", aaDropdown.value);
        PlayerPrefs.SetInt("Fullscreen", Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetFloat("Volume", currentVolume);
    }

    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("QualitySetting"))
            qualityDropdown.value = PlayerPrefs.GetInt("QualitySetting");
        else
            qualityDropdown.value = 3;
        if (PlayerPrefs.HasKey("Resolution"))
            resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");
        else
            resolutionDropdown.value = currentResolutionIndex;
        if (PlayerPrefs.HasKey("TextureQuality"))
            textureDropdown.value = PlayerPrefs.GetInt("TextureQuality");
        else
            textureDropdown.value = 0;
        if (PlayerPrefs.HasKey("AntiAliasing"))
            aaDropdown.value = PlayerPrefs.GetInt("AntiAliasing");
        else
            aaDropdown.value = 1;
        if (PlayerPrefs.HasKey("Fullscreen"))
            Screen.fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("Fullscreen"));
        else
            Screen.fullScreen = true;
        if (PlayerPrefs.HasKey("Volume"))
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        else
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
    }

    void Start()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " +
                     resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width
                  && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);
    }
}
