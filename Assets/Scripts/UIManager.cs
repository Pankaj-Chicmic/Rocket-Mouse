using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public bool Initialized = false, loaded = false;
    public Animator startButton, settingsButton, dialog, contentpanel, gearImage;
    float Volume;
    InterstitialAdExample interstialAd;
    [SerializeField] Slider VolumeSlider;
    [SerializeField] Toggle VolumeToggle;
    [SerializeField] AudioSource MainAudio;
    private void Start()
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
        VolumeChange();
        interstialAd = gameObject.GetComponent<InterstitialAdExample>();
        StartCoroutine(CheckIntializationOfAd());
    }
    IEnumerator CheckIntializationOfAd()
    {
        while (!Initialized)
        {
            yield return new WaitForSecondsRealtime(0.01f);
        }
        interstialAd.LoadAd();
        StartCoroutine(CheckLoadingOfAd());
    }
    IEnumerator CheckLoadingOfAd()
    {
        while (!loaded)
        {
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
    public void startGame()
    {
        RetainVolume();
        if (interstialAd.loaded)
        {
            Debug.Log("Calling Function to show AD before Starting game.");
            interstialAd.ShowAd(); 
        }
        else
        {
            Debug.Log("Started Game without playing AD because it was not loaded.");
            PlayerPrefs.SetFloat("JetpackPower", 50);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    public void VolumeChange()
    {
        if (VolumeToggle.isOn)
        {
            Volume = 0;
        }
        else
        {
            Volume = VolumeSlider.value;
        }
        MainAudio.volume = Volume;
    }
    public void OpenSttings()
    {
        startButton.SetBool("isHidden", true);
        settingsButton.SetBool("isHidden", true);
        dialog.SetBool("isHidden", false);
    }
    public void CloseSttings()
    {
        startButton.SetBool("isHidden", false);
        settingsButton.SetBool("isHidden", false);
        dialog.SetBool("isHidden", true);
    }
    public void ToggleMenu()
    {
        bool isHidden = contentpanel.GetBool("isHidden");
        contentpanel.SetBool("isHidden", !isHidden);
        gearImage.SetBool("isHidden", !isHidden);
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
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            RetainVolume();
            Application.Quit();
        }
    }
    public void OnDisable()
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
