using Codes.ServiceModules.AdsService;
using Codes.ServiceModules.GameService;
using Codes.ServiceModules.GameService.Constants;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePanelComp : MonoBehaviour
{
    public GameObject MenuPanel;

    Text ScoreText;
    Button StopGameBtn;
    
    float timer = 0f;
    bool isGameStop = false;

    public void StartGame()
    {
        timer = 0;
        isGameStop = false;

        ScoreText = transform.Find("Panel").Find("Score").GetComponent<Text>();
        StopGameBtn = transform.Find("StopGameBtn").GetComponent<Button>();
        StopGameBtn.onClick.RemoveAllListeners();
        StopGameBtn.onClick.AddListener(OnStopGameButtonClicked);
 
    }

    private void OnStopGameButtonClicked()
    {
        var score = Convert.ToInt32(timer);
        // Game is over. Check Achievements and Send Score to Leaderboard.
        isGameStop = true;
        GameServiceManager.Instance.CheckGamePlayTimeAchievements(score);
        GameServiceManager.Instance.provider.SendScore(score, GameServiceConstants.leaderboard_best_scores);

        // Save Score as Latest Score
        PlayerPrefs.SetInt("latest_score", score);

        // Save Highest Score - You can get this value also from leader board.
        var highestScore = PlayerPrefs.GetInt("highest_score", 0);
        if(score > highestScore)
        {
            PlayerPrefs.SetInt("highest_score", score);
        }

        // After Game is finished show interstitial advertisement
        AdsManager.Instance.ShowInterstitialAd(AdIDs.InterCareer);

        // Hide Game Panel and Show Menu Panel Again
        gameObject.SetActive(false);
        var MenuPanelComp = MenuPanel.GetComponent<MenuPanelComp>();
        MenuPanelComp.refreshScores();
        MenuPanelComp.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isGameStop)
        {
            timer += (Time.deltaTime * 2);
            ScoreText.text = timer.ToString("##");
        }
    }
}
