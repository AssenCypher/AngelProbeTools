#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AngelProbeTools.Editor
{
    public static class APT_UIUtility
    {
        public static void RevealInstallRoot()
        {
            string installRoot = APT_Paths.ResolveInstallRootAbsolutePath();
            if (!string.IsNullOrEmpty(installRoot) && Directory.Exists(installRoot))
            {
                EditorUtility.RevealInFinder(installRoot);
                return;
            }

            string projectRoot = Application.dataPath.Replace("/Assets", string.Empty).Replace("\\Assets", string.Empty);
            EditorUtility.RevealInFinder(projectRoot);
        }
    }
}
#endif
