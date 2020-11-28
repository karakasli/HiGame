using Codes.ServiceModules.GameService.Entities;
using System;

public interface IGameServiceProvider
{
    void Init();
    bool IsAuthenticated();
    void SignOut();
    void AuthenticateUser(Action<bool> callback = null);
    void SendScore(int score, string boardId);
    void ShowLeaderBoard(string boardId = "");
    void ShowAchievements();
    void UnlockAchievement(string key);
    CommonAuthUser GetUserInfo();
}
