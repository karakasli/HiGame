
using System;
using Codes.ServiceModules.GameService.Constants;
using Codes.ServiceModules.GameService.Entities;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

namespace Codes.ServiceModules.GameService.Provider
{
    public class GooglePlayGameServiceProvider : IGameServiceProvider
    {
        private static string TAG = "HiGame-GooglePlayServiceProvider";

        private PlayGamesPlatform _platform;

        public CommonAuthUser commonAuthUser = null;

        public void Init()
        {
            InitPlayGamesPlatform();
        }

        public bool IsAuthenticated()
        {
            return commonAuthUser != null;
        }

        private void InitPlayGamesPlatform()
        {
            if (_platform == null)
            {
                PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
                PlayGamesPlatform.DebugLogEnabled = true;
                PlayGamesPlatform.InitializeInstance(config);

                PlayGamesPlatform.DebugLogEnabled = true;
                _platform = PlayGamesPlatform.Activate();
            }
        }

        public void SignOut()
        {
            commonAuthUser = null;
            _platform?.SignOut();
        }

        public void AuthenticateUser(Action<bool> callback = null)
        {
            if (!IsAuthenticated())
            {
                Social.localUser.Authenticate(result =>
                {
                    if (result)
                    {
                        Debug.Log(TAG + " Authenticate is successfull! UserName is " + Social.localUser.userName);
                        commonAuthUser = new CommonAuthUser
                        {
                            email = Social.localUser.userName,
                            name = Social.localUser.userName,
                            id = Social.localUser.id
                        };
                        PlayerPrefs.SetInt("autoLogin", 1);
                    }
                    else
                    {
                        Debug.Log(TAG + " Authenticate is failed!");
                    }
                    callback?.Invoke(result);
                });
            }
        }

        public void SendScore(int score, string boardId)
        {
            if (!IsAuthenticated())
            {
                Debug.Log(TAG + ": Sent score failed user is not authenticated!");
                return;
            }

            Social.ReportScore(score, boardId, success =>
            {
                Debug.Log(TAG + ": Sent score to leader board: " + boardId);
            });
        }

        public void ShowLeaderBoard(string boardId = "")
        {
            if (!IsAuthenticated())
            {
                AuthenticateUser(success =>
                {
                    if (success)
                    {
                        if (boardId.Equals(""))
                            Social.ShowLeaderboardUI();
                        else
                            Social.ShowLeaderboardUI();
                    }
                    else
                    {
                        Debug.Log(TAG + ": Show leader board connection failed.");
                    }
                });
            }
            else
            {
                Social.ShowLeaderboardUI();
            }
        }

        public void ShowAchievements()
        {
            if (!IsAuthenticated())
                AuthenticateUser(success =>
                {
                    if (success)
                        Social.ShowAchievementsUI();
                    else
                        Debug.Log(TAG + ": Connection Failed.");
                });
            else
                Social.ShowAchievementsUI();
        }

        public void UnlockAchievement(string key)
        {
            if (!IsAuthenticated())
            {
                Debug.Log(TAG + ": InitAchievement Error: user is not authenticated!");
                return;
            }

            Social.ReportProgress(key, 100.0f, success =>
            {
                Debug.Log(TAG + " " + key + ":achievements is unlocked successfully");
            });
        }

        public CommonAuthUser GetUserInfo()
        {
            return commonAuthUser;
        }
    }
}