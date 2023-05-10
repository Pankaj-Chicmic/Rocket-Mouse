using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Audio : MonoBehaviour
{
    float Volume;
    [SerializeField] AudioSource CoinSound, MainSound, JetpackSound, FootSound, LaserSound;
    [SerializeField] GameObject Mouse;
    [SerializeField] Toggle VolumeToggle;
    [SerializeField] Slider VolumeSlider;
    void Start()
    {
        VolumeSlider.value = PlayerPrefs.GetFloat("Volume");
        if (PlayerPrefs.GetFloat("VolumeToggle") == 1)
        {
            VolumeToggle.isOn = true;
        }
        else
        {
            VolumeToggle.isOn = false;
        }
        SetVolume();
        MainSound.Play();
        MainSound.volume = Volume / 2;
        JetpackSound.Play();
        FootSound.Play();
        FootSound.volume = 0;
    }
    public void CoinPlay()
    {
        CoinSound.volume = Volume;
        CoinSound.Play();
    }
    public void laserPlay()
    {
        LaserSound.volume = Volume;
        LaserSound.Play();
    }
    public void SetJetpackSoundMax()
    {
        JetpackSound.volume = Volume;
    }
    public void SetJetpackSoundMin()
    {
        JetpackSound.volume = 0;
    }
    public void SetJetpackSoundMid()
    {
        JetpackSound.volume = Volume / 2;
    }
    public void SetFootSoundMax()
    {
        FootSound.volume = Volume;
    }
    public void SetFootSoundMin()
    {
        FootSound.volume = 0;
    }
    public void SetVolume()
    {
        if (VolumeToggle.isOn)
        {
            Volume = 0;
        }
        else
        {
            Volume = VolumeSlider.value;
        }
        MainSound.volume = Volume / 2;
    }
    public void RetainVolume()
    {
        PlayerPrefs.SetFloat("Volume", VolumeSlider.value);
        if (VolumeToggle.isOn)
        {
            PlayerPrefs.SetFloat("VolumeToggle", 1);
        }
        else
        {
            PlayerPrefs.SetFloat("VolumeToggle", 0);
        }
    }
    private void OnDisable()
    {
        RetainVolume();
    }
    public void PauseAllAudio()
    {
        AudioListener.volume = 0;
    }
    public void ResumeAllAudio()
    {
        AudioListener.volume = 1;
    }
}
