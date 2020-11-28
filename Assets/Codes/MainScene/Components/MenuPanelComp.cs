using Codes.ServiceModules.IapService;
using Codes.ServiceModules.IapService.Entities;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanelComp : MonoBehaviour
{
    private LocalProduct localProduct;
    private Button removeAdsButton;

    public GameObject GamePanel;

    private bool isProductLoaded = false;

    void Start()
    {
        refreshScores();
        var buildTypeText = transform.Find("BuildType").GetComponent<Text>();
        #if HMS_BUILD
            buildTypeText.text = "HMS Build";
        #else
            buildTypeText.text = "GMS Build";
        #endif

        localProduct = new LocalProduct();
        var StartGameBtn = transform.Find("StartGameBtn").GetComponent<Button>();
        StartGameBtn.onClick.AddListener(OnClickedStartGameBtn);

        removeAdsButton = transform.Find("RemoveAdsButton").GetComponent<Button>();
        removeAdsButton.onClick.AddListener(OnClickedBuyButton);

        IapManager.Instance.Init(localProduct, (isInitialized) =>
        {
            if (isInitialized)
            {
                isProductLoaded = true;
                ShowProduct();
            }
            else
            {
                Debug.Log("Iap Manager cannot be initialized...");
            }
        });
    }

    private void Update()
    {
        ShowProduct();
    }

    public void OnClickedBuyButton()
    {
        IapManager.Instance.BuyProduct();
    }

    public void ShowProduct()
    {
        var isAdsRemoved = PlayerPrefs.GetInt("remove_ads", 0);
        if(isProductLoaded && isAdsRemoved != 1)
        {
            removeAdsButton.transform.Find("Price").GetComponent<Text>().text = localProduct.price;
            removeAdsButton.gameObject.SetActive(true);
        }
        else
        {
            removeAdsButton.gameObject.SetActive(false);
        }
    }

    private void OnClickedStartGameBtn()
    {
        transform.gameObject.SetActive(false);
        GamePanel.SetActive(true);

        var gamePanelComp = GamePanel.GetComponent<GamePanelComp>();
        gamePanelComp.StartGame();
    }

    public void refreshScores()
    {
        var latestsScore = PlayerPrefs.GetInt("latest_score", 0);
        var highestScore = PlayerPrefs.GetInt("highest_score", 0);

        transform.Find("LatestScore").GetComponent<Text>().text = latestsScore.ToString();
        transform.Find("HighestScore").GetComponent<Text>().text = highestScore.ToString();
    }
}
