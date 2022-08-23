using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer: MonoBehaviour
{
    public AudioSource audioSource;
    public float musicVolume;

    void Start()
    {
        audioSource.Play();
        //se esiste lo setto al valore precedente altrimenti a quello di default su unity
        musicVolume = PlayerPrefs.GetFloat("VolumeStart"); 
    }

    void Update()
    {
        audioSource.volume = musicVolume;
        PlayerPrefs.SetFloat("VolumeStart", musicVolume);
    }

    public void UpdateVolume(float volume)
    {
        musicVolume = volume;
    }
}
