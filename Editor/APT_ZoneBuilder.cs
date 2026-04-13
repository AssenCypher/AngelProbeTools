#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AngelProbeTools.Editor
{
    public static class APT_ZoneBuilder
    {
        public static List<Bounds> BuildGenerationZones(Transform[] selectedRoots, Bounds combinedBounds)
        {
            switch (APT_Settings.ZoneMode)
            {
                case APT_Settings.ZonePerSelectedRoot:
                    return BuildPerRootZones(selectedRoots);

                case APT_Settings.ZoneExperimentalAutoSplit:
                    List<Bounds> autoZones = BuildExperimentalAutoZones(selectedRoots);
                    if (autoZones.Count > 0)
                    {
                        return autoZones;
                    }

                    return new List<Bounds>
                    {
                        APT_BoundsUtility.ExpandBounds(combinedBounds, APT_Settings.ExpandPercent, 0f)
                    };

                default:
                    return new List<Bounds>
                    {
                        APT_BoundsUtility.ExpandBounds(combinedBounds, APT_Settings.ExpandPercent, 0f)
                    };
            }
        }

        private static List<Bounds> BuildPerRootZones(Transform[] selectedRoots)
        {
            List<Bounds> zones = new List<Bounds>();
            if (selectedRoots == null)
            {
                return zones;
            }

            for (int i = 0; i < selectedRoots.Length; i++)
            {
                Transform tr = selectedRoots[i];
                if (tr == null)
                {
                    continue;
                }

                Bounds? bounds = APT_BoundsUtility.CollectBounds(new[] { tr }, true, true);
                if (bounds.HasValue)
                {
                    zones.Add(APT_BoundsUtility.ExpandBounds(bounds.Value, APT_Settings.ExpandPercent, 0f));
                }
            }

            return zones;
        }

        private static List<Bounds> BuildExperimentalAutoZones(Transform[] roots)
        {
            List<Bounds> candidates = CollectAutoZoneCandidates(roots);
            if (candidates.Count == 0)
            {
                return new List<Bounds>();
            }

            List<List<Bounds>> groups = new List<List<Bounds>>();
            SplitBoundsRecursive(candidates, groups, 0);

            if (groups.Count == 0)
            {
                groups.Add(candidates);
            }

            List<Bounds> zones = new List<Bounds>(groups.Count);
            for (int i = 0; i < groups.Count; i++)
            {
                List<Bounds> group = groups[i];
                if (group == null || group.Count == 0)
                {
                    continue;
                }

                Bounds merged = APT_BoundsUtility.Encapsulate(group);
                zones.Add(APT_BoundsUtility.ExpandBounds(merged, APT_Settings.ExpandPercent, APT_Settings.AutoZonePadding));
            }

            return zones;
        }

        private static List<Bounds> CollectAutoZoneCandidates(Transform[] roots)
        {
            List<Bounds> list = new List<Bounds>();
            if (roots == null || roots.Length == 0)
            {
                return list;
            }

            HashSet<Transform> seen = new HashSet<Transform>();
            for (int i = 0; i < roots.Length; i++)
            {
                Transform root = roots[i];
                if (root == null)
                {
                    continue;
                }

                Transform[] children = root.GetComponentsInChildren<Transform>(true);
                for (int j = 0; j < children.Length; j++)
                {
                    Transform tr = children[j];
                    if (tr == null || !seen.Add(tr))
                    {
                        continue;
                    }

                    Bounds? candidate = null;
                    Renderer renderer = tr.GetComponent<Renderer>();
                    if (renderer != null && !(renderer is ParticleSystemRenderer) && !(renderer is TrailRenderer) && !(renderer is LineRenderer))
                    {
                        candidate = renderer.bounds;
                    }

                    if (!candidate.HasValue)
                    {
                        Collider collider = tr.GetComponent<Collider>();
                        if (collider != null)
                        {
                            candidate = collider.bounds;
                        }
                    }

                    if (!candidate.HasValue)
                    {
                        continue;
                    }

                    Bounds bounds = candidate.Value;
                    float largestAxis = Mathf.Max(bounds.size.x, Mathf.Max(bounds.size.y, bounds.size.z));
                    if (largestAxis < APT_Settings.AutoZoneMinMeshSize)
                    {
                        continue;
                    }

                    list.Add(bounds);
                }
            }

            return list;
        }

        private static void SplitBoundsRecursive(List<Bounds> source, List<List<Bounds>> output, int depth)
        {
            if (source == null || source.Count == 0)
            {
                return;
            }

            if (output.Count >= APT_Settings.AutoZoneMaxCount - 1 || source.Count < 2 || depth >= 8)
            {
                output.Add(source);
                return;
            }

            if (TrySplitBoundsGroup(source, out List<Bounds> left, out List<Bounds> right))
            {
                SplitBoundsRecursive(left, output, depth + 1);
                if (output.Count < APT_Settings.AutoZoneMaxCount)
                {
                    SplitBoundsRecursive(right, output, depth + 1);
                }
                else
                {
                    output.Add(right);
                }
            }
            else
            {
                output.Add(source);
            }
        }

        private static bool TrySplitBoundsGroup(List<Bounds> group, out List<Bounds> left, out List<Bounds> right)
        {
            left = null;
            right = null;

            if (group == null || group.Count < 2)
            {
                return false;
            }

            Bounds merged = APT_BoundsUtility.Encapsulate(group);
            int[] axisOrder = GetAxisPriority(merged);

            float bestGap = APT_Settings.AutoZoneSplitGap;
            int bestIndex = -1;
            List<Bounds> bestSorted = null;

            for (int axisIndex = 0; axisIndex < axisOrder.Length; axisIndex++)
            {
                int axis = axisOrder[axisIndex];
                float axisSize = GetAxisSize(merged, axis);
                if (axisSize < APT_Settings.AutoZoneMinMeshSize * 1.5f)
                {
                    continue;
                }

                List<Bounds> sorted = group.OrderBy(bounds => GetAxisCenter(bounds, axis)).ToList();
                for (int i = 0; i < sorted.Count - 1; i++)
                {
                    float current = GetAxisCenter(sorted[i], axis);
                    float next = GetAxisCenter(sorted[i + 1], axis);
                    float gap = next - current;
                    if (gap <= bestGap)
                    {
                        continue;
                    }

                    List<Bounds> maybeLeft = sorted.GetRange(0, i + 1);
                    List<Bounds> maybeRight = sorted.GetRange(i + 1, sorted.Count - (i + 1));
                    if (maybeLeft.Count == 0 || maybeRight.Count == 0)
                    {
                        continue;
                    }

                    Bounds leftBounds = APT_BoundsUtility.Encapsulate(maybeLeft);
                    Bounds rightBounds = APT_BoundsUtility.Encapsulate(maybeRight);
                    if (GetAxisSize(leftBounds, axis) < APT_Settings.AutoZoneMinMeshSize * 0.5f)
                    {
                        continue;
                    }

                    if (GetAxisSize(rightBounds, axis) < APT_Settings.AutoZoneMinMeshSize * 0.5f)
                    {
                        continue;
                    }

                    bestGap = gap;
                    bestIndex = i;
                    bestSorted = sorted;
                }
            }

            if (bestSorted == null || bestIndex < 0)
            {
                return false;
            }

            left = bestSorted.GetRange(0, bestIndex + 1);
            right = bestSorted.GetRange(bestIndex + 1, bestSorted.Count - (bestIndex + 1));
            return left.Count > 0 && right.Count > 0;
        }

        private static int[] GetAxisPriority(Bounds bounds)
        {
            List<KeyValuePair<int, float>> axes = new List<KeyValuePair<int, float>>
            {
                new KeyValuePair<int, float>(0, bounds.size.x),
                new KeyValuePair<int, float>(2, bounds.size.z)
            };

            if (!APT_Settings.AutoZoneXZOnly)
            {
                axes.Add(new KeyValuePair<int, float>(1, bounds.size.y));
            }

            return axes.OrderByDescending(pair => pair.Value).Select(pair => pair.Key).ToArray();
        }

        private static float GetAxisCenter(Bounds bounds, int axis)
        {
            switch (axis)
            {
                case 0: return bounds.center.x;
                case 1: return bounds.center.y;
                default: return bounds.center.z;
            }
        }

        private static float GetAxisSize(Bounds bounds, int axis)
        {
            switch (axis)
            {
                case 0: return bounds.size.x;
                case 1: return bounds.size.y;
                default: return bounds.size.z;
            }
        }
    }
}
#endif
