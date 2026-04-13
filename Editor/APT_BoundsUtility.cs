#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace AngelProbeTools.Editor
{
    public static class APT_BoundsUtility
    {
        public static Bounds? CollectBounds(Transform[] roots, bool includeRenderers, bool includeColliders)
        {
            if (roots == null || roots.Length == 0)
            {
                return null;
            }

            bool hasAny = false;
            Bounds collected = new Bounds();
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

                    if (includeRenderers)
                    {
                        Renderer renderer = tr.GetComponent<Renderer>();
                        if (renderer != null && !(renderer is ParticleSystemRenderer) && !(renderer is TrailRenderer) && !(renderer is LineRenderer))
                        {
                            if (!hasAny)
                            {
                                collected = renderer.bounds;
                                hasAny = true;
                            }
                            else
                            {
                                collected.Encapsulate(renderer.bounds);
                            }
                        }
                    }

                    if (includeColliders)
                    {
                        Collider collider = tr.GetComponent<Collider>();
                        if (collider != null)
                        {
                            if (!hasAny)
                            {
                                collected = collider.bounds;
                                hasAny = true;
                            }
                            else
                            {
                                collected.Encapsulate(collider.bounds);
                            }
                        }
                    }
                }
            }

            return hasAny ? collected : (Bounds?)null;
        }

        public static Bounds ExpandBounds(Bounds bounds, float expandPercent, float extraPadding)
        {
            Bounds expanded = bounds;
            expanded.Expand(expanded.size * Mathf.Clamp(expandPercent, 0f, 0.20f));
            if (extraPadding > 0f)
            {
                expanded.Expand(extraPadding * 2f);
            }

            return expanded;
        }

        public static Bounds Encapsulate(IList<Bounds> bounds)
        {
            if (bounds == null || bounds.Count == 0)
            {
                return new Bounds(Vector3.zero, Vector3.one);
            }

            Bounds merged = bounds[0];
            for (int i = 1; i < bounds.Count; i++)
            {
                merged.Encapsulate(bounds[i]);
            }

            return merged;
        }

        public static string BuildSelectionLabel(Transform[] transforms)
        {
            if (transforms == null || transforms.Length == 0)
            {
                return "Selection";
            }

            if (transforms.Length == 1)
            {
                return SanitizeName(transforms[0].name);
            }

            return SanitizeName(transforms[0].name) + "_plus" + (transforms.Length - 1);
        }

        public static string SanitizeName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "Selection";
            }

            return value.Replace("/", "_")
                        .Replace("\\", "_")
                        .Replace(":", "_")
                        .Replace("*", "_")
                        .Replace("?", "_")
                        .Replace("\"", "_")
                        .Replace("<", "_")
                        .Replace(">", "_")
                        .Replace("|", "_");
        }
    }
}
#endif
