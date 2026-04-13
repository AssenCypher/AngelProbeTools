#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace AngelProbeTools.Editor
{
    public sealed class APT_MainWindow : EditorWindow
    {
        private Vector2 scroll;

        [MenuItem(APT_Info.MenuRoot + "/Open", priority = 0)]
        public static void OpenDefault()
        {
            APT_Loc.Ensure();
            APT_PackageDetector.Ensure();

            if (APT_PackageDetector.HasAngelPanelCore && APT_APBridge.TryOpenInsideAngelPanel())
            {
                return;
            }

            OpenStandaloneWindow();
        }

        [MenuItem(APT_Info.MenuRoot + "/Open Standalone Window", priority = 1)]
        public static void OpenStandaloneWindow()
        {
            APT_Loc.Ensure();
            APT_MainWindow window = GetWindow<APT_MainWindow>();
            window.titleContent = new GUIContent(APT_Loc.T("AP_PT_WINDOW_TITLE"));
            window.minSize = new Vector2(420f, 520f);
            window.Show();
            window.Focus();
        }

        [MenuItem(APT_Info.MenuRoot + "/Open Window", priority = 2)]
        public static void OpenWindowLegacyAlias()
        {
            OpenStandaloneWindow();
        }

        private void OnEnable()
        {
            APT_Loc.Ensure();
            titleContent = new GUIContent(APT_Loc.T("AP_PT_WINDOW_TITLE"));
            APT_PackageDetector.Ensure();
        }

        private void OnGUI()
        {
            APT_UI.DrawStandalone(this, ref scroll);
        }
    }
}
#endif
