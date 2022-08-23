using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//barra generale
public class GeneralBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxBar(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    public void SetBar(float health)
    {
        slider.value = health;
    }

    public void TakeDamage(float damage)
    {
        slider.value -= damage;
    }
    public void TakeHealth(float damage)
    {
        slider.value += damage;
    }

    public float GetValue()
    {
        return slider.value;
    }
}
