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

public class GenericLoadPrefs : MonoBehaviour
{
    [Header("General Setting")]
    //true = we can use preferences
    [SerializeField] private bool canUse = false;
    [SerializeField] private bool isMenu = false;
    [SerializeField] private MenuController menuController;

    [Header("Invert Y Setting")]
    [SerializeField] private Toggle invertYToggle;

    /// <summary>
    /// Settings Load
    /// </summary>
    /// <remarks>
    /// if the scene has permission, I load the settings selected by the player, otherwise I load the default settings
    /// </remarks>
    private void Awake()
    {
        if (canUse)
        {
            if (isMenu)
                LoadMenuPref();
            else
                LoadLevelPref();
        }
    }

    private void LoadMenuPref()
    {
        CommonPref();
        if (PlayerPrefs.HasKey("masterSen"))
        {
            float localSensitivity = PlayerPrefs.GetFloat("masterSen");
            menuController.mainControllerSensibility = Mathf.RoundToInt(localSensitivity);
        }

        if (PlayerPrefs.HasKey("masterInvertY"))
        {
            invertYToggle.isOn = PlayerPrefs.GetInt("masterInvertY") == 1;
        }
    }

    private void LoadLevelPref()
    {
        CommonPref();
        if (PlayerPrefs.HasKey("masterSen"))
        {
            float localSensitivity = PlayerPrefs.GetFloat("masterSen");
        }
    }

    private void CommonPref()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            float localVolume = PlayerPrefs.GetFloat("masterVolume");
            AudioListener.volume = localVolume;
        }
        if (PlayerPrefs.HasKey("masterQuality"))
        {
            int localQuality = PlayerPrefs.GetInt("masterQuality");
            QualitySettings.SetQualityLevel(localQuality);
        }

        if (PlayerPrefs.HasKey("masterFullscreen"))
        {
            int localFullscreen = PlayerPrefs.GetInt("masterQuality");
            Screen.fullScreen = localFullscreen == 1;

        }

        if (PlayerPrefs.HasKey("masterBrightness"))
        {
            float localBrightness = PlayerPrefs.GetFloat("masterBrightness");
            //change the brightness
        }

    }
}
