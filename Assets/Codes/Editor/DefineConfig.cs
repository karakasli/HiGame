using System.Collections.Generic;
using System.Linq;
using UnityEditor;

[InitializeOnLoad]
public class DefineConfig : Editor
{
  public static readonly string[] Symbols = new string[] {
        "TEST_VERSION",
        "HMS_BUILD",
        //"GMS_BUILD",
     };

  static DefineConfig()
  {
    List<string> allDefines = new List<string>();
    allDefines.AddRange(Symbols.Except(allDefines));
    PlayerSettings.SetScriptingDefineSymbolsForGroup(
        EditorUserBuildSettings.selectedBuildTargetGroup,
        string.Join(";", allDefines.ToArray()));
  }
}
