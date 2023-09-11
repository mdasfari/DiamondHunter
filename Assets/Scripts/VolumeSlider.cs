using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider volumeSlider;
    void Start()
        {

            if (!PlayerPrefs.HasKey("Music"))
            {
                PlayerPrefs.SetFloat("Music", 1);
                Load();
            }
            else
            {
                Load();
            }
        }

    // Update is called once per frame
    void Update()
        {
        
        }
    public void ChangeVolume()
        {
            AudioListener.volume = volumeSlider.value;
            Save();
        }
    public void Save()
        {
            PlayerPrefs.SetFloat("Music",volumeSlider.value);
        }
    public void Load()
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Music");
            AudioListener.volume = volumeSlider.value;
        }
}
