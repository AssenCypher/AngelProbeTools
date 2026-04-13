#if UNITY_EDITOR
using UnityEditor;

namespace AngelProbeTools.Editor
{
    [InitializeOnLoad]
    public static class APT_Bootstrap
    {
        static APT_Bootstrap()
        {
            EditorApplication.delayCall += Boot;
        }

        private static void Boot()
        {
            APT_PackageDetector.Ensure();
            APT_Loc.Ensure();
        }
    }
}
#endif
