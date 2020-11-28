using Codes.ServiceModules.AdsService.Listeners;
using Codes.ServiceModules.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Codes.ServiceModules.AdsService
{
  public class AdsManager : Singleton<AdsManager>
  {
    protected Dictionary<string, string> adsDictionary = new Dictionary<string, string>();
    protected Dictionary<string, int> intervals = new Dictionary<string, int>();

    public IAdsProvider provider;

    public void Init()
    {
        #if HMS_BUILD

            provider = HuaweiAdsProvider.Instance;

            #if TEST_VERSION
                AddAdUnit(AdIDs.RewardClue, "testx9dtjwj8hp", 180);
                AddAdUnit(AdIDs.InterCareer, "testb4znbuh3n2", 180);
                AddAdUnit(AdIDs.BannerInGame, "testw6vs28auh3", 180);
            #else
                AddAdUnit(AdIDs.RewardClue, "YOUR_AD_ID", 180);
                AddAdUnit(AdIDs.InterCareer, "YOUR_AD_ID", 180);
                AddAdUnit(AdIDs.BannerInGame, "YOUR_AD_ID", 180);
            #endif
        #else

            provider = GoogleAdsProvider.Instance;

            #if TEST_VERSION
                AddAdUnit(AdIDs.RewardClue, "ca-app-pub-3940256099942544/5224354917", 180);
                AddAdUnit(AdIDs.InterCareer, "ca-app-pub-3940256099942544/1033173712", 180);
                AddAdUnit(AdIDs.BannerInGame, "ca-app-pub-3940256099942544/6300978111", 180);
            #else
                AddAdUnit(AdIDs.RewardClue, "YOUR_AD_ID", 60);
                AddAdUnit(AdIDs.InterCareer, "YOUR_AD_ID", 120 );
                AddAdUnit(AdIDs.BannerInGame, "YOUR_AD_ID", 180);
            #endif
        #endif
    }

    public void CreateAndLoadInterstitialAd(string key)
    {
      if (PlayerPrefs.GetInt("remove_ads", 0) > 0)
      {
        Debug.Log("Ads is removed by player, there will not be any interstitial ads.");
        return;
      }

      provider.CreateAndLoadInterstitialAd(adsDictionary[key]);
    }

    public void CreateAndLoadBannerAd(string key)
    {
        if (PlayerPrefs.GetInt("remove_ads", 0) > 0)
        {
            Debug.Log("Ads is removed by player, there will not be any interstitial ads.");
            return;
        }
            provider.CreateAndLoadBannerAd(adsDictionary[key]);
    }
    
    public void ShowInterstitialAd(string key)
    {
        if (CheckAdInterval(adsDictionary[key], intervals[key]))
        {
            provider.ShowInterstitialAd(adsDictionary[key], GetTime());
        }
    }

    public void CreateAndLoadRewardedAd(string key, IRewardedAdListener listener)
    {
            Debug.Log("PortModule AdsManager CreateAndLoadRewardedAd ");
            if (!CheckAdInterval(adsDictionary[key], intervals[key]))
            {
                Debug.Log("PortModule AdsManager Interval is not ended.");
                return;
            }

            provider.CreateAndLoadRewardedAd(adsDictionary[key], listener);
    }

    public void DestroyBannerAd(string key)
    {
            provider.DestroyBannerAd(adsDictionary[key]);
    }

    public void ShowRewardedAd(string key)
    {
            provider.ShowRewardedAd(adsDictionary[key], GetTime());
    }

    public void AddAdUnit(string key, string adId, int interval)
    {
        if (!adsDictionary.ContainsKey(key))
            adsDictionary.Add(key, adId);
        if (!intervals.ContainsKey(key))
            intervals.Add(key, interval);
    }

    protected static int GetTime()
    {
        var date = DateTime.UtcNow;
        return Convert.ToInt32(new DateTimeOffset(date).ToUnixTimeMilliseconds() / 1000);
    }

    protected bool CheckAdInterval(string adId, int interval)
    {
        var previousShowedTime = Convert.ToInt32(PlayerPrefs.GetString(adId, "0"));
        return GetTime() - previousShowedTime > interval;
    }
    }
}
