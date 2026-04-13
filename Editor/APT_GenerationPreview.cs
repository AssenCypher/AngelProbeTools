#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace AngelProbeTools.Editor
{
    public static class APT_GenerationPreview
    {
        public sealed class Preview
        {
            public bool HasSelection;
            public bool HasBounds;
            public int SelectionCount;
            public string RootName;
            public Vector3 CombinedSize;
            public int ZoneCount;
            public int EstimatedProbeCountTotal;
            public int EstimatedProbeCountLargestZone;
            public bool HasAnyGeneratorEnabled;
            public bool IsHeavyLpg;
            public string ResultHandlingLocKey;
        }

        public static Preview Build(Transform[] selection)
        {
            Preview preview = new Preview
            {
                HasSelection = selection != null && selection.Length > 0,
                SelectionCount = selection != null ? selection.Length : 0,
                RootName = APT_Settings.RootName,
                HasAnyGeneratorEnabled = APT_Settings.HasAnyGeneratorEnabled,
                ResultHandlingLocKey = APT_Settings.BuildResultHandlingSummaryLoc()
            };

            if (!preview.HasSelection)
            {
                return preview;
            }

            Bounds? combined = APT_BoundsUtility.CollectBounds(selection, true, true);
            if (!combined.HasValue)
            {
                return preview;
            }

            preview.HasBounds = true;
            preview.CombinedSize = combined.Value.size;

            List<Bounds> zones = APT_ZoneBuilder.BuildGenerationZones(selection, combined.Value);
            if (zones == null || zones.Count == 0)
            {
                zones = new List<Bounds>
                {
                    APT_BoundsUtility.ExpandBounds(combined.Value, APT_Settings.ExpandPercent, 0f)
                };
            }

            preview.ZoneCount = zones.Count;

            if (APT_Settings.CreateLpg)
            {
                int total = 0;
                int largest = 0;
                for (int i = 0; i < zones.Count; i++)
                {
                    int count = EstimateProbeCount(zones[i], APT_Settings.GridStep);
                    total += count;
                    if (count > largest)
                    {
                        largest = count;
                    }
                }

                preview.EstimatedProbeCountTotal = total;
                preview.EstimatedProbeCountLargestZone = largest;
                preview.IsHeavyLpg = total >= 12000 || largest >= 6000;
            }

            return preview;
        }

        public static int EstimateProbeCount(Bounds bounds, float requestedStep)
        {
            float safeStep = Mathf.Max(0.25f, requestedStep);
            Vector3 size = bounds.size;
            int countX = ComputeAxisSampleCount(size.x, safeStep);
            int countY = ComputeAxisSampleCount(size.y, safeStep);
            int countZ = ComputeAxisSampleCount(size.z, safeStep);
            return Mathf.Max(1, countX * countY * countZ);
        }

        private static int ComputeAxisSampleCount(float size, float step)
        {
            if (size <= 0.001f)
            {
                return 1;
            }

            return Mathf.Max(2, Mathf.CeilToInt(size / Mathf.Max(0.25f, step)) + 1);
        }
    }
}
#endif
