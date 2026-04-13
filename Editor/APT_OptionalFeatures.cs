#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;

namespace AngelProbeTools.Editor
{
    public static class APT_OptionalFeatures
    {
        public sealed class FeatureCard
        {
            public string Id;
            public string TitleLocKey;
            public string SummaryLocKey;
            public Func<bool> IsInstalled;
            public string InstalledStateLocKey;
            public string MissingStateLocKey;
            public MessageType StateTypeWhenInstalled = MessageType.Info;
            public MessageType StateTypeWhenMissing = MessageType.Warning;
        }

        private static readonly FeatureCard[] Cards =
        {
            new FeatureCard
            {
                Id = "lightingtools.dlp",
                TitleLocKey = "AP_PT_OPT_DLP_NAME",
                SummaryLocKey = "AP_PT_OPT_DLP_SUMMARY",
                IsInstalled = () => APT_PackageDetector.HasLightingToolsDlp,
                InstalledStateLocKey = "AP_PT_OPT_STATE_INSTALLED",
                MissingStateLocKey = "AP_PT_OPT_STATE_RESERVED"
            },
            new FeatureCard
            {
                Id = "lightingtools.mlp",
                TitleLocKey = "AP_PT_OPT_MLP_NAME",
                SummaryLocKey = "AP_PT_OPT_MLP_SUMMARY",
                IsInstalled = () => APT_PackageDetector.HasMagicLightProbes,
                InstalledStateLocKey = "AP_PT_OPT_STATE_PRESENT_ONLY",
                MissingStateLocKey = "AP_PT_OPT_STATE_NOT_PRESENT",
                StateTypeWhenInstalled = MessageType.None,
                StateTypeWhenMissing = MessageType.None
            },
            new FeatureCard
            {
                Id = "lightingtools.vrclv",
                TitleLocKey = "AP_PT_OPT_VRCLV_NAME",
                SummaryLocKey = "AP_PT_OPT_VRCLV_SUMMARY",
                IsInstalled = () => APT_PackageDetector.HasVRCLightVolumes,
                InstalledStateLocKey = "AP_PT_OPT_STATE_PRESENT_ONLY",
                MissingStateLocKey = "AP_PT_OPT_STATE_NOT_PRESENT",
                StateTypeWhenInstalled = MessageType.None,
                StateTypeWhenMissing = MessageType.None
            }
        };

        public static IReadOnlyList<FeatureCard> GetCards()
        {
            return Cards;
        }

        public static List<FeatureCard> BuildCards()
        {
            return new List<FeatureCard>(Cards);
        }

        public static int GetInstalledCount()
        {
            int count = 0;
            for (int i = 0; i < Cards.Length; i++)
            {
                if (Cards[i].IsInstalled != null && Cards[i].IsInstalled())
                {
                    count++;
                }
            }

            return count;
        }
    }
}
#endif
