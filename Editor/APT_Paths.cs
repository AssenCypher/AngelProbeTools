#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AngelProbeTools.Editor
{
    public static class APT_Paths
    {
        private const string InfoScriptName = "APT_Info";
        private const string EditorSuffix = "/Editor/APT_Info.cs";

        public static string ResolveInstallRootAssetPath()
        {
            string[] guids = AssetDatabase.FindAssets(InfoScriptName + " t:MonoScript");
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (path.EndsWith(EditorSuffix))
                {
                    return path.Substring(0, path.Length - EditorSuffix.Length);
                }
            }

            guids = AssetDatabase.FindAssets(InfoScriptName);
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (path.EndsWith(EditorSuffix))
                {
                    return path.Substring(0, path.Length - EditorSuffix.Length);
                }
            }

            if (AssetDatabase.IsValidFolder(APT_Info.AssetsRoot))
            {
                return APT_Info.AssetsRoot;
            }

            return string.Empty;
        }

        public static bool IsInstalledInAssets()
        {
            string root = ResolveInstallRootAssetPath();
            return !string.IsNullOrEmpty(root) && root.StartsWith("Assets/");
        }

        public static string ResolveInstallRootAbsolutePath()
        {
            string assetPath = ResolveInstallRootAssetPath();
            if (string.IsNullOrEmpty(assetPath))
            {
                return string.Empty;
            }

            string projectRoot = Application.dataPath.Replace("/Assets", string.Empty).Replace("\\Assets", string.Empty);
            string normalizedRelativePath = assetPath.Replace('/', Path.DirectorySeparatorChar);
            return Path.GetFullPath(Path.Combine(projectRoot, normalizedRelativePath));
        }
    }
}
#endif
