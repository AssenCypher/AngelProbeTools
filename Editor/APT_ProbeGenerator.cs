#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AngelProbeTools.Editor
{
    public static class APT_ProbeGenerator
    {
        private const int MaxRecommendedProbeCount = 24000;

        public static GameObject LastCreatedObject { get; private set; }
        public static string LastRunSummary { get; private set; }

        public static bool GenerateForSelection()
        {
            Transform[] selected = Selection.transforms;
            if (selected == null || selected.Length == 0)
            {
                EditorUtility.DisplayDialog(APT_Loc.T("AP_PT_DIALOG_NO_SELECTION"), APT_Loc.T("AP_PT_DIALOG_NO_SELECTION_BODY"), APT_Loc.T("AP_PT_OK"));
                return false;
            }

            if (!APT_Settings.HasAnyGeneratorEnabled)
            {
                EditorUtility.DisplayDialog(APT_Loc.T("AP_PT_DIALOG_NOTHING_ENABLED"), APT_Loc.T("AP_PT_DIALOG_NOTHING_ENABLED_BODY"), APT_Loc.T("AP_PT_OK"));
                return false;
            }

            Bounds? combined = APT_BoundsUtility.CollectBounds(selected, true, true);
            if (!combined.HasValue)
            {
                EditorUtility.DisplayDialog(APT_Loc.T("AP_PT_DIALOG_NO_BOUNDS"), APT_Loc.T("AP_PT_DIALOG_NO_BOUNDS_BODY"), APT_Loc.T("AP_PT_OK"));
                return false;
            }

            Scene targetScene = selected[0] != null && selected[0].gameObject.scene.IsValid()
                ? selected[0].gameObject.scene
                : SceneManager.GetActiveScene();

            List<Bounds> zones = APT_ZoneBuilder.BuildGenerationZones(selected, combined.Value);
            if (zones == null || zones.Count == 0)
            {
                zones = new List<Bounds>
                {
                    APT_BoundsUtility.ExpandBounds(combined.Value, APT_Settings.ExpandPercent, 0f)
                };
            }

            Undo.IncrementCurrentGroup();
            int undoGroup = Undo.GetCurrentGroup();

            string rootName = APT_Settings.RootName;
            GameObject root = APT_Settings.HierarchyMode == APT_Settings.HierarchyPerRunRoot
                ? APT_HierarchyBuilder.CreatePerRunRoot(targetScene, combined.Value, rootName)
                : APT_HierarchyBuilder.FindOrCreateSharedRoot(targetScene, rootName);

            Transform reflectionFolder = null;
            Transform lpgFolder = null;
            Transform triggerFolder = null;

            if (APT_Settings.CreateReflectionProbe)
            {
                reflectionFolder = APT_HierarchyBuilder.GetOrCreateFolder(root.transform, APT_HierarchyBuilder.FolderReflectionProbes);
            }

            if (APT_Settings.CreateLpg)
            {
                lpgFolder = APT_HierarchyBuilder.GetOrCreateFolder(root.transform, APT_HierarchyBuilder.FolderLightProbeGroups);
            }

            if (APT_Settings.CreateTrigger)
            {
                triggerFolder = APT_HierarchyBuilder.GetOrCreateFolder(root.transform, APT_HierarchyBuilder.FolderBoxTriggers);
            }

            string selectionScope = APT_NameUtility.BuildSelectionScope(selected, rootName);
            LastCreatedObject = null;

            int createdCount = 0;
            int updatedCount = 0;
            int replacedCount = 0;
            int lpgProbeCount = 0;

            for (int i = 0; i < zones.Count; i++)
            {
                Bounds zone = zones[i];

                if (APT_Settings.CreateReflectionProbe && reflectionFolder != null)
                {
                    PrepareStats stats = new PrepareStats();
                    LastCreatedObject = CreateOrUpdateReflectionProbe(reflectionFolder, zone, selectionScope, i, zones.Count, APT_Settings.GetEffectiveResultMode(APT_Settings.FeatureReflection), ref stats);
                    createdCount += stats.CreatedCount;
                    updatedCount += stats.UpdatedCount;
                    replacedCount += stats.ReplacedCount;
                }

                if (APT_Settings.CreateLpg && lpgFolder != null)
                {
                    PrepareStats stats = new PrepareStats();
                    LastCreatedObject = CreateOrUpdateLightProbeGroup(lpgFolder, zone, APT_Settings.GridStep, selectionScope, i, zones.Count, APT_Settings.GetEffectiveResultMode(APT_Settings.FeatureLightProbeGroup), ref stats, out int probeCount);
                    createdCount += stats.CreatedCount;
                    updatedCount += stats.UpdatedCount;
                    replacedCount += stats.ReplacedCount;
                    lpgProbeCount += probeCount;
                }

                if (APT_Settings.CreateTrigger && triggerFolder != null)
                {
                    PrepareStats stats = new PrepareStats();
                    LastCreatedObject = CreateOrUpdateBoxTrigger(triggerFolder, zone, selectionScope, i, zones.Count, APT_Settings.GetEffectiveResultMode(APT_Settings.FeatureTriggerScaffold), ref stats);
                    createdCount += stats.CreatedCount;
                    updatedCount += stats.UpdatedCount;
                    replacedCount += stats.ReplacedCount;
                }
            }

            if (LastCreatedObject != null)
            {
                Selection.activeGameObject = LastCreatedObject;
                EditorGUIUtility.PingObject(LastCreatedObject);
            }
            else
            {
                Selection.activeGameObject = root;
                EditorGUIUtility.PingObject(root);
            }

            Undo.CollapseUndoOperations(undoGroup);
            LastRunSummary = BuildLastRunSummary(root, zones.Count, createdCount, updatedCount, replacedCount, lpgProbeCount);
            return true;
        }

        public static GameObject FocusCurrentRootInActiveScene()
        {
            Scene scene = SceneManager.GetActiveScene();
            GameObject root = APT_HierarchyBuilder.FindSharedRoot(scene, APT_Settings.RootName);
            if (root == null)
            {
                return null;
            }

            Selection.activeGameObject = root;
            EditorGUIUtility.PingObject(root);
            return root;
        }

        private static GameObject CreateOrUpdateReflectionProbe(Transform parent, Bounds bounds, string selectionScope, int zoneIndex, int zoneCount, int resultMode, ref PrepareStats stats)
        {
            string objectName = APT_NameUtility.BuildGeneratedName("Reflection Probe", selectionScope, zoneIndex, zoneCount);
            GameObject go = PrepareGeneratedObject(parent, objectName, "Create Reflection Probe", resultMode, ref stats);
            if (go == null)
            {
                return null;
            }

            ReflectionProbe probe = go.GetComponent<ReflectionProbe>();
            if (probe == null)
            {
                probe = Undo.AddComponent<ReflectionProbe>(go);
            }

            probe.mode = UnityEngine.Rendering.ReflectionProbeMode.Baked;
            probe.boxProjection = true;
            probe.center = Vector3.zero;
            probe.size = bounds.size;
            go.transform.position = bounds.center;
            return go;
        }

        private static GameObject CreateOrUpdateLightProbeGroup(Transform parent, Bounds bounds, float requestedStep, string selectionScope, int zoneIndex, int zoneCount, int resultMode, ref PrepareStats stats, out int probeCount)
        {
            string objectName = APT_NameUtility.BuildGeneratedName("Light Probe Group", selectionScope, zoneIndex, zoneCount);
            GameObject go = PrepareGeneratedObject(parent, objectName, "Create LightProbeGroup", resultMode, ref stats);
            if (go == null)
            {
                probeCount = 0;
                return null;
            }

            LightProbeGroup lightProbeGroup = go.GetComponent<LightProbeGroup>();
            if (lightProbeGroup == null)
            {
                lightProbeGroup = Undo.AddComponent<LightProbeGroup>(go);
            }

            go.transform.position = bounds.center;
            List<Vector3> positions = BuildProbeGrid(bounds, go.transform.position, requestedStep);
            lightProbeGroup.probePositions = positions.ToArray();
            probeCount = positions.Count;
            return go;
        }

        private static GameObject CreateOrUpdateBoxTrigger(Transform parent, Bounds bounds, string selectionScope, int zoneIndex, int zoneCount, int resultMode, ref PrepareStats stats)
        {
            string objectName = APT_NameUtility.BuildGeneratedName("Trigger Scaffold", selectionScope, zoneIndex, zoneCount);
            GameObject go = PrepareGeneratedObject(parent, objectName, "Create Box Trigger Collider", resultMode, ref stats);
            if (go == null)
            {
                return null;
            }

            go.transform.position = bounds.center;

            BoxCollider boxCollider = go.GetComponent<BoxCollider>();
            if (boxCollider == null)
            {
                boxCollider = Undo.AddComponent<BoxCollider>(go);
            }

            boxCollider.isTrigger = true;
            boxCollider.center = Vector3.zero;
            boxCollider.size = bounds.size;

            Rigidbody rigidbody = go.GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                rigidbody = Undo.AddComponent<Rigidbody>(go);
            }

            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
            return go;
        }

        private static GameObject PrepareGeneratedObject(Transform parent, string objectName, string undoLabel, int resultMode, ref PrepareStats stats)
        {
            GameObject existing = FindDirectChild(parent, objectName);
            GameObject target;

            switch (resultMode)
            {
                case APT_Settings.ResultReplaceMatching:
                    if (existing != null)
                    {
                        Undo.DestroyObjectImmediate(existing);
                        stats.ReplacedCount++;
                    }

                    target = CreateGeneratedObject(parent, objectName, undoLabel);
                    stats.CreatedCount++;
                    return target;

                case APT_Settings.ResultUpdateMatching:
                    if (existing != null)
                    {
                        Undo.RecordObject(existing.transform, "Update APT Generated Object");
                        target = existing;
                        stats.UpdatedCount++;
                    }
                    else
                    {
                        target = CreateGeneratedObject(parent, objectName, undoLabel);
                        stats.CreatedCount++;
                    }
                    break;

                default:
                    target = CreateGeneratedObject(parent, objectName, undoLabel);
                    stats.CreatedCount++;
                    break;
            }

            target.name = objectName;
            target.transform.SetParent(parent, false);
            target.transform.localRotation = Quaternion.identity;
            target.transform.localScale = Vector3.one;
            return target;
        }

        private static GameObject CreateGeneratedObject(Transform parent, string objectName, string undoLabel)
        {
            GameObject go = new GameObject(objectName);
            Undo.RegisterCreatedObjectUndo(go, undoLabel);
            EditorSceneManager.MoveGameObjectToScene(go, parent.gameObject.scene);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            return go;
        }

        private static GameObject FindDirectChild(Transform parent, string objectName)
        {
            if (parent == null || string.IsNullOrWhiteSpace(objectName))
            {
                return null;
            }

            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (child != null && child.name == objectName)
                {
                    return child.gameObject;
                }
            }

            return null;
        }

        private static string BuildLastRunSummary(GameObject root, int zoneCount, int createdCount, int updatedCount, int replacedCount, int lpgProbeCount)
        {
            string rootName = root != null ? root.name : APT_Settings.RootName;
            string resultMode = APT_Loc.T(APT_Settings.BuildResultHandlingSummaryLoc());
            return string.Format(
                APT_Loc.T("AP_PT_LAST_RUN_FMT"),
                rootName,
                zoneCount,
                createdCount,
                updatedCount,
                replacedCount,
                lpgProbeCount,
                resultMode);
        }

        private static List<Vector3> BuildProbeGrid(Bounds bounds, Vector3 pivotWorldPosition, float requestedStep)
        {
            float safeStep = Mathf.Max(0.25f, requestedStep);
            Vector3 size = bounds.size;
            int countX = ComputeAxisSampleCount(size.x, safeStep);
            int countY = ComputeAxisSampleCount(size.y, safeStep);
            int countZ = ComputeAxisSampleCount(size.z, safeStep);

            long total = (long)countX * countY * countZ;
            if (total > MaxRecommendedProbeCount)
            {
                float scale = Mathf.Pow(total / (float)MaxRecommendedProbeCount, 1f / 3f);
                safeStep *= Mathf.Max(1f, scale);
                countX = ComputeAxisSampleCount(size.x, safeStep);
                countY = ComputeAxisSampleCount(size.y, safeStep);
                countZ = ComputeAxisSampleCount(size.z, safeStep);
            }

            float[] xs = BuildAxisValues(bounds.min.x, bounds.max.x, countX);
            float[] ys = BuildAxisValues(bounds.min.y, bounds.max.y, countY);
            float[] zs = BuildAxisValues(bounds.min.z, bounds.max.z, countZ);

            List<Vector3> positions = new List<Vector3>(Mathf.Max(1, xs.Length * ys.Length * zs.Length));
            for (int xi = 0; xi < xs.Length; xi++)
            {
                for (int yi = 0; yi < ys.Length; yi++)
                {
                    for (int zi = 0; zi < zs.Length; zi++)
                    {
                        Vector3 world = new Vector3(xs[xi], ys[yi], zs[zi]);
                        positions.Add(world - pivotWorldPosition);
                    }
                }
            }

            if (positions.Count == 0)
            {
                positions.Add(Vector3.zero);
            }

            return positions;
        }

        private static int ComputeAxisSampleCount(float size, float step)
        {
            if (size <= 0.001f)
            {
                return 1;
            }

            return Mathf.Max(2, Mathf.CeilToInt(size / Mathf.Max(0.25f, step)) + 1);
        }

        private static float[] BuildAxisValues(float min, float max, int count)
        {
            if (count <= 1 || Mathf.Abs(max - min) <= 0.0001f)
            {
                return new[] { (min + max) * 0.5f };
            }

            float[] values = new float[count];
            float denominator = count - 1f;
            for (int i = 0; i < count; i++)
            {
                float t = i / denominator;
                values[i] = Mathf.Lerp(min, max, t);
            }

            values[0] = min;
            values[count - 1] = max;
            return values;
        }

        private struct PrepareStats
        {
            public int CreatedCount;
            public int UpdatedCount;
            public int ReplacedCount;
        }
    }
}
#endif
