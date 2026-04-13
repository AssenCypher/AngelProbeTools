#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AngelProbeTools.Editor
{
    public static class APT_HierarchyBuilder
    {
        public const string FolderReflectionProbes = "Reflection Probes";
        public const string FolderLightProbeGroups = "Light Probe Groups";
        public const string FolderBoxTriggers = "Box Triggers";

        public static GameObject FindSharedRoot(Scene scene, string rootName)
        {
            string effectiveRootName = APT_Settings.SanitizeRootName(rootName);
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            for (int i = 0; i < allObjects.Length; i++)
            {
                GameObject go = allObjects[i];
                if (go == null || EditorUtility.IsPersistent(go))
                {
                    continue;
                }

                if (go.name == effectiveRootName && go.scene == scene)
                {
                    return go;
                }
            }

            return null;
        }

        public static GameObject FindOrCreateSharedRoot(Scene scene, string rootName)
        {
            string effectiveRootName = APT_Settings.SanitizeRootName(rootName);
            GameObject existing = FindSharedRoot(scene, effectiveRootName);
            if (existing != null)
            {
                return existing;
            }

            GameObject root = new GameObject(effectiveRootName);
            Undo.RegisterCreatedObjectUndo(root, "Create APT Shared Root");
            EditorSceneManager.MoveGameObjectToScene(root, scene);
            root.transform.position = Vector3.zero;
            root.transform.rotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            return root;
        }

        public static GameObject CreatePerRunRoot(Scene scene, Bounds pivotBounds, string rootName)
        {
            string effectiveRootName = APT_Settings.SanitizeRootName(rootName);
            string uniqueName = MakeUniqueSceneObjectName(scene, effectiveRootName);

            GameObject root = new GameObject(uniqueName);
            Undo.RegisterCreatedObjectUndo(root, "Create APT Root");
            EditorSceneManager.MoveGameObjectToScene(root, scene);
            root.transform.position = pivotBounds.center;
            root.transform.rotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            return root;
        }

        public static string BuildSuggestedRootNameFromSelection(Transform[] selection)
        {
            string label = APT_BoundsUtility.BuildSelectionLabel(selection);
            if (string.IsNullOrWhiteSpace(label))
            {
                return APT_Info.DefaultSharedRootName;
            }

            string sanitized = APT_Settings.SanitizeRootName(label.Replace("_plus", " + "));
            if (sanitized.EndsWith("Probe", System.StringComparison.OrdinalIgnoreCase) || sanitized.EndsWith("Probes", System.StringComparison.OrdinalIgnoreCase))
            {
                return sanitized;
            }

            return APT_Settings.SanitizeRootName(sanitized + " Probes");
        }

        public static Transform GetOrCreateFolder(Transform root, string folderName)
        {
            if (root == null)
            {
                return null;
            }

            for (int i = 0; i < root.childCount; i++)
            {
                Transform child = root.GetChild(i);
                if (child != null && child.name == folderName)
                {
                    return child;
                }
            }

            GameObject folder = new GameObject(folderName);
            Undo.RegisterCreatedObjectUndo(folder, "Create APT Folder");
            EditorSceneManager.MoveGameObjectToScene(folder, root.gameObject.scene);
            folder.transform.SetParent(root, false);
            folder.transform.localPosition = Vector3.zero;
            folder.transform.localRotation = Quaternion.identity;
            folder.transform.localScale = Vector3.one;
            return folder.transform;
        }

        private static string MakeUniqueSceneObjectName(Scene scene, string baseName)
        {
            string candidate = baseName;
            int suffix = 1;
            while (SceneContainsName(scene, candidate))
            {
                candidate = baseName + " " + suffix.ToString("00");
                suffix++;
            }

            return candidate;
        }

        private static bool SceneContainsName(Scene scene, string objectName)
        {
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            for (int i = 0; i < allObjects.Length; i++)
            {
                GameObject go = allObjects[i];
                if (go == null || EditorUtility.IsPersistent(go))
                {
                    continue;
                }

                if (go.scene == scene && go.name == objectName)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
#endif
