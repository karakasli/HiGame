using System.Collections.Generic;
using Codes.ServiceModules.AdsService.Listeners;
using Codes.ServiceModules.Utils;
using GoogleMobileAds.Api;
using UnityEngine;

public class GoogleAdsProvider: Singleton<GoogleAdsProvider>, IAdsProvider
{
    private readonly Dictionary<string, RewardedAd> _rewardedAds = new Dictionary<string, RewardedAd>();
    private readonly Dictionary<string, BannerView> _bannerAds = new Dictionary<string, BannerView>();
    private InterstitialAd _interstitialAd;

    public bool isInitialized;

    public void Init()
    {
        if (isInitialized)
        {
            return;
        }

        MobileAds.Initialize(initStatus =>
        {
            isInitialized = true;
        });
    }

    public void CreateAndLoadBannerAd(string adId)
    {
        if (_bannerAds.ContainsKey(adId))
        {
            return;
        }

        var bannerView = new BannerView(adId, AdSize.SmartBanner, AdPosition.Bottom);
        bannerView.LoadAd(CreateAdRequest());

        bannerView.OnAdFailedToLoad += (sender, args) =>
        {
            Debug.Log("PortModule AdsManager bannerView load is failed.");
        };

        _bannerAds.Add(adId, bannerView);
    }

    public void DestroyBannerAd(string adId)
    {
        if (_bannerAds.ContainsKey(adId))
        {
            _bannerAds[adId].Destroy();
            _bannerAds.Remove(adId);
        }
    }

    public void CreateAndLoadInterstitialAd(string adId)
    {
        _interstitialAd = new InterstitialAd(adId);
        _interstitialAd.LoadAd(CreateAdRequest());
    }

    public void CreateAndLoadRewardedAd(string adId, IRewardedAdListener listener)
    {
        var rewardedAd = new RewardedAd(adId);
        rewardedAd.OnAdLoaded += (sender, args) =>
        {
            listener.OnAdLoaded(adId);
        };
        rewardedAd.OnUserEarnedReward += (sender, reward) =>
        {
            listener.OnRewardEarned(adId);
        };
        rewardedAd.OnAdFailedToLoad += (sender, args) =>
        {
            Debug.Log("rewardedAd.OnAdFailedToLoad - " + args.Message);
            listener.OnAdLoadFailed(adId);
        };
        rewardedAd.OnAdClosed += (sender, args) =>
        {
            listener.OnAdClosed(adId);
        };

        rewardedAd.LoadAd(CreateAdRequest());

        if (_rewardedAds.ContainsKey(adId))
        {
            _rewardedAds.Remove(adId);
        }
        _rewardedAds.Add(adId, rewardedAd);
    }

    public void ShowInterstitialAd(string adId, int time)
    {
        if (_interstitialAd != null && _interstitialAd.IsLoaded())
        {
            _interstitialAd.Show();
            PlayerPrefs.SetString(adId, time.ToString());
        }
        else
        {
            CreateAndLoadInterstitialAd(adId);
        }
    }

    public void ShowRewardedAd(string adKey, int time)
    {
        var ad = GetRewardedAd(adKey);
        if (ad == null || !ad.IsLoaded()) return;

        ad.Show();
        PlayerPrefs.SetString(adKey, time.ToString());
    }

    private RewardedAd GetRewardedAd(string key)
    {
        return _rewardedAds.ContainsKey(key) ? _rewardedAds[key] : null;
    }

    private AdRequest CreateAdRequest()
    {
        #if TEST_VERSION
                return CreateAdRequestForTestDevice();
        #else
                    Debug.Log("PortModule AdsManager: CreateAdRequest");
                    return new AdRequest.Builder().Build();
        #endif
    }

    private static AdRequest CreateAdRequestForTestDevice()
    {
        Debug.Log("PortModule AdsManager: CreateAdRequest Test");

        return new AdRequest.Builder()
          .AddTestDevice("0C8C4338DBABBDF13C0C862BBBBFDDD4")
          .AddTestDevice("14704590229E2E5E999AF630A5FAF8EE")
          .AddTestDevice("C79CC4EF4F08CF3CE12678BA1BB542E6").Build();
    }
}
