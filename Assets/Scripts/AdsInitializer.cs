using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using TMPro;
public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    private string _gameId;
    [SerializeField] TextMeshProUGUI loadingText;
    public bool started = false, intialized = false;
    void Awake()
    {
        InvokeRepeating("Loading", 0f, 1f);
        DontDestroyOnLoad(gameObject);
        Invoke("NextScene", 3f);
    }
    void Update()
    {
        if (!started && !(Application.internetReachability == NetworkReachability.NotReachable))
        {
            Debug.Log(Application.internetReachability);
            Debug.Log("Unity Ads initialization started.");
            InitializeAds();
            started = true;
        }
    }
    public void InitializeAds()
    {
        Debug.Log("Unity Ads initialization started.");
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSGameId
            : _androidGameId;
        Advertisement.Initialize(_gameId, _testMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        gameObject.GetComponent<BannerAdExample>().LoadBanner();
        intialized= true;
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
    }
    public void Loading()
    {
        string temp = loadingText.text;
        temp += ".";
        loadingText.text = temp;
    }
    void NextScene()
    {
        CancelInvoke("Loading");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}