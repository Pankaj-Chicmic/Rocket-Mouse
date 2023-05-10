using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameUi : MonoBehaviour
{
    [SerializeField] GameObject Mouse;
    [SerializeField] Animator RestartPanelAnimator;
    [SerializeField] Animator SettingPanelAnimator;
    [SerializeField] Animator PausedPanelAnimator;
    [SerializeField] Text scoreText;
    [SerializeField] Audio AudioManager;
    [SerializeField] TextMeshProUGUI JetpackPowertext;
    [SerializeField] Slider JetpackPowerSlider;
    [SerializeField] TextMeshProUGUI DistanceText;
    [SerializeField] RewardedAdsButton AD;
    [SerializeField] GameObject AfterAdText;

    PlayerController MousePlayerController;
    bool openedByRestartPanel;
    Animator MouseAnimator;
    void Start()
    {
        MousePlayerController = Mouse.GetComponent<PlayerController>();
        MouseAnimator = Mouse.GetComponent<Animator>();
        JetpackPowerSlider.interactable = false;
    }
    public void UpdateScore()
    {
        scoreText.text = MousePlayerController.score.ToString();
    }
    public void restart()
    {
        if (gameObject.GetComponent<ADDuringRestart>().loaded)
        {
            Debug.Log("Calling Function to show AD before Starting game.");
            gameObject.GetComponent<ADDuringRestart>().ShowAd();
        }
        else
        {
            Debug.Log("Started Game without playing AD because it was not loaded.");
            PlayerPrefs.SetFloat("JetpackPower", 50);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void Exit()
    {
        Time.timeScale = 1;
        AudioManager.RetainVolume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void RestartPanelSettings()
    {
        slideOutRestartPanel();
        slideInSettingPanel();
        openedByRestartPanel = true;
    }
    public void PausedPanelSettings()
    {
        slideOutPausedPanel();
        slideInSettingPanel();
        openedByRestartPanel = false;
    }
    public void SetVolume()
    {
        AudioManager.SetVolume();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !MouseAnimator.GetBool("isDead") && SettingPanelAnimator.GetBool("isHidden") && !AfterAdText.activeSelf)
        {
            slideInPausedPanel();
            Time.timeScale = 0;
        }
    }
    public void Resume()
    {
        slideOutPausedPanel();
        Time.timeScale = 1;
    }
    public void CloseSettings()
    {
        slideOutSettingPanel();
        if (openedByRestartPanel)
        {
            slideInRestartPanel();
        }
        else
        {
            slideInPausedPanel();
        }
    }
    public void slideInSettingPanel()
    {
        SettingPanelAnimator.SetBool("isHidden", false);
    }
    public void slideInPausedPanel()
    {
        PausedPanelAnimator.SetBool("isHidden", false);
        AD.hideAdButtons();
    }
    public void slideInRestartPanel()
    {
        RestartPanelAnimator.SetBool("isHidden", false);
        AD.hideAdButtons();
    }
    public void slideOutSettingPanel()
    {
        SettingPanelAnimator.SetBool("isHidden", true);
    }
    public void slideOutPausedPanel()
    {
        PausedPanelAnimator.SetBool("isHidden", true);
    }
    public void slideOutRestartPanel()
    {
        RestartPanelAnimator.SetBool("isHidden", true);
    }
    public void SetJetpackSliderAndText(float JetpackPower)
    {
        JetpackPowerSlider.value = JetpackPower / (float)100;
        JetpackPowertext.text = "Jetpack Power" + ((int)JetpackPower).ToString();
    }
    public void SetDistance(int Distance)
    {

        DistanceText.text = "Distance " + (Distance).ToString();

    }
}
