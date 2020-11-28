
using System;
using Codes.ServiceModules.GameService.Entities;
using HuaweiMobileServices.Base;
using HuaweiMobileServices.Game;
using HuaweiMobileServices.Id;
using UnityEngine;

namespace Codes.ServiceModules.GameService.Provider
{
    public class HMSGameServiceProvider : IGameServiceProvider
    {
        private static string TAG = "HMSGameServiceProvider";

        private HuaweiIdAuthService _authService;
        private IRankingsClient _rankingClient;
        private IAchievementsClient _achievementClient;

        public AuthHuaweiId HuaweiId;


        public CommonAuthUser commonAuthUser = null;

        public void Init()
        {
            InitHuaweiAuthService();
        }

        public bool IsAuthenticated()
        {
            return commonAuthUser != null;
        }

        private void InitHuaweiAuthService()
        {
            if (_authService == null)
            {
                var authParams = new HuaweiIdAuthParamsHelper(HuaweiIdAuthParams.DEFAULT_AUTH_REQUEST_PARAM_GAME).SetIdToken().CreateParams();
                _authService = HuaweiIdAuthManager.GetService(authParams);
                Debug.Log(TAG + " authservice is assigned.");
            }
        }

        public void SignOut()
        {
            commonAuthUser = null;
            _authService.SignOut();
        }

        public void AuthenticateUser(Action<bool> callback = null)
        {
            if (!IsAuthenticated())
            {
                _authService.StartSignIn(authId =>
                {
                    Debug.Log(TAG + ": Signed In Succesfully!");

                    commonAuthUser = new CommonAuthUser
                    {
                        email = authId.Email,
                        name = authId.DisplayName,
                        id = authId.OpenId,
                        photoUrl = authId.AvatarUriString
                    };

                    PlayerPrefs.SetInt("autoLogin", 1);

                    HuaweiId = authId;
                    Debug.Log(TAG + ": HuaweiIdToken is " + HuaweiId.IdToken);

                    //Load IJosAppClient for HMS
                    HuaweiMobileServicesUtil.SetApplication();
                    IJosAppsClient josAppsClient = JosApps.GetJosAppsClient(HuaweiId);
                    josAppsClient.Init();
                    

                    _rankingClient = Games.GetRankingsClient(HuaweiId);
                    _achievementClient = Games.GetAchievementsClient(HuaweiId);

                    callback?.Invoke(true);
                }, (error) =>
                {
                    commonAuthUser = null;
                    callback?.Invoke(false);
                });
            }
        }

        public void SendScore(int score, string boardId)
        {
            if (!IsAuthenticated())
            {
                Debug.Log(TAG + ": SendScore failed! User is not authenticated!");
                return;
            }

            ITask<ScoreSubmissionInfo> task = _rankingClient.SubmitScoreWithResult(boardId, score);
            task.AddOnSuccessListener((scoreInfo) =>
            {
                Debug.Log(TAG + ": SendScore is succeeed. Leader board is " + boardId);
            }).AddOnFailureListener((error) =>
            {
                Debug.Log(TAG + ": SendScore is failed to leader board: " + boardId + " " + error.WrappedExceptionMessage);
            });
        }

        public void ShowLeaderBoard(string boardId = "")
        {
            if (!IsAuthenticated())
            {
                Debug.Log(TAG + ": SendScore failed! User is not authenticated! Trying to login...");
                AuthenticateUser(success =>
                {
                    if (success)
                    {
                        _rankingClient.ShowTotalRankings(() =>
                        {
                            Debug.Log(TAG + ": User is authenticated! Rankinsg are showed!");

                        }, (exception) =>
                        {
                            Debug.Log(TAG + ": ShowLeaderboards ERROR - " + exception.WrappedExceptionMessage);
                        });
                    }
                    else
                    {
                        Debug.Log(TAG + ": ShowLeaderboards connection is failed!");
                    }
                });
            }
            else
            {
                _rankingClient.ShowTotalRankings(() =>
                {
                    Debug.Log(TAG + ": User is authenticated! Rankinsg are showed!");

                }, (exception) =>
                {
                    Debug.Log(TAG + ": ShowLeaderboards ERROR - " + exception.WrappedExceptionMessage);
                });
            }
        }

        public void ShowAchievements()
        {
            if (!IsAuthenticated())
            {
                Debug.Log(TAG + ": ShowAchievements failed! User is not authenticated! Trying to login...");
                AuthenticateUser(success =>
                {
                    if (success)
                    {
                        _achievementClient.ShowAchievementList(() =>
                        {
                            Debug.Log(TAG + ": User is authenticated! ShowAchievementList are showed!");
                        }, 
                        (exception) =>
                        {
                            Debug.Log(TAG + ": ShowAchievementList ERROR - " + exception.WrappedExceptionMessage);
                        });
                    }
                    else
                    {
                        Debug.Log(TAG + ": ShowAchievementList connection is failed!");
                    }
                });
            }
            else
            {
                _achievementClient.ShowAchievementList(() =>
                {
                    Debug.Log(TAG + ": User is authenticated! ShowAchievementList are showed!");
                },
                (exception) =>
                {
                    Debug.Log(TAG + ": ShowAchievementList ERROR - " + exception.WrappedExceptionMessage);
                });
            }
        }

        public void UnlockAchievement(string key)
        {
            if (!IsAuthenticated())
            {
                Debug.Log(TAG + ": UnlockAchievement is failed! User is not authenticated!");
                return;
            }

            ITask<HuaweiMobileServices.Utils.Void> task = _achievementClient.ReachWithResult(key);
            task.AddOnSuccessListener((result) =>
            {
                Debug.Log(TAG + ":achievements is unlocked successfully for " + key);

            }).AddOnFailureListener((exception) =>
            {
                Debug.Log(TAG + ": UnlockAchievements ERROR: " + exception.WrappedExceptionMessage);
            });
        }

        public CommonAuthUser GetUserInfo()
        {
            return commonAuthUser;
        }
    }
}