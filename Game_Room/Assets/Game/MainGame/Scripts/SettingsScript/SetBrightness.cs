using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SetBrightness : MonoBehaviour
{
    public PostProcessProfile brightness;
    public PostProcessLayer layer;
    private AutoExposure exposure;
    void Awake()
    {
        brightness.TryGetSettings(out exposure);
        exposure.keyValue.value = PlayerPrefs.GetFloat("masterBrightness");
    }
}
