using UnityEngine;
using UnityEngine.UI;

namespace Codes.ServiceModules.GameService.Account.Components
{
  public class ProfileComp : MonoBehaviour
  {
    public GameObject _profileContainer;
    public GameObject _loginContainer;
    public Text _username;
    public Button _logoutButton;
    public Button _loginButton;

    public static string TAG = "ProfileComp";

    // Start is called before the first frame update
    void Start()
    {
      Debug.Log("Profile Comp is hidden till user loged in!");
      _profileContainer.SetActive(false);
      _loginContainer.SetActive(false);

      _logoutButton.onClick.AddListener(OnLogoutButtonClicked);
      _loginButton.onClick.AddListener(OnLoginButtonClicked);
    }
   
    private void OnLogoutButtonClicked() 
    {
        GameServiceManager.Instance.SignOut();
    }

    private void OnLoginButtonClicked()
    {
        GameServiceManager.Instance.SignIn();
    }
    void Update()
    {
      CheckAccount();
    }

    void CheckAccount()
    {
      if (GameServiceManager.Instance.provider.IsAuthenticated())
      {
        _profileContainer.SetActive(true);
        _loginContainer.SetActive(false);
        _username.text = GameServiceManager.Instance.provider.GetUserInfo().name;
      }
      else
      {
        _profileContainer.SetActive(false);
        _loginContainer.SetActive(true);
      }
    }
  }
}
