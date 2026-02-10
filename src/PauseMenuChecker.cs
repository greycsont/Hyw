using UnityEngine;

namespace HYW;

public static class PauseMenuChecker
{
    public static GameObject _zpPauseMenu;
    public static void CheckPauseMenu(OptionsMenuToManager __instance)
    {
        if (__instance.pauseMenu != null && __instance.pauseMenu.name == "PauseMenu")
        {
            if (_zpPauseMenu == null)
            {
                _zpPauseMenu = new GameObject("ZpPauseMenu");
                
                _zpPauseMenu.transform.SetParent(__instance.pauseMenu.transform.parent, false);
                
                _zpPauseMenu.AddComponent<ZpPauseMenu>();
            }
            __instance.pauseMenu = _zpPauseMenu;
            _zpPauseMenu.SetActive(false);
            
            Plugin.Logger.LogInfo("HYW");
        }
    }
}