using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 *  @file LoadPrefs.cs
 *  @author Davide Giovanetti
 *  @detail the LoadPrefs class will provide the authorized scenes
 *  with the settings selected by the player
 */

public class LoadPrefs : MonoBehaviour
{
    [Header("General Setting")]
    //true = we can use preferences
    [SerializeField] private bool canUse = false;
    [SerializeField] private MenuController menuController;

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue;
    [SerializeField] private Slider volumeSlider;

    [Header("Brightness Setting")]
    [SerializeField] private Slider brightnesSlider;
    [SerializeField] private TMP_Text brightnessTextValue;

    [Header("Quality Level Setting")]
    [SerializeField] private TMP_Dropdown qualityDropdown;

    [Header("Fullscreen Setting")]
    [SerializeField] private Toggle fullScreenToggle;

    [Header("Sensitivity Setting")]
    [SerializeField] private TMP_Text controllerSenTextValue;
    [SerializeField] private Slider controllerSenSlider;

    [Header("Invert Y Setting")]
    [SerializeField] private Toggle invertYToggle;

    /// <summary>
    /// Settings Load
    /// </summary>
    /// <remarks>
    /// if the scene has permission, I load the settings selected by the player, otherwise I load the default settings
    /// </remarks>
    private void Awake2()
    {
        if (canUse)
        {
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");
                volumeTextValue.text = localVolume.ToString("0.0");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else
            {
                menuController.ResetButton("Audio");
            }

            if (PlayerPrefs.HasKey("masterQuality"))
            {
                int localQuality = PlayerPrefs.GetInt("masterQuality");
                qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }

            if (PlayerPrefs.HasKey("masterFullscreen"))
            {
                int localFullscreen = PlayerPrefs.GetInt("masterQuality");
                if (localFullscreen == 1)
                {
                    Screen.fullScreen = true;
                    fullScreenToggle.isOn = true;
                }
                else
                {
                    Screen.fullScreen = false;
                    fullScreenToggle.isOn = false;
                }
            }

            if (PlayerPrefs.HasKey("masterBrightness"))
            {
                float localBrightness = PlayerPrefs.GetFloat("masterBrightness");
                brightnessTextValue.text = localBrightness.ToString("0.0");
                brightnesSlider.value = localBrightness;
                //change the brightness
            }

            if (PlayerPrefs.HasKey("masterSen"))
            {
                float localSensitivity = PlayerPrefs.GetFloat("masterSen");

                controllerSenTextValue.text = localSensitivity.ToString("0");
                controllerSenSlider.value = localSensitivity;
                menuController.mainControllerSensibility = Mathf.RoundToInt(localSensitivity);
            }

            if (PlayerPrefs.HasKey("masterInvertY"))
            {
                invertYToggle.isOn = PlayerPrefs.GetInt("masterInvertY") == 1;
            }
        }
    }
}
