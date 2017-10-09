using UnityEngine;
using KSP.Localization;
using BDArmory.Parts;

namespace AntiSubmarineWeapon
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class ModUiManager : MonoBehaviour
    {
        public static int setAllDepthCharges = 25;
        private static int _setAllDepthChargesTemp = 25;
        public static int setAllTorpedos = 1;
        private static int _setAllTorpedosTemp = 1;
        private static bool _isHide;

        public static Rect settingsWindowRect = new Rect(100, 100, 400, 80);

        public void Awake()
        {
            GameEvents.onShowUI.Add(ShowUi);
            GameEvents.onHideUI.Add(HideUi);
        }

        public void OnDestory()
        {
            GameEvents.onShowUI.Remove(ShowUi);
            GameEvents.onHideUI.Remove(HideUi);
        }

        public void ShowUi()
        {
            _isHide = false;
        }

        public void HideUi()
        {
            _isHide = true;
        }

        private void OnGUI()
        {
            if (ModGlobal.LoadedSceneIsOn && ModGlobal.drawSettingsWindow && !_isHide)
            {
                settingsWindowRect = GUI.Window(976233, settingsWindowRect, DrawDepthContorllerWindow, Localizer.Format("#autoLOC_NAS_DepthSettings_title"));
            }
        }

        private static void DrawDepthContorllerWindow(int windowId)
        {
            var depthChargeMaxValue = FindMinDepth("DepthCharge");
            var torpedoMaxValue = FindMinDepth("Torpedo");

            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label(Localizer.Format("#autoLOC_NAS_DepthSettings_depthCharge"));
            GUILayout.FlexibleSpace();
            
            if (depthChargeMaxValue > 0)
            {
                GUILayout.Label(setAllDepthCharges.ToString() + " m");
                setAllDepthCharges = (int)GUILayout.HorizontalScrollbar(
                    setAllDepthCharges, 2, 25,
                    depthChargeMaxValue + 2, GUILayout.MinWidth(100));
                
                if (setAllDepthCharges != _setAllDepthChargesTemp)
                {
                    SetAllDepthChargeDepth(setAllDepthCharges);
                    _setAllDepthChargesTemp = setAllDepthCharges;
                }
            }
            else
            {
                GUILayout.Label(Localizer.Format("#autoLOC_NAS_DepthSettings_unavailable"));
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label(Localizer.Format("#autoLOC_NAS_DepthSettings_torpedo"));
            GUILayout.FlexibleSpace();
            if (torpedoMaxValue > 0)
            {
                GUILayout.Label(setAllTorpedos.ToString() + " m");
                setAllTorpedos = (int)GUILayout.HorizontalScrollbar(setAllTorpedos, 2, 1, torpedoMaxValue, GUILayout.MinWidth(100));
                if (setAllTorpedos != _setAllTorpedosTemp)
                {
                    SetAllTorpedoDepth(setAllTorpedos);
                    _setAllTorpedosTemp = setAllTorpedos;
                }
            }
            else
            {
                GUILayout.Label(Localizer.Format("#autoLOC_NAS_DepthSettings_unavailable"));
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUI.DragWindow();
        }

        private static float FindMinDepth(string type)
        {
            var min = -1.0f;
            foreach (var current in FlightGlobals.ActiveVessel.Parts)
            {
                var target =  current.GetComponent<ModuleAntiSubmarineWeapon>();
                var missileLauncher = current.GetComponent<MissileLauncher>();

                if (target == null || target.type != type || missileLauncher.HasFired)
                    continue;

                min = target.maxDepth < min ? target.maxDepth : min;

                if (min < 0)
                {
                    min = target.maxDepth;
                }
            }

            return min;
        }

        private static void SetAllDepthChargeDepth(int value)
        {
            foreach (var current in FlightGlobals.ActiveVessel.Parts)
            {
                if (current == null)
                    continue;

                var missileLauncher = current.GetComponent<MissileLauncher>();
                if (missileLauncher == null || missileLauncher.HasFired)
                    continue;
                
                var target = current.GetComponent<ModuleAntiSubmarineWeapon>();

                if (target == null || target.type != "DepthCharge")
                    continue;

                target.autoDestroyDepth = value;
            }
        }

        private static void SetAllTorpedoDepth(int value)
        {
            foreach (var current in FlightGlobals.ActiveVessel.Parts)
            {
                if (current == null)
                    continue;

                var missileLauncher = current.GetComponent<MissileLauncher>();
                if (missileLauncher == null || missileLauncher.HasFired)
                    continue;
                
                var target = current.GetComponent<ModuleAntiSubmarineWeapon>();

                if (target == null || target.type != "Torpedo")
                    continue;

                target.cruiseDepth = value;
            }
        }
    }
}
