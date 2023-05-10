using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
public class ADDuringRestart : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] string _iOsAdUnitId = "Interstitial_iOS";
    [SerializeField] Audio audioManager;

    public bool loaded = false,loading=false;

    string _adUnitId;
    AdsInitializer adsInitializer;
    private void Start()
    {
        adsInitializer = GameObject.FindObjectOfType<AdsInitializer>();
    }
    void Awake()
    {
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsAdUnitId
            : _androidAdUnitId;
    }
    void Update()
    {
        if(!loading && adsInitializer.intialized)
        {
            loading = true;
            Debug.Log("Started to Load Start Game Ad.");
            LoadAd();
        }
    }
    public void LoadAd()
    {
        Advertisement.Load(_adUnitId, this);
    }
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Start game Ad loaded");
        loaded = true;
    }
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log("Failed to Load Start Game Ad,retrying");
        LoadAd();
    }
    public void ShowAd()
    {
        Advertisement.Show(_adUnitId, this);
        audioManager.PauseAllAudio();
    }
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        audioManager.ResumeAllAudio();
        Debug.Log($"Failed to show Start Game Ad + {error} + {message}");
        ReloadScene(false);
    }

    public void OnUnityAdsShowStart(string adUnitId) { Debug.Log("ONUNITYADSSHOWSTARTED"); }
    public void OnUnityAdsShowClick(string adUnitId) { Debug.Log("ONUNITYADSSHOWCLICK"); }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {

        audioManager.ResumeAllAudio();
        if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Completed to show Start Game Ad with full reward.");
            ReloadScene(true);
        }
        else
        {
            Debug.Log("Completed to show Start Game Ad with half reward.");
            ReloadScene(false);
        }
    }
    public void ReloadScene(bool comp)
    {
        if (comp)
        {
            PlayerPrefs.SetFloat("JetpackPower", 100);
        }
        else
        {
            PlayerPrefs.SetFloat("JetpackPower", 50);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
