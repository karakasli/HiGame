using UnityEngine;
using UnityEngine.UI;

namespace Codes.ServiceModules.GameService.Account.Components
{
  public class LoginButtonComp : MonoBehaviour
  {
    private Button _button;
    
    private void Start()
    {
      _button = transform.Find("Button").GetComponent<Button>();
      _button.onClick.AddListener(OnClickLogin);
    }

    private void CheckCurrentAccount()
    {
      _button.gameObject.SetActive(!GameServiceManager.Instance.provider.IsAuthenticated());
    }
    
    private void OnClickLogin()
    {      
      GameServiceManager.Instance.provider.AuthenticateUser( result =>
      {
        if (result)
        {
              Debug.Log("User is authenticated!");
        }
      });
    }

    private void Update()
    {
      CheckCurrentAccount();
    }
  }
}
