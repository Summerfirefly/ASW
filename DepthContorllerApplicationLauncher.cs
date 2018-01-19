using KSP.UI.Screens;
using UnityEngine;

namespace AntiSubmarineWeapon
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class DepthContorllerApplicationLauncher : MonoBehaviour
    {
        public static ApplicationLauncherButton btn;

        private static bool DrawGui
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
            GameEvents.onGUIApplicationLauncherReady.Add(OnLauncherReady);
        }

        public void OnDestroy()
        {
            if (btn != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(btn);
            }
            
            btn = null;
        }

        public void OnLauncherReady()
        {
            const ApplicationLauncher.AppScenes visibleIn = ApplicationLauncher.AppScenes.FLIGHT;

            if (!ApplicationLauncher.Ready)
                return;

            if (btn == null)
            {
                btn = ApplicationLauncher.Instance.AddModApplication(OnToggleApp, OnToggleApp, null, null, null, null,
                    visibleIn,
                    GameDatabase.Instance.GetTexture("NAS/Plugins/NASIcon", false));
            }

            if (DrawGui)
            {
                btn.SetTrue(false);
            }
            else
            {
                btn.SetFalse(false);
            }
        }

        public void OnToggleApp()
        {
            DrawGui = !DrawGui;
        }
    }
}
