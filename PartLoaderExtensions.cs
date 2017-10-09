using System.Collections.Generic;
using System.Linq;

namespace AntiSubmarineWeapon
{
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
