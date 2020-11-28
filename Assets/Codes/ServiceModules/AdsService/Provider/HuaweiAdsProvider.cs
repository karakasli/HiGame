using System.Collections.Generic;
using Codes.ServiceModules.AdsService.Listeners;
using Codes.ServiceModules.Utils;
using HuaweiConstants;
using HuaweiMobileServices.Ads;
using UnityEngine;
using static HuaweiConstants.UnityBannerAdPositionCode;

public class HuaweiAdsProvider: Singleton<HuaweiAdsProvider>, IAdsProvider
{
    private readonly Dictionary<string, RewardAd> _rewardedAds = new Dictionary<string, RewardAd>();
    private readonly Dictionary<string, BannerAd> _bannerAds = new Dictionary<string, BannerAd>();
    private InterstitialAd _interstitialAd;

    public bool isInitialized;

    public IRewardedAdListener rewardedlistener;

    public void Init()
    {
        if (isInitialized)
        {
            return;
        }

        HwAds.Init();
        isInitialized = true;
    }

    public void CreateAndLoadBannerAd(string adId)
    {
        if (_bannerAds.ContainsKey(adId))
        {
            return;
        }

        var bannerAdListener = new AdStatusListener();
        bannerAdListener.mOnAdFailed += (sender, args) =>
        {
            Debug.Log("PortModule AdsManager bannerView load is failed.");
        };
        bannerAdListener.mOnAdLoaded += (sender, args) =>
        {
            Debug.Log("PortModule AdsManager bannerView load is completed.");
            _bannerAds[adId].ShowBanner();
        };

        var bannerAdView = new BannerAd(bannerAdListener);
        bannerAdView.AdId = adId;
        bannerAdView.PositionType = (int)UnityBannerAdPositionCodeType.POSITION_BOTTOM;
        bannerAdView.SizeType = UnityBannerAdSize.BANNER_SIZE_SMART;

        bannerAdView.LoadBanner(CreateAdRequest());

        _bannerAds.Add(adId, bannerAdView);
    }

    public void DestroyBannerAd(string adId)
    {
        if (_bannerAds.ContainsKey(adId))
        {
            _bannerAds[adId].HideBanner();
            _bannerAds.Remove(adId);
        }
    }

    public void CreateAndLoadInterstitialAd(string adId)
    {
        _interstitialAd = new InterstitialAd
        {
            AdId = adId,
            AdListener = new InterstitialAdListener(this)
        };
        _interstitialAd.LoadAd(CreateAdRequest());
    }

    public void CreateAndLoadRewardedAd(string adId, IRewardedAdListener listener)
    {
        rewardedlistener = listener;
        var rewardedAd = new RewardAd(adId);
        rewardedAd.LoadAd(
                CreateAdRequest(),
                () => listener.OnAdLoaded(adId),
                (errorCode) => Debug.Log($"[HMS] Rewarded ad loading failed with error ${errorCode}")
            );

        if (_rewardedAds.ContainsKey(adId))
        {
            _rewardedAds.Remove(adId);
        }
        _rewardedAds.Add(adId, rewardedAd);
    }

    public void ShowInterstitialAd(string adId, int time)
    {
        if (_interstitialAd != null && _interstitialAd.Loaded)
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
        if (ad == null || !ad.Loaded) return;

        ad.Show(new RewardAdListener(this));
        PlayerPrefs.SetString(adKey, time.ToString());
    }

    private RewardAd GetRewardedAd(string key)
    {
        return _rewardedAds.ContainsKey(key) ? _rewardedAds[key] : null;
    }

    private AdParam CreateAdRequest()
    {
        Debug.Log("PortModule AdsManager: CreateAdRequest");
        return new AdParam.Builder().Build();
    }

    private static AdParam CreateAdRequestForTestDevice()
    {
        Debug.Log("PortModule AdsManager: CreateAdRequest Test");

        Debug.Log("PortModule AdsManager: CreateAdRequest");
        return new AdParam.Builder().Build();
    }
}

internal class RewardAdListener : IRewardAdStatusListener
{
    private HuaweiAdsProvider huaweiAdsProvider;

    public RewardAdListener(HuaweiAdsProvider huaweiAdsProvider)
    {
        this.huaweiAdsProvider = huaweiAdsProvider;
    }

    public void OnRewardAdClosed()
    {
        Debug.Log("[HMS] AdsManager RewardAdListener OnRewardAdClosed");
        huaweiAdsProvider.rewardedlistener.OnAdClosed("");
    }

    public void OnRewardAdFailedToShow(int errorCode)
    {
        Debug.Log("[HMS] AdsManager RewardAdListener OnRewardAdFailedToShow");
        huaweiAdsProvider.rewardedlistener.OnAdLoadFailed("");
    }

    public void OnRewardAdOpened()
    {
        Debug.Log("[HMS] AdsManager RewardAdListener OnRewardAdOpened");
    }

    public void OnRewarded(Reward reward)
    {
        Debug.Log("[HMS] AdsManager RewardAdListener OnRewarded");
        huaweiAdsProvider.rewardedlistener.OnRewardEarned("");
    }
}

internal class InterstitialAdListener : IAdListener
{
    private HuaweiAdsProvider huaweiAdsProvider;

    public InterstitialAdListener(HuaweiAdsProvider huaweiAdsProvider)
    {
        this.huaweiAdsProvider = huaweiAdsProvider;
    }

    public void OnAdClicked()
    {
        Debug.Log("[HMS] AdsManager OnAdClicked");
    }

    public void OnAdClosed()
    {
        Debug.Log("[HMS] AdsManager OnAdClosed");
    }

    public void OnAdFailed(int reason)
    {
        Debug.Log("[HMS] AdsManager OnAdFailed");
    }

    public void OnAdImpression()
    {
        Debug.Log("[HMS] AdsManager OnAdImpression");
    }

    public void OnAdLeave()
    {
        Debug.Log("[HMS] AdsManager OnAdLeave");
    }

    public void OnAdLoaded()
    {
        Debug.Log("[HMS] AdsManager OnAdLoaded");
    }

    public void OnAdOpened()
    {
        Debug.Log("[HMS] AdsManager OnAdOpened");
    }
}