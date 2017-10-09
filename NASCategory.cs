using KSP.UI.Screens;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AntiSubmarineWeapon
{
    public class NASCategoryModule : PartModule
    {

    }

    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class NASCategory
    {
        private static readonly List<AvailablePart> availableParts = new List<AvailablePart>();

        void Awake()
        {
            GameEvents.onGUIEditorToolbarReady.Add(NASCategoryFunc);
        }

        void NASCategoryFunc()
        {
            const string customCategoryName = "NAS";
            const string customDisplayCategoryName = "NAS Parts";
            availableParts.Clear();
            availableParts.AddRange(PartLoader.LoadedPartsList.NASParts());
            Texture2D iconTex = GameDatabase.Instance.GetTexture("NAS/Plugins/NASIcon", false);
            RUI.Icons.Selectable.Icon icon = new RUI.Icons.Selectable.Icon("NAS", iconTex, iconTex, false);
            PartCategorizer.Category filter = PartCategorizer.Instance.filters.Find(f => f.button.categoryName == "Filter by function");
            PartCategorizer.AddCustomSubcategoryFilter(filter, customCategoryName, customDisplayCategoryName, icon, p => availableParts.Contains(p));
        }
    }

    public static class PartLoaderExtensions
    {
        public static IEnumerable<AvailablePart> NASParts(this List<AvailablePart> parts)
        {
            return (from avPart in parts.Where(p => p.partPrefab)
                    let NASModule = avPart.partPrefab.GetComponent<NASCategoryModule>()
                    where NASModule != null
                    select avPart
                     ).ToList();
        }
    }
}
