using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    [SerializeField] private AudioMixer audioMixer;

    [Tooltip("Resolution Dropdown from TextMeshPro")]
    [SerializeField] private TMPro.TMP_Dropdown TMProResolutionDropdown;
    [Tooltip("Default Resolution Dropdown")]
    [SerializeField] private Dropdown defaultResolutionDropdown;
    
    private Resolution[] resolutions;
    private int currentResolutionIndex;

    private void Start()
    {
        resolutions = Screen.resolutions;

        //Handles resolutions for default dropdown
        if (defaultResolutionDropdown != null)
        {
            defaultResolutionDropdown.ClearOptions();

            defaultResolutionDropdown.AddOptions(ResolutionsFunction());
            defaultResolutionDropdown.value = currentResolutionIndex;
            defaultResolutionDropdown.RefreshShownValue();
        }
        //Handles resolutions for TextMeshPro dropdown
        else if (TMProResolutionDropdown != null)
        {
            TMProResolutionDropdown.ClearOptions();

            TMProResolutionDropdown.AddOptions(ResolutionsFunction());
            TMProResolutionDropdown.value = currentResolutionIndex;
            TMProResolutionDropdown.RefreshShownValue();
        }
    }

    //Gets list of resolutions as a string and returns
        //Also sets default resolution
    private List<string> ResolutionsFunction()
    {
        //fill options list with strings of resolutions
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            
            //finds current running resolution to set as default
            if (resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        return options;
    }


    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }


    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
