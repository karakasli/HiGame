namespace Codes.ServiceModules.AdsService.Listeners
{
    public interface IRewardedAdListener
    {
        void OnAdLoaded(string adId);
        void OnRewardEarned(string adId);
        void OnAdLoadFailed(string adId);
        void OnAdClosed(string adId);
    }
}