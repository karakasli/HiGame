using Codes.ServiceModules.AdsService.Listeners;

public interface IAdsProvider 
{
    void Init();
    void CreateAndLoadInterstitialAd(string adId);
    void CreateAndLoadBannerAd(string adId);
    void DestroyBannerAd(string adId);
    void ShowInterstitialAd(string adId, int time);
    void CreateAndLoadRewardedAd(string adId, IRewardedAdListener listener);
    void ShowRewardedAd(string adId, int time);
}
