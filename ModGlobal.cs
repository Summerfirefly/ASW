using System;

namespace AntiSubmarineWeapon
{
    internal static class ModGlobal
    {
        internal static bool drawSettingsWindow = false;
        internal static bool loadedSceneIsOn
        {
            get
            {
                return HighLogic.LoadedSceneIsFlight || HighLogic.LoadedSceneIsEditor;
            }
        }
    }
}
