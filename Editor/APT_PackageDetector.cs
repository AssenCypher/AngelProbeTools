#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;

namespace AngelProbeTools.Editor
{
    public static class APT_PackageDetector
    {
        public static bool HasAngelPanelCore { get; private set; }
        public static bool HasVRCSDKWorlds { get; private set; }
        public static bool HasVrcSdkWorlds => HasVRCSDKWorlds;
        public static bool HasUdonSharp { get; private set; }
        public static bool HasLightingToolsDlp { get; private set; }
        public static bool HasMagicLightProbes { get; private set; }
        public static bool HasVRCLightVolumes { get; private set; }
        public static bool IsScanning { get; private set; }
        public static DateTime LastScanTime { get; private set; }

        private static bool initialized;

        public static void Ensure()
        {
            if (initialized)
            {
                return;
            }

            initialized = true;
            QuickProbe();
            EditorApplication.delayCall += FullScanSafe;
        }

        public static void RefreshNow()
        {
            if (IsScanning)
            {
                return;
            }

            FullScanSafe();
        }

        private static void QuickProbe()
        {
            HasAngelPanelCore = ProbeType(
                "AngelPanel.Editor.AP_ModuleBridge, AngelPanel.Core.Editor",
                "AngelPanel.Editor.AP_ModuleBridge");

            HasVRCSDKWorlds = ProbeType(
                "VRC.SDK3.Components.VRCSceneDescriptor, VRCSDK3",
                "VRC.SDK3.Components.VRCSceneDescriptor");

            HasUdonSharp = ProbeType(
                "UdonSharp.UdonSharpBehaviour, UdonSharp",
                "UdonSharp.UdonSharpBehaviour");

            HasLightingToolsDlp = ProbeType(
                "AngelLightingTools.Editor.DLP_ProbeBootstrap",
                "AngelPanel.LightingTools.Editor.DLP_ProbeBootstrap",
                "AngelLightingTools.Editor.DLP_MainWindow",
                "AngelPanel.LightingTools.Editor.DLP_MainWindow");

            HasMagicLightProbes = ProbeType(
                "MagicLightProbes.MLPQuickEditing",
                "MagicLightProbes.MLPVolume",
                "MagicLightProbes.MagicLightProbes");

            HasVRCLightVolumes = ProbeType(
                "RedSim.VRCLightVolumes.LightVolume",
                "RedSim.VRCLightVolumes.LightVolumeManager",
                "VRCLightVolumes.LightVolume",
                "VRCLightVolumes.LightVolumeManager");
        }

        private static bool ProbeType(params string[] names)
        {
            if (names == null)
            {
                return false;
            }

            for (int i = 0; i < names.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(names[i]))
                {
                    continue;
                }

                if (Type.GetType(names[i]) != null)
                {
                    return true;
                }
            }

            return false;
        }

        private static void FullScanSafe()
        {
            if (IsScanning)
            {
                return;
            }

            IsScanning = true;
            try
            {
                bool hasAngelPanel = false;
                bool hasWorlds = false;
                bool hasUdonSharp = false;
                bool hasLightingToolsDlp = false;
                bool hasMagicLightProbes = false;
                bool hasVRCLightVolumes = false;

                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                for (int i = 0; i < assemblies.Length; i++)
                {
                    Assembly assembly = assemblies[i];
                    string assemblyName = string.Empty;
                    try
                    {
                        assemblyName = assembly.GetName().Name ?? string.Empty;
                    }
                    catch
                    {
                        assemblyName = string.Empty;
                    }

                    if (!hasLightingToolsDlp)
                    {
                        string loweredAssembly = assemblyName.ToLowerInvariant();
                        hasLightingToolsDlp = loweredAssembly.Contains("lightingtools") || loweredAssembly.Contains("dlp");
                    }

                    if (!hasMagicLightProbes)
                    {
                        hasMagicLightProbes = assemblyName.IndexOf("MagicLightProbes", StringComparison.OrdinalIgnoreCase) >= 0;
                    }

                    if (!hasVRCLightVolumes)
                    {
                        string loweredAssembly = assemblyName.ToLowerInvariant();
                        hasVRCLightVolumes = loweredAssembly.Contains("lightvolume") || loweredAssembly.Contains("vrclightvol");
                    }

                    Type[] types;
                    try
                    {
                        types = assembly.GetTypes();
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        types = ex.Types;
                    }
                    catch
                    {
                        types = null;
                    }

                    if (types == null)
                    {
                        continue;
                    }

                    for (int j = 0; j < types.Length; j++)
                    {
                        Type type = types[j];
                        if (type == null)
                        {
                            continue;
                        }

                        string fullName = type.FullName ?? type.Name ?? string.Empty;
                        string lowered = fullName.ToLowerInvariant();

                        if (!hasAngelPanel && fullName == "AngelPanel.Editor.AP_ModuleBridge")
                        {
                            hasAngelPanel = true;
                        }

                        if (!hasWorlds && fullName == "VRC.SDK3.Components.VRCSceneDescriptor")
                        {
                            hasWorlds = true;
                        }

                        if (!hasUdonSharp && fullName == "UdonSharp.UdonSharpBehaviour")
                        {
                            hasUdonSharp = true;
                        }

                        if (!hasLightingToolsDlp)
                        {
                            hasLightingToolsDlp = lowered.Contains("lightingtools") || lowered.Contains(".dlp_") || lowered.Contains(".dlp.");
                        }

                        if (!hasMagicLightProbes)
                        {
                            hasMagicLightProbes = lowered.Contains("magiclightprobes") || lowered.Contains("mlpvolume");
                        }

                        if (!hasVRCLightVolumes)
                        {
                            hasVRCLightVolumes = lowered.Contains("lightvolume") || lowered.Contains("vrclightvol");
                        }

                        if (hasAngelPanel && hasWorlds && hasUdonSharp && hasLightingToolsDlp && hasMagicLightProbes && hasVRCLightVolumes)
                        {
                            break;
                        }
                    }
                }

                HasAngelPanelCore = hasAngelPanel;
                HasVRCSDKWorlds = hasWorlds;
                HasUdonSharp = hasUdonSharp;
                HasLightingToolsDlp = hasLightingToolsDlp;
                HasMagicLightProbes = hasMagicLightProbes;
                HasVRCLightVolumes = hasVRCLightVolumes;
                LastScanTime = DateTime.Now;
            }
            finally
            {
                IsScanning = false;
            }
        }
    }
}
#endif
