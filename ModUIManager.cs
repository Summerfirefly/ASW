using System;
using UnityEngine;
using KSP.Localization;
using BahaTurret;
using AntiSubmarineWeapon;

namespace AntiSubmarineWeapon
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    class ModUIManager : MonoBehaviour
    {
        public static int setAllDepthCharges = 25;
        private static int setAllDepthChargesTemp = 25;
        public static int setAllTorpedos = 1;
        private static int setAllTorpedosTemp = 1;
        private static bool isHide = false;

        public static Rect settingsWindowRect = new Rect(100, 100, 400, 80);

        public void Awake()
        {
            GameEvents.onShowUI.Add(showUI);
            GameEvents.onHideUI.Add(hideUI);
        }

        public void OnDestory()
        {
            GameEvents.onShowUI.Remove(showUI);
            GameEvents.onHideUI.Remove(hideUI);
        }

        public void showUI()
        {
            isHide = false;
        }

        public void hideUI()
        {
            isHide = true;
        }

        void OnGUI()
        {
            if (ModGlobal.loadedSceneIsOn && ModGlobal.drawSettingsWindow && !isHide)
            {
                settingsWindowRect = GUI.Window(976233, settingsWindowRect, DrawDepthContorllerWindow, Localizer.Format("#autoLOC_NAS_DepthSettings_title"));
            }
            if (Camera.main)
            {
                if (!Camera.main.GetComponent<KerwisRedRound>())
                    Camera.main.gameObject.AddComponent<KerwisRedRound>();
            }
        }

        void DrawDepthContorllerWindow(int windowID)
        {
            float depthChargeMaxValue = findMinDepth("DepthCharge");
            float torpedoMaxValue = findMinDepth("Torpedo");

            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label(Localizer.Format("#autoLOC_NAS_DepthSettings_depthCharge"));
            GUILayout.FlexibleSpace();
            if (depthChargeMaxValue > 0)
            {
                GUILayout.Label(setAllDepthCharges.ToString() + " m");
                setAllDepthCharges = (int)GUILayout.HorizontalScrollbar(setAllDepthCharges, 2, 25, depthChargeMaxValue + 2, GUILayout.MinWidth(100));
                if (setAllDepthCharges != setAllDepthChargesTemp)
                {
                    setAllDepthChargeDepth(setAllDepthCharges);
                    setAllDepthChargesTemp = setAllDepthCharges;
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
                if (setAllTorpedos != setAllTorpedosTemp)
                {
                    setAllTorpedoDepth(setAllTorpedos);
                    setAllTorpedosTemp = setAllTorpedos;
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

        float findMinDepth(string type)
        {
            float min = -1.0f;
            foreach (var current in FlightGlobals.ActiveVessel.Parts)
            {
                ModuleAntiSubmarineWeapon target =  current.GetComponent<ModuleAntiSubmarineWeapon>();
                MissileLauncher missileLauncher = current.GetComponent<MissileLauncher>();

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

        void setAllDepthChargeDepth(int value)
        {
            MissileLauncher missileLauncher = null;
            foreach (var current in FlightGlobals.ActiveVessel.Parts)
            {
                if (current == null)
                    continue;

                missileLauncher = current.GetComponent<MissileLauncher>();
                if (missileLauncher == null || missileLauncher.HasFired)
                    continue;
                
                ModuleAntiSubmarineWeapon target = current.GetComponent<ModuleAntiSubmarineWeapon>();

                if (target == null || target.type != "DepthCharge")
                    continue;

                target.autoDestroyDepth = value;
            }
        }

        void setAllTorpedoDepth(int value)
        {
            MissileLauncher missileLauncher = null;
            foreach (var current in FlightGlobals.ActiveVessel.Parts)
            {
                if (current == null)
                    continue;

                missileLauncher = current.GetComponent<MissileLauncher>();
                if (missileLauncher == null || missileLauncher.HasFired)
                    continue;
                
                ModuleAntiSubmarineWeapon target = current.GetComponent<ModuleAntiSubmarineWeapon>();

                if (target == null || target.type != "Torpedo")
                    continue;

                target.cruiseDepth = value;
            }
        }
    }
}
