using Codes.ServiceModules.AdsService;
using Codes.ServiceModules.GameService;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    void Start()
    {
        /**
         * To load GameServiceManager, we can use below code line.
         * It will create Singletan GameServiceManager and assinged provider accoirding to #defines
         * Init function will be triggered signIn.
         */
        GameServiceManager.Instance.Init();

        /**
        * To load GameServiceManager, we can use below code line.
        * It will create Singletan GameServiceManager and assinged provider accoirding to #defines
        * Init function will be triggered signIn.
        */
        AdsManager.Instance.Init();

        AdsManager.Instance.CreateAndLoadBannerAd(AdIDs.BannerInGame);
        AdsManager.Instance.CreateAndLoadInterstitialAd(AdIDs.InterCareer);
    }
}
