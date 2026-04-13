#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEditor;

namespace AngelProbeTools.Editor
{
    [InitializeOnLoad]
    public static class APT_APBridge
    {
        private const string FallbackModuleId = "ap.tools.probetools.free";
        private const string FallbackLocProviderId = "ap.tools.probetools.loc";
        private const int FallbackSortOrder = 420;

        private static bool attempted;
        private static bool locRegistered;
        private static bool moduleRegistered;

        static APT_APBridge()
        {
            EditorApplication.delayCall += EnsureRegistered;
        }

        public static void EnsureRegistered()
        {
            if (attempted && locRegistered && moduleRegistered)
            {
                return;
            }

            attempted = true;

            if (!TryRegisterLocProvider())
            {
                return;
            }

            TryRegisterToolModule();
        }

        public static bool TryOpenInsideAngelPanel()
        {
            Type apMainType = FindType("AngelPanel.Editor.AP_Main");
            if (apMainType == null)
            {
                return false;
            }

            MethodInfo openTab = apMainType.GetMethod("OpenTab", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string) }, null);
            if (openTab == null)
            {
                return false;
            }

            try
            {
                openTab.Invoke(null, new object[] { GetModuleId() });
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool TryRegisterLocProvider()
        {
            if (locRegistered)
            {
                return true;
            }

            Type providerType = FindType("AngelPanel.Editor.AP_LocProvider");
            Type bridgeType = FindType("AngelPanel.Editor.AP_ModuleBridge");
            if (providerType == null || bridgeType == null)
            {
                return false;
            }

            ConstructorInfo ctor = providerType.GetConstructor(new[] { typeof(string), typeof(int) });
            MethodInfo addMethod = providerType.GetMethod(
                "Add",
                BindingFlags.Public | BindingFlags.Instance,
                null,
                new[] { typeof(string), typeof(string), typeof(string), typeof(string), typeof(string) },
                null);
            MethodInfo registerMethod = bridgeType.GetMethod("TryRegisterLocProvider", BindingFlags.Public | BindingFlags.Static);

            if (ctor == null || addMethod == null || registerMethod == null)
            {
                return false;
            }

            object provider = ctor.Invoke(new object[] { GetLocProviderId(), 25 });
            foreach (KeyValuePair<string, string[]> pair in APT_Loc.AllEntries)
            {
                string[] values = pair.Value ?? Array.Empty<string>();
                addMethod.Invoke(provider, new object[]
                {
                    pair.Key,
                    values.Length > 0 ? values[0] : null,
                    values.Length > 1 ? values[1] : null,
                    values.Length > 2 ? values[2] : null,
                    values.Length > 3 ? values[3] : null,
                });
            }

            try
            {
                object result = registerMethod.Invoke(null, new[] { provider });
                locRegistered = result is bool ok && ok;
                return locRegistered;
            }
            catch
            {
                return false;
            }
        }

        private static bool TryRegisterToolModule()
        {
            if (moduleRegistered)
            {
                return true;
            }

            Type bridgeType = FindType("AngelPanel.Editor.AP_ModuleBridge");
            Type hostContextType = FindType("AngelPanel.Editor.AP_HostContext");
            if (bridgeType == null)
            {
                return false;
            }

            MethodInfo registerTool = null;
            foreach (MethodInfo method in bridgeType.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if (method.Name == "TryRegisterExternalTool")
                {
                    registerTool = method;
                    break;
                }
            }

            if (registerTool == null)
            {
                return false;
            }

            try
            {
                ParameterInfo[] parameters = registerTool.GetParameters();
                object[] args = new object[parameters.Length];
                args[0] = GetModuleId();
                args[1] = "AP_PT_NAME";
                args[2] = "AP_PT_SUMMARY";
                args[3] = GetSortOrder();
                args[4] = APT_Info.Version;
                args[5] = (Action)APT_MainWindow.OpenStandaloneWindow;
                args[6] = BuildCapabilities();
                if (parameters.Length > 7)
                {
                    args[7] = CreateDrawHostDelegate(hostContextType);
                }
                if (parameters.Length > 8)
                {
                    args[8] = APT_Info.AuthorName;
                }
                if (parameters.Length > 9)
                {
                    args[9] = string.Empty;
                }

                object result = registerTool.Invoke(null, args);
                moduleRegistered = result is bool ok && ok;
                return moduleRegistered;
            }
            catch
            {
                return false;
            }
        }

        private static object CreateDrawHostDelegate(Type hostContextType)
        {
            if (hostContextType == null)
            {
                return null;
            }

            MethodInfo targetMethod = typeof(APT_APBridge).GetMethod(nameof(DrawHostPageProxy), BindingFlags.NonPublic | BindingFlags.Static);
            if (targetMethod == null)
            {
                return null;
            }

            ParameterExpression contextParam = Expression.Parameter(hostContextType, "context");
            MethodCallExpression body = Expression.Call(targetMethod, Expression.Convert(contextParam, typeof(object)));
            Type actionType = typeof(Action<>).MakeGenericType(hostContextType);
            return Expression.Lambda(actionType, body, contextParam).Compile();
        }

        private static void DrawHostPageProxy(object hostContext)
        {
            APT_HostPage.DrawEmbedded(hostContext);
        }

        private static string[] BuildCapabilities()
        {
            return new[]
            {
                GetCapabilityValue("Root", "capability.probe"),
                GetCapabilityValue("ReflectionProbe", "capability.probe.reflectionProbe"),
                GetCapabilityValue("LightProbeGroup", "capability.probe.lightProbeGroup"),
                GetCapabilityValue("BoxTriggerCollider", "capability.probe.boxTriggerCollider"),
                GetCapabilityValue("BoundsExpand", "capability.probe.boundsExpand"),
                GetCapabilityValue("ZoneScaffold", "capability.probe.zoneScaffold")
            };
        }

        private static string GetCapabilityValue(string fieldName, string fallback)
        {
            Type capabilityType = FindType("AngelPanel.Editor.AP_ProbeCapabilityIds");
            if (capabilityType == null)
            {
                return fallback;
            }

            FieldInfo field = capabilityType.GetField(fieldName, BindingFlags.Public | BindingFlags.Static);
            if (field == null || field.FieldType != typeof(string))
            {
                return fallback;
            }

            return field.GetValue(null) as string ?? fallback;
        }

        private static string GetModuleId()
        {
            Type contractType = FindType("AngelPanel.Editor.AP_ProbeToolsBridgeContract");
            return GetStringField(contractType, "ModuleId", FallbackModuleId);
        }

        private static string GetLocProviderId()
        {
            Type contractType = FindType("AngelPanel.Editor.AP_ProbeToolsBridgeContract");
            return GetStringField(contractType, "LocProviderId", FallbackLocProviderId);
        }

        private static int GetSortOrder()
        {
            Type contractType = FindType("AngelPanel.Editor.AP_ProbeToolsBridgeContract");
            FieldInfo field = contractType != null ? contractType.GetField("DefaultSortOrder", BindingFlags.Public | BindingFlags.Static) : null;
            if (field != null && field.FieldType == typeof(int))
            {
                return (int)field.GetValue(null);
            }

            return FallbackSortOrder;
        }

        private static string GetStringField(Type type, string fieldName, string fallback)
        {
            FieldInfo field = type != null ? type.GetField(fieldName, BindingFlags.Public | BindingFlags.Static) : null;
            if (field != null && field.FieldType == typeof(string))
            {
                return field.GetValue(null) as string ?? fallback;
            }

            return fallback;
        }

        private static Type FindType(string fullName)
        {
            Type type = Type.GetType(fullName + ", AngelPanel.Core.Editor");
            if (type != null)
            {
                return type;
            }

            type = Type.GetType(fullName);
            if (type != null)
            {
                return type;
            }

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                try
                {
                    type = assemblies[i].GetType(fullName, false);
                    if (type != null)
                    {
                        return type;
                    }
                }
                catch
                {
                }
            }

            return null;
        }
    }
}
#endif
