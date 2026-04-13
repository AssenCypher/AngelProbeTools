#if UNITY_EDITOR
using UnityEngine;

namespace AngelProbeTools.Editor
{
    public static class APT_NameUtility
    {
        public static string BuildSelectionScope(Transform[] selection, string rootName)
        {
            if (selection == null || selection.Length == 0)
            {
                return BuildRootScope(rootName);
            }

            if (selection.Length == 1 && selection[0] != null)
            {
                return APT_BoundsUtility.SanitizeName(selection[0].name);
            }

            string first = selection[0] != null ? APT_BoundsUtility.SanitizeName(selection[0].name) : BuildRootScope(rootName);
            return first + " +" + Mathf.Max(1, selection.Length - 1);
        }

        public static string BuildZoneSuffix(int zoneIndex, int zoneCount)
        {
            if (zoneCount <= 1)
            {
                return string.Empty;
            }

            return "Zone " + (zoneIndex + 1).ToString("00");
        }

        public static string BuildGeneratedName(string featureName, string selectionScope, int zoneIndex, int zoneCount)
        {
            string safeFeature = string.IsNullOrWhiteSpace(featureName) ? "Generated" : featureName.Trim();
            string safeScope = string.IsNullOrWhiteSpace(selectionScope) ? "Selection" : selectionScope.Trim();
            string zoneSuffix = BuildZoneSuffix(zoneIndex, zoneCount);

            if (string.IsNullOrEmpty(zoneSuffix))
            {
                return safeFeature + " - " + safeScope;
            }

            return safeFeature + " - " + safeScope + " - " + zoneSuffix;
        }

        public static string BuildRootScope(string rootName)
        {
            string safe = APT_Settings.SanitizeRootName(rootName);
            return string.IsNullOrWhiteSpace(safe) ? APT_Info.DefaultSharedRootName : safe;
        }
    }
}
#endif
