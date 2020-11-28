using Codes.ServiceModules.GameService.Constants;
using Codes.ServiceModules.Utils;
using UnityEngine;

namespace Codes.ServiceModules.GameService
{
    public class GameServiceManager : Singleton<GameServiceManager>
    {
        public IGameServiceProvider provider;

        protected override void Awake()
        {
            base.Awake();
            provider = GameServiceFactory.CreateGameServiceProvider();
        }

        public void Init()
        {
            provider.Init();
            var autoLogin = PlayerPrefs.GetInt("autoLogin", 1);
            if(autoLogin == 1)
            {
                SignIn();
            }
        }

        public void SignIn()
        {
            provider.AuthenticateUser();
            PlayerPrefs.SetInt("autoLogin", 1);
        }

        public void SignOut()
        {
            provider.SignOut();
            PlayerPrefs.SetInt("autoLogin", 0);
        }

        public void CheckGamePlayTimeAchievements(int score)
        {
            if (score >= 25)
            {
              provider.UnlockAchievement(GameServiceConstants.achievement_reached_to_25_point);
            }

            if (score >= 50)
            {
              provider.UnlockAchievement(GameServiceConstants.achievement_reached_to_50_point);
            }
        }
    }
}
