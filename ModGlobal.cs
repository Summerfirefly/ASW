namespace AntiSubmarineWeapon
{
    internal static class ModGlobal
    {
        internal static bool drawSettingsWindow = false;
        internal static bool LoadedSceneIsOn => HighLogic.LoadedSceneIsFlight || HighLogic.LoadedSceneIsEditor;
    }
}
