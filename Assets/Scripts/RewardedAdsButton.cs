using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using TMPro;
using System.Collections;

public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] Button _showAdButton, CloseAdButton;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] PlayerController Player;
    [SerializeField] GameObject AdNotPlayedProperly;
    [SerializeField] GameUi UIManager;
    [SerializeField] TextMeshProUGUI AfterAdTimingText;
    [SerializeField] Audio audioManager;

    string _adUnitId = null;
    bool loaded = false,loading=false;
    int AfterAdTime = 0;
    AdsInitializer adsInitializer;
    public bool closedByPlayer = false;

    void Awake()
    {
        _adUnitId = _androidAdUnitId;
    }
    void Update()
    {
        if(!loading && adsInitializer.intialized)
        {
            Debug.Log("Started to Load Rewarded Game Ad.");
            loading = true;
            LoadAd();
        }
    }
    void Start()
    {
        hideAdButtons();
        AdNotPlayedProperly.SetActive(false);
        AfterAdTimingText.gameObject.SetActive(false);
        adsInitializer = GameObject.FindAnyObjectByType<AdsInitializer>();
    }
    public void LoadAd()
    {
        Advertisement.Load(_adUnitId, this);
    }
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Rewarded Ad loaded.");
        loaded = true;
    }
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log("Rewarded Ad loading failed, Retrying.");
        LoadAd();
    }
    public void ShowAd()
    {
        Advertisement.Show(_adUnitId, this);
        audioManager.PauseAllAudio();
    }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            AfterAdPlayed(true);
            Debug.Log("Rewarded Ad Show Complete with full reward.");
        }
        else
        {
            Debug.Log("Rewarded Ad Show Complete with half reward.");
            AfterAdPlayed(false);
        }
        loaded = false;
        audioManager.ResumeAllAudio();
        StartCoroutine(AfterAdTiming());
        loaded = false;
        LoadAd();
    }
   

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        AdNotPlayedProperly.SetActive(true);
        audioManager.ResumeAllAudio();
        StartCoroutine(AfterAdFailure());
        StartCoroutine(AfterAdTiming());
        Debug.Log("Rewarded Ad Show Failure.");
        loaded = false;
        LoadAd();
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    public void PlayAd()
    {
        if (loaded)
        {
            Time.timeScale = 0;
            hideAdButtons();
            ShowAd();
            Debug.Log("Started Rewarded Ad show");
        }
        else
        {
            hideAdButtons();
            closedByPlayer = true;
            AdNotPlayedProperly.SetActive(true);
            StartCoroutine(AfterAdFailure());
            Debug.Log("Cannot Start Rewarded Ad show because it is not loaded");
        }
    }

    public void PlayAdPaused()
    {
        UIManager.slideOutPausedPanel();
        if (loaded)
        {
            ShowAd();
            Debug.Log("Started Rewarded Ad show");
        }
        else
        {
            AdNotPlayedProperly.SetActive(true);
            Time.timeScale = 1;
            StartCoroutine(AfterAdFailure());
            Debug.Log("Cannot Start Rewarded Ad show because it is not loaded");
        }
    }
    public void closeAdButton()
    {
        closedByPlayer = true;
        hideAdButtons();
    }
    public void showAdButtons()
    {
        if (!Player.MouseAnimator.GetBool("isDead"))
        {
            _showAdButton.gameObject.SetActive(true);
            CloseAdButton.gameObject.SetActive(true);
        }
    }
    public void hideAdButtons()
    {
        _showAdButton.gameObject.SetActive(false);
        CloseAdButton.gameObject.SetActive(false);
    }
    void AfterAdPlayed(bool comp)
    {
        if (comp) Player.JetpackPower = 100;
    }
    IEnumerator AfterAdFailure()
    {
        yield return new WaitForSecondsRealtime(3);
        AdNotPlayedProperly.SetActive(false);
        Time.timeScale = 1;
    }
    IEnumerator AfterAdTiming()
    {
        AfterAdTimingText.gameObject.SetActive(true);
        while (AfterAdTime <= 2)
        {
            AfterAdTimingText.text = (3 - AfterAdTime).ToString();
            AfterAdTime++;
            yield return new WaitForSecondsRealtime(1);
        }
        if (AfterAdTime == 3)
        {
            AfterAdTime = 0;
            AfterAdTimingText.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }
}