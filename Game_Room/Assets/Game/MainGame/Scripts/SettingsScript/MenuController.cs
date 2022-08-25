using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class MenuController : MonoBehaviour
{
    [Header("Levels To Load")]
    public string _newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noSavedGameDialog;

    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private float defaultVolume = 0.7f;
    private float currentVolumeVal;

    [Header("GameplaySettings")] 
    [SerializeField] private TMP_Text controllerSenTextValue;
    [SerializeField] private Slider controllerSenSlider; 
    [SerializeField] private int defaultSen = 4;
    public int mainControllerSensibility = 4;
    [SerializeField] private Toggle invertYToggle;
    private float currentSenVal;
    private bool currentInvertYVal;
    

    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnesSlider;
    [SerializeField] private TMP_Text brightnessTextValue;
    [SerializeField] private float defaultBrightness = 1;
    private int qualityLevel;
    private bool isFullScreen;
    private float brightnessLevel;

    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;


    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullScreenToggle;
    
    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt;


    public void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        
    }

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGameDialog.SetActive(true);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }


    //Options

    //volume
    public void GetVolume()
    {
        currentVolumeVal = AudioListener.volume;
        SetVolume(AudioListener.volume);
        volumeSlider.value = AudioListener.volume;
    }
    public void SetVolume(float volume)
    {
        Debug.Log("SetVolume");
        currentVolumeVal = volume;
        volumeTextValue.text = (100*volume).ToString("0")+ "%";
    }
    public void VolumeApply()
    {
        Debug.Log("ApplayVolume");
        AudioListener.volume = currentVolumeVal;
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    //gameplay
    public void GetGameplayOptions()
    {
        currentSenVal = PlayerPrefs.GetFloat("masterSen");
        currentInvertYVal = PlayerPrefs.GetInt("masterInvertY") == 1;
        controllerSenTextValue.text = currentSenVal.ToString("0");
        mainControllerSensibility = Mathf.RoundToInt(currentSenVal); ;
        controllerSenTextValue.text = currentSenVal.ToString("0");
        controllerSenSlider.value = currentSenVal;
        invertYToggle.isOn = currentInvertYVal;
    }
    public void SetControllerSensibility(float sensitivity)
    {
        mainControllerSensibility = Mathf.RoundToInt(sensitivity);
        controllerSenTextValue.text = sensitivity.ToString("0");
    }
    public void GameplayApply()
    {
        PlayerPrefs.SetInt("masterInvertY", invertYToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("masterSen", mainControllerSensibility);
        StartCoroutine(ConfirmationBox());
    }

    //graphics
    public void GetGraphicsInfo()
    {

    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetBrightness(float brightness)
    {
        brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
    }
    public void SetFullScreen(bool isFullScreen)
    {
        this.isFullScreen = isFullScreen;
    }
    public void SetQuality(int qualityLv)
    {
        qualityLevel = qualityLv;
    }
    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", brightnessLevel);
        //change your brightness with your post processing or whatever it is

        PlayerPrefs.SetInt("masterQuality", qualityLevel);
        QualitySettings.SetQualityLevel(qualityLevel);

        PlayerPrefs.SetInt("masterFullscreen", isFullScreen ? 1 : 0);
        Screen.fullScreen = isFullScreen;

        StartCoroutine(ConfirmationBox());
    }


    //altro
    public void ResetButton(string MenuType)
    {
        switch (MenuType)
        {
            case ("Audio"):
                volumeSlider.value = defaultVolume;
                SetVolume(defaultVolume);
                VolumeApply();
                break;
            case ("Gameplay"):
                controllerSenTextValue.text = defaultSen.ToString("0");
                controllerSenSlider.value = defaultSen;
                mainControllerSensibility = defaultSen;
                invertYToggle.isOn = false;
                GameplayApply();
                break;
            case ("Graphic"):
                //reset brightness
                brightnesSlider.value = defaultBrightness;
                brightnessTextValue.text = defaultBrightness.ToString("0.0");

                qualityDropdown.value = 1;
                QualitySettings.SetQualityLevel(1);
                fullScreenToggle.isOn = false;
                Screen.fullScreen = false;

                Resolution currentResolution = Screen.currentResolution;
                Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
                resolutionDropdown.value = resolutions.Length;
                GraphicsApply();
                break;
        }
    }
    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(3);
        confirmationPrompt.SetActive(false);
    }
}
