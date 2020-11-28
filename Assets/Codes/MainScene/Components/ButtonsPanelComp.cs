using Codes.ServiceModules.GameService;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsPanelComp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var ScoreBoardButton = transform.Find("ScoreBoardButton").GetComponent<Button>();
        var AchievementButton = transform.Find("AchievementButton").GetComponent<Button>();

        ScoreBoardButton.onClick.AddListener(OnClickedScoreBoardButton);
        AchievementButton.onClick.AddListener(OnClickedAchievementButton);
    }

    private void OnClickedScoreBoardButton()
    {
        GameServiceManager.Instance.provider.ShowLeaderBoard();
    }

    private void OnClickedAchievementButton()
    {
        GameServiceManager.Instance.provider.ShowAchievements();
    }
}
