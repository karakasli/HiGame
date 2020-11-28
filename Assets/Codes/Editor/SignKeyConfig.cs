using UnityEditor;

[InitializeOnLoad]
public class StartUp
{
#if UNITY_EDITOR

  static StartUp()
  {
    PlayerSettings.Android.keyaliasName = "your_key_alias";
    PlayerSettings.Android.keyaliasPass = "your_key_pass";
    PlayerSettings.Android.keystorePass = "your_store_pass";
  }

#endif
}
