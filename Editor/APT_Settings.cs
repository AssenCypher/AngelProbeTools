#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace AngelProbeTools.Editor
{
    public static class APT_Settings
    {
        public const int HierarchySharedRoot = 0;
        public const int HierarchyPerRunRoot = 1;

        public const int ZoneCombined = 0;
        public const int ZonePerSelectedRoot = 1;
        public const int ZoneExperimentalAutoSplit = 2;

        public const int ResultCreateNew = 0;
        public const int ResultReplaceMatching = 1;
        public const int ResultUpdateMatching = 2;

        public const int FeatureReflection = 0;
        public const int FeatureLightProbeGroup = 1;
        public const int FeatureTriggerScaffold = 2;

        private const string PrefPrefix = "AngelProbeTools.";
        private const string PrefHierarchyMode = PrefPrefix + "HierarchyMode";
        private const string PrefZoneMode = PrefPrefix + "ZoneMode";
        private const string PrefResultMode = PrefPrefix + "ResultMode";
        private const string PrefUsePerFeatureResultHandling = PrefPrefix + "UsePerFeatureResultHandling";
        private const string PrefReflectionResultMode = PrefPrefix + "ReflectionResultMode";
        private const string PrefLpgResultMode = PrefPrefix + "LpgResultMode";
        private const string PrefTriggerResultMode = PrefPrefix + "TriggerResultMode";
        private const string PrefExpandPercent = PrefPrefix + "ExpandPercent";
        private const string PrefGridStep = PrefPrefix + "GridStep";
        private const string PrefCreateReflection = PrefPrefix + "CreateReflection";
        private const string PrefCreateLpg = PrefPrefix + "CreateLPG";
        private const string PrefCreateTrigger = PrefPrefix + "CreateTrigger";
        private const string PrefRootName = PrefPrefix + "RootName";
        private const string PrefFoldoutOverview = PrefPrefix + "FoldoutOverview";
        private const string PrefFoldoutOptional = PrefPrefix + "FoldoutOptional";
        private const string PrefFoldoutStatus = PrefPrefix + "FoldoutStatus";
        private const string PrefAutoZoneMinMeshSize = PrefPrefix + "AutoZoneMinMeshSize";
        private const string PrefAutoZoneSplitGap = PrefPrefix + "AutoZoneSplitGap";
        private const string PrefAutoZonePadding = PrefPrefix + "AutoZonePadding";
        private const string PrefAutoZoneMaxCount = PrefPrefix + "AutoZoneMaxCount";
        private const string PrefAutoZoneXZOnly = PrefPrefix + "AutoZoneXZOnly";

        public static int HierarchyMode
        {
            get => EditorPrefs.GetInt(PrefHierarchyMode, HierarchySharedRoot);
            set => EditorPrefs.SetInt(PrefHierarchyMode, Mathf.Clamp(value, HierarchySharedRoot, HierarchyPerRunRoot));
        }

        public static int ZoneMode
        {
            get => EditorPrefs.GetInt(PrefZoneMode, ZoneCombined);
            set => EditorPrefs.SetInt(PrefZoneMode, Mathf.Clamp(value, ZoneCombined, ZoneExperimentalAutoSplit));
        }

        public static int ResultMode
        {
            get => EditorPrefs.GetInt(PrefResultMode, ResultUpdateMatching);
            set => EditorPrefs.SetInt(PrefResultMode, Mathf.Clamp(value, ResultCreateNew, ResultUpdateMatching));
        }

        public static bool UsePerFeatureResultHandling
        {
            get => EditorPrefs.GetBool(PrefUsePerFeatureResultHandling, false);
            set => EditorPrefs.SetBool(PrefUsePerFeatureResultHandling, value);
        }

        public static int ReflectionResultMode
        {
            get => EditorPrefs.GetInt(PrefReflectionResultMode, ResultUpdateMatching);
            set => EditorPrefs.SetInt(PrefReflectionResultMode, Mathf.Clamp(value, ResultCreateNew, ResultUpdateMatching));
        }

        public static int LightProbeGroupResultMode
        {
            get => EditorPrefs.GetInt(PrefLpgResultMode, ResultUpdateMatching);
            set => EditorPrefs.SetInt(PrefLpgResultMode, Mathf.Clamp(value, ResultCreateNew, ResultUpdateMatching));
        }

        public static int TriggerResultMode
        {
            get => EditorPrefs.GetInt(PrefTriggerResultMode, ResultUpdateMatching);
            set => EditorPrefs.SetInt(PrefTriggerResultMode, Mathf.Clamp(value, ResultCreateNew, ResultUpdateMatching));
        }

        public static float ExpandPercent
        {
            get => Mathf.Clamp(EditorPrefs.GetFloat(PrefExpandPercent, 0.02f), 0f, 0.20f);
            set => EditorPrefs.SetFloat(PrefExpandPercent, Mathf.Clamp(value, 0f, 0.20f));
        }

        public static float GridStep
        {
            get => Mathf.Max(0.25f, EditorPrefs.GetFloat(PrefGridStep, 2.0f));
            set => EditorPrefs.SetFloat(PrefGridStep, Mathf.Max(0.25f, value));
        }

        public static bool CreateReflectionProbe
        {
            get => EditorPrefs.GetBool(PrefCreateReflection, true);
            set => EditorPrefs.SetBool(PrefCreateReflection, value);
        }

        public static bool CreateLpg
        {
            get => EditorPrefs.GetBool(PrefCreateLpg, true);
            set => EditorPrefs.SetBool(PrefCreateLpg, value);
        }

        public static bool CreateTrigger
        {
            get => EditorPrefs.GetBool(PrefCreateTrigger, false);
            set => EditorPrefs.SetBool(PrefCreateTrigger, value);
        }

        public static string RootName
        {
            get
            {
                string value = EditorPrefs.GetString(PrefRootName, APT_Info.DefaultSharedRootName);
                return SanitizeRootName(value);
            }
            set => EditorPrefs.SetString(PrefRootName, SanitizeRootName(value));
        }

        public static bool FoldoutOverview
        {
            get => EditorPrefs.GetBool(PrefFoldoutOverview, false);
            set => EditorPrefs.SetBool(PrefFoldoutOverview, value);
        }

        public static bool FoldoutOptional
        {
            get => EditorPrefs.GetBool(PrefFoldoutOptional, false);
            set => EditorPrefs.SetBool(PrefFoldoutOptional, value);
        }

        public static bool FoldoutStatus
        {
            get => EditorPrefs.GetBool(PrefFoldoutStatus, false);
            set => EditorPrefs.SetBool(PrefFoldoutStatus, value);
        }

        public static float AutoZoneMinMeshSize
        {
            get => Mathf.Max(0.5f, EditorPrefs.GetFloat(PrefAutoZoneMinMeshSize, 5.0f));
            set => EditorPrefs.SetFloat(PrefAutoZoneMinMeshSize, Mathf.Max(0.5f, value));
        }

        public static float AutoZoneSplitGap
        {
            get => Mathf.Max(0.5f, EditorPrefs.GetFloat(PrefAutoZoneSplitGap, 6.0f));
            set => EditorPrefs.SetFloat(PrefAutoZoneSplitGap, Mathf.Max(0.5f, value));
        }

        public static float AutoZonePadding
        {
            get => Mathf.Max(0f, EditorPrefs.GetFloat(PrefAutoZonePadding, 0.5f));
            set => EditorPrefs.SetFloat(PrefAutoZonePadding, Mathf.Max(0f, value));
        }

        public static int AutoZoneMaxCount
        {
            get => Mathf.Clamp(EditorPrefs.GetInt(PrefAutoZoneMaxCount, 12), 1, 64);
            set => EditorPrefs.SetInt(PrefAutoZoneMaxCount, Mathf.Clamp(value, 1, 64));
        }

        public static bool AutoZoneXZOnly
        {
            get => EditorPrefs.GetBool(PrefAutoZoneXZOnly, true);
            set => EditorPrefs.SetBool(PrefAutoZoneXZOnly, value);
        }

        public static bool HasAnyGeneratorEnabled => CreateReflectionProbe || CreateLpg || CreateTrigger;

        public static int GetEffectiveResultMode(int feature)
        {
            if (!UsePerFeatureResultHandling)
            {
                return ResultMode;
            }

            switch (feature)
            {
                case FeatureReflection:
                    return ReflectionResultMode;
                case FeatureLightProbeGroup:
                    return LightProbeGroupResultMode;
                case FeatureTriggerScaffold:
                    return TriggerResultMode;
                default:
                    return ResultMode;
            }
        }

        public static string BuildResultHandlingSummaryLoc()
        {
            if (!UsePerFeatureResultHandling)
            {
                return GetResultModeLocKey(ResultMode);
            }

            return "AP_PT_RESULT_MIXED";
        }

        public static string SanitizeRootName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return APT_Info.DefaultSharedRootName;
            }

            string trimmed = value.Trim();
            if (trimmed.Length > 64)
            {
                trimmed = trimmed.Substring(0, 64).Trim();
            }

            return string.IsNullOrWhiteSpace(trimmed) ? APT_Info.DefaultSharedRootName : trimmed;
        }

        public static string GetResultModeLocKey(int value)
        {
            switch (Mathf.Clamp(value, ResultCreateNew, ResultUpdateMatching))
            {
                case ResultReplaceMatching:
                    return "AP_PT_RESULT_REPLACE";
                case ResultUpdateMatching:
                    return "AP_PT_RESULT_UPDATE";
                default:
                    return "AP_PT_RESULT_CREATE";
            }
        }
    }
}
#endif
