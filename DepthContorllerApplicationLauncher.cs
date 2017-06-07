using System;
using KSP.UI.Screens;
using UnityEngine;

namespace AntiSubmarineWeapon
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class DepthContorllerApplicationLauncher : MonoBehaviour
    {
        public static ApplicationLauncherButton btn = null;

        private static bool drawGUI
        {
            get
            {
                return ModGlobal.drawSettingsWindow;
            }
            set
            {
                ModGlobal.drawSettingsWindow = value;
            }
        }

        public void Awake()
        {
            GameEvents.onGUIApplicationLauncherReady.Add(onLauncherReady);
        }

        public void OnDestroy()
        {
            if (btn != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(btn);
                btn = null;
            }
        }

        public void onLauncherReady()
        {
            ApplicationLauncher.AppScenes visibleIn = ApplicationLauncher.AppScenes.FLIGHT;

            if (!ApplicationLauncher.Ready)
                return;

            if (btn == null)
            {
                btn = ApplicationLauncher.Instance.AddModApplication(onToggleApp, onToggleApp, null, null, null, null, visibleIn,
                    (Texture)GameDatabase.Instance.GetTexture("NAS/Plugins/NASIcon", false));
            }

            if (drawGUI)
            {
                btn.SetTrue(false);
            }
            else
            {
                btn.SetFalse(false);
            }
        }

        public void onToggleApp()
        {
            drawGUI = !drawGUI;
        }
    }
}
