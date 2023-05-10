using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
public class InterstitialAdExample : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] string _iOsAdUnitId = "Interstitial_iOS";
    [SerializeField] UIManager audioManager;

    public bool loaded=false,loading=false;

    AdsInitializer adsInitializer;
    string _adUnitId;

    void Awake()
    {
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsAdUnitId
            : _androidAdUnitId;
    }
    private void Start()
    {
        adsInitializer = GameObject.FindObjectOfType<AdsInitializer>();
    }
    void Update()
    {
        if (!loading && adsInitializer.intialized)
        {
            Debug.Log("Started to Load Start Game Ad.");
            loading = true;
            LoadAd();
        }
    }
    public void LoadAd()
    {
        Advertisement.Load(_adUnitId, this);
    }
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Game start Ad loaded");
        loaded = true;
    }
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log("Game start Ad load failed, retrying");
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
        PlayerPrefs.SetFloat("JetpackPower", 50);
        Debug.Log("Start game Ad show failure");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {

        audioManager.ResumeAllAudio();
        if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Start game Ad Show completed with full Reward");
            PlayerPrefs.SetFloat("JetpackPower", 100);
        }
        else
        {
            Debug.Log("Start game Ad Show completed with half Reward");
            PlayerPrefs.SetFloat("JetpackPower", 50);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
}