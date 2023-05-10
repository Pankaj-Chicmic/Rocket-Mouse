using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;

public class BannerAdExample : MonoBehaviour
{
    [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;
    [SerializeField] string _androidAdUnitId = "Banner_Android";
    string _adUnitId = null;
    bool shown = false,started=false;
    void Start()
    {
        _adUnitId = _androidAdUnitId;
        Advertisement.Banner.SetPosition(_bannerPosition);
    }

    public void LoadBanner()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };
        Advertisement.Banner.Load(_adUnitId, options);
    }
    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
        if (!started)
        {
            started = true;
            InvokeRepeating("BannerAd", 0,5f);
        }
    }
    void OnBannerError(string message)
    {
        Debug.Log("Failed to Load BannerAd, Retrying");
        LoadBanner();
    }
    void ShowBannerAd()
    {
        Debug.Log("Banner Shown");
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };
        Advertisement.Banner.Show(_adUnitId, options);
    }

    void HideBannerAd()
    {
        Debug.Log("Banner Hidden");
        Advertisement.Banner.Hide();
    }

    void OnBannerClicked() { }
    void OnBannerShown() { }
    void OnBannerHidden() { }
    void BannerAd()
    {
        if (shown)
        {
            HideBannerAd();
            LoadBanner();
        }
        else
        {
            ShowBannerAd();
        }
        shown = !shown;
    }
}