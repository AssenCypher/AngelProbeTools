#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AngelProbeTools.Editor
{
    public static class APT_UI
    {
        public static void DrawStandalone(EditorWindow window, ref Vector2 scroll)
        {
            APT_Loc.Ensure();
            APT_PackageDetector.Ensure();

            scroll = EditorGUILayout.BeginScrollView(scroll);
            DrawContent(isStandalone: true, hostContext: null, windowWidth: Mathf.Max(320f, window != null ? window.position.width : 460f));
            EditorGUILayout.EndScrollView();
        }

        public static void DrawEmbeddedHostPage(object hostContext)
        {
            APT_Loc.Ensure();
            APT_PackageDetector.Ensure();
            float windowWidth = GetHostWidth(hostContext);
            DrawContent(isStandalone: false, hostContext: hostContext, windowWidth: windowWidth);
        }

        private static void DrawContent(bool isStandalone, object hostContext, float windowWidth)
        {
            bool compact = windowWidth < 640f;
            DrawHeader(isStandalone, compact);
            GUILayout.Space(8f);
            DrawGenerationSettings(compact);
            GUILayout.Space(8f);
            DrawActions(isStandalone, compact);
            GUILayout.Space(8f);
            DrawOverviewFoldout();
            GUILayout.Space(8f);
            DrawOptionalFeaturesFoldout();
            GUILayout.Space(8f);
            DrawStatusFoldout(isStandalone, compact);
        }

        private static void DrawHeader(bool isStandalone, bool compact)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label(APT_Loc.T("AP_PT_WINDOW_TITLE"), new GUIStyle(EditorStyles.boldLabel)
                    {
                        fontSize = compact ? 14 : 15,
                        wordWrap = true
                    });

                    GUILayout.FlexibleSpace();
                    GUILayout.Label("v" + APT_Info.Version, EditorStyles.miniLabel, GUILayout.Width(48f));

                    if (!isStandalone && APT_PackageDetector.HasAngelPanelCore)
                    {
                        if (GUILayout.Button(APT_Loc.T("AP_PT_ACTION_OPEN_STANDALONE"), EditorStyles.miniButton, GUILayout.Height(18f), GUILayout.MinWidth(110f)))
                        {
                            APT_MainWindow.OpenStandaloneWindow();
                        }
                    }
                    else if (isStandalone && APT_PackageDetector.HasAngelPanelCore)
                    {
                        if (GUILayout.Button(APT_Loc.T("AP_PT_ACTION_OPEN_IN_AP"), EditorStyles.miniButton, GUILayout.Height(18f), GUILayout.MinWidth(110f)))
                        {
                            APT_MainWindow.OpenDefault();
                        }
                    }

                    if (GUILayout.Button(APT_Loc.T("AP_PT_ACTION_OPEN_PACKAGE_ROOT"), EditorStyles.miniButton, GUILayout.Height(18f), GUILayout.MinWidth(96f)))
                    {
                        APT_UIUtility.RevealInstallRoot();
                    }
                }
            }
        }

        private static void DrawGenerationSettings(bool compact)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                GUILayout.Label(APT_Loc.T("AP_PT_SECTION_GEN"), EditorStyles.boldLabel);

                APT_Settings.RootName = EditorGUILayout.TextField(APT_Loc.T("AP_PT_ROOT_NAME"), APT_Settings.RootName);

                if (compact)
                {
                    if (GUILayout.Button(APT_Loc.T("AP_PT_ACTION_USE_SELECTION_ROOT"), EditorStyles.miniButton))
                    {
                        APT_Settings.RootName = APT_HierarchyBuilder.BuildSuggestedRootNameFromSelection(Selection.transforms);
                    }

                    if (GUILayout.Button(APT_Loc.T("AP_PT_ACTION_RESET_ROOT"), EditorStyles.miniButton))
                    {
                        APT_Settings.RootName = APT_Info.DefaultSharedRootName;
                    }
                }
                else
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.Space(EditorGUIUtility.labelWidth);

                        if (GUILayout.Button(APT_Loc.T("AP_PT_ACTION_USE_SELECTION_ROOT"), EditorStyles.miniButton, GUILayout.MaxWidth(160f)))
                        {
                            APT_Settings.RootName = APT_HierarchyBuilder.BuildSuggestedRootNameFromSelection(Selection.transforms);
                        }

                        if (GUILayout.Button(APT_Loc.T("AP_PT_ACTION_RESET_ROOT"), EditorStyles.miniButton, GUILayout.MaxWidth(80f)))
                        {
                            APT_Settings.RootName = APT_Info.DefaultSharedRootName;
                        }

                        GUILayout.FlexibleSpace();
                    }
                }

                DrawSubtleNote(APT_Loc.T("AP_PT_ROOT_NAME_NOTE"));

                APT_Settings.HierarchyMode = EditorGUILayout.Popup(
                    APT_Loc.T("AP_PT_HIERARCHY_MODE"),
                    APT_Settings.HierarchyMode,
                    new[]
                    {
                        APT_Loc.T("AP_PT_HIERARCHY_SHARED"),
                        APT_Loc.T("AP_PT_HIERARCHY_LEGACY")
                    });

                APT_Settings.ZoneMode = EditorGUILayout.Popup(
                    APT_Loc.T("AP_PT_ZONE_MODE"),
                    APT_Settings.ZoneMode,
                    new[]
                    {
                        APT_Loc.T("AP_PT_ZONE_COMBINED"),
                        APT_Loc.T("AP_PT_ZONE_PER_ROOT"),
                        APT_Loc.T("AP_PT_ZONE_AUTO_SPLIT")
                    });

                APT_Settings.ResultMode = EditorGUILayout.Popup(
                    APT_Loc.T("AP_PT_RESULT_MODE"),
                    APT_Settings.ResultMode,
                    BuildResultModeOptions());
                APT_Settings.UsePerFeatureResultHandling = EditorGUILayout.ToggleLeft(APT_Loc.T("AP_PT_RESULT_PER_FEATURE"), APT_Settings.UsePerFeatureResultHandling);

                if (APT_Settings.UsePerFeatureResultHandling)
                {
                    EditorGUI.indentLevel++;
                    if (APT_Settings.CreateReflectionProbe)
                    {
                        APT_Settings.ReflectionResultMode = EditorGUILayout.Popup(
                            APT_Loc.T("AP_PT_RESULT_REFLECTION"),
                            APT_Settings.ReflectionResultMode,
                            BuildResultModeOptions());
                    }

                    if (APT_Settings.CreateLpg)
                    {
                        APT_Settings.LightProbeGroupResultMode = EditorGUILayout.Popup(
                            APT_Loc.T("AP_PT_RESULT_LPG"),
                            APT_Settings.LightProbeGroupResultMode,
                            BuildResultModeOptions());
                    }

                    if (APT_Settings.CreateTrigger)
                    {
                        APT_Settings.TriggerResultMode = EditorGUILayout.Popup(
                            APT_Loc.T("AP_PT_RESULT_TRIGGER"),
                            APT_Settings.TriggerResultMode,
                            BuildResultModeOptions());
                    }

                    EditorGUI.indentLevel--;
                    DrawSubtleNote(APT_Loc.T("AP_PT_RESULT_PER_FEATURE_NOTE"));
                }
                else
                {
                    DrawSubtleNote(APT_Loc.T("AP_PT_RESULT_NOTE"));
                }

                APT_Settings.ExpandPercent = EditorGUILayout.Slider(APT_Loc.T("AP_PT_BOUNDS_EXPAND"), APT_Settings.ExpandPercent * 100f, 0f, 20f) / 100f;
                APT_Settings.GridStep = Mathf.Max(0.25f, EditorGUILayout.FloatField(APT_Loc.T("AP_PT_GRID_STEP"), APT_Settings.GridStep));
                DrawSubtleNote(APT_Loc.T("AP_PT_LPG_NOTE"));

                if (compact)
                {
                    APT_Settings.CreateReflectionProbe = EditorGUILayout.ToggleLeft(APT_Loc.T("AP_PT_CREATE_REFLECTION"), APT_Settings.CreateReflectionProbe);
                    APT_Settings.CreateLpg = EditorGUILayout.ToggleLeft(APT_Loc.T("AP_PT_CREATE_LPG"), APT_Settings.CreateLpg);
                    APT_Settings.CreateTrigger = EditorGUILayout.ToggleLeft(APT_Loc.T("AP_PT_CREATE_TRIGGER"), APT_Settings.CreateTrigger);
                }
                else
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        APT_Settings.CreateReflectionProbe = EditorGUILayout.ToggleLeft(APT_Loc.T("AP_PT_CREATE_REFLECTION"), APT_Settings.CreateReflectionProbe);
                        APT_Settings.CreateLpg = EditorGUILayout.ToggleLeft(APT_Loc.T("AP_PT_CREATE_LPG"), APT_Settings.CreateLpg);
                        APT_Settings.CreateTrigger = EditorGUILayout.ToggleLeft(APT_Loc.T("AP_PT_CREATE_TRIGGER"), APT_Settings.CreateTrigger);
                    }
                }

                if (APT_Settings.CreateTrigger)
                {
                    DrawSubtleNote(APT_Loc.T("AP_PT_TRIGGER_NOTE"));
                }

                if (APT_Settings.ZoneMode == APT_Settings.ZoneExperimentalAutoSplit)
                {
                    GUILayout.Space(6f);
                    GUILayout.Label(APT_Loc.T("AP_PT_SECTION_AUTO"), EditorStyles.boldLabel);
                    DrawSubtleNote(APT_Loc.T("AP_PT_AUTO_NOTE"));
                    APT_Settings.AutoZoneMinMeshSize = Mathf.Max(0.5f, EditorGUILayout.FloatField(APT_Loc.T("AP_PT_AUTO_MIN_SIZE"), APT_Settings.AutoZoneMinMeshSize));
                    APT_Settings.AutoZoneSplitGap = Mathf.Max(0.5f, EditorGUILayout.FloatField(APT_Loc.T("AP_PT_AUTO_SPLIT_GAP"), APT_Settings.AutoZoneSplitGap));
                    APT_Settings.AutoZonePadding = Mathf.Max(0f, EditorGUILayout.FloatField(APT_Loc.T("AP_PT_AUTO_PADDING"), APT_Settings.AutoZonePadding));
                    APT_Settings.AutoZoneMaxCount = Mathf.Clamp(EditorGUILayout.IntField(APT_Loc.T("AP_PT_AUTO_MAX_COUNT"), APT_Settings.AutoZoneMaxCount), 1, 64);
                    APT_Settings.AutoZoneXZOnly = EditorGUILayout.ToggleLeft(APT_Loc.T("AP_PT_AUTO_XZ_ONLY"), APT_Settings.AutoZoneXZOnly);
                }
            }
        }


        private static string[] BuildResultModeOptions()
        {
            return new[]
            {
                APT_Loc.T("AP_PT_RESULT_CREATE"),
                APT_Loc.T("AP_PT_RESULT_REPLACE"),
                APT_Loc.T("AP_PT_RESULT_UPDATE")
            };
        }

        private static void DrawActions(bool isStandalone, bool compact)
        {
            APT_GenerationPreview.Preview preview = APT_GenerationPreview.Build(Selection.transforms);
            GameObject existingRoot = APT_HierarchyBuilder.FindSharedRoot(SceneManager.GetActiveScene(), APT_Settings.RootName);

            using (new EditorGUILayout.VerticalScope("box"))
            {
                GUILayout.Label(APT_Loc.T("AP_PT_SECTION_ACTIONS"), EditorStyles.boldLabel);
                DrawSelectionSummary(preview);
                GUILayout.Space(4f);

                if (preview.EstimatedProbeCountTotal > 0)
                {
                    string plan = string.Format(
                        APT_Loc.T("AP_PT_PLAN_FMT"),
                        Mathf.Max(1, preview.ZoneCount),
                        preview.EstimatedProbeCountTotal,
                        APT_Loc.T(preview.ResultHandlingLocKey));
                    DrawSubtleNote(plan);
                }

                if (!preview.HasAnyGeneratorEnabled)
                {
                    DrawSubtleNote(APT_Loc.T("AP_PT_PLAN_NO_FEATURES"));
                }
                else if (preview.IsHeavyLpg)
                {
                    EditorGUILayout.HelpBox(APT_Loc.T("AP_PT_PLAN_HEAVY_LPG"), MessageType.Warning);
                }

                if (!string.IsNullOrWhiteSpace(APT_ProbeGenerator.LastRunSummary))
                {
                    DrawSubtleNote(APT_ProbeGenerator.LastRunSummary);
                }

                string generateLabel = APT_Loc.T("AP_PT_ACTION_GENERATE");
                if (!APT_Settings.UsePerFeatureResultHandling)
                {
                    if (APT_Settings.ResultMode == APT_Settings.ResultUpdateMatching)
                    {
                        generateLabel = APT_Loc.T("AP_PT_ACTION_GENERATE_UPDATE");
                    }
                    else if (APT_Settings.ResultMode == APT_Settings.ResultReplaceMatching)
                    {
                        generateLabel = APT_Loc.T("AP_PT_ACTION_GENERATE_REPLACE");
                    }
                }

                using (new EditorGUI.DisabledScope(!preview.HasSelection || !preview.HasBounds || !preview.HasAnyGeneratorEnabled))
                {
                    if (GUILayout.Button(generateLabel, GUILayout.Height(30f)))
                    {
                        if (APT_ProbeGenerator.GenerateForSelection() && isStandalone && EditorWindow.focusedWindow != null)
                        {
                            EditorWindow.focusedWindow.ShowNotification(new GUIContent(APT_Loc.T("AP_PT_DONE")));
                        }
                    }
                }

                if (compact)
                {
                    using (new EditorGUI.DisabledScope(existingRoot == null))
                    {
                        if (GUILayout.Button(APT_Loc.T("AP_PT_ACTION_FOCUS_ROOT")))
                        {
                            APT_ProbeGenerator.FocusCurrentRootInActiveScene();
                        }
                    }

                    using (new EditorGUI.DisabledScope(APT_ProbeGenerator.LastCreatedObject == null))
                    {
                        if (GUILayout.Button(APT_Loc.T("AP_PT_ACTION_REVEAL_LAST")))
                        {
                            Selection.activeGameObject = APT_ProbeGenerator.LastCreatedObject;
                            EditorGUIUtility.PingObject(APT_ProbeGenerator.LastCreatedObject);
                        }
                    }
                }
                else
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        using (new EditorGUI.DisabledScope(existingRoot == null))
                        {
                            if (GUILayout.Button(APT_Loc.T("AP_PT_ACTION_FOCUS_ROOT")))
                            {
                                APT_ProbeGenerator.FocusCurrentRootInActiveScene();
                            }
                        }

                        using (new EditorGUI.DisabledScope(APT_ProbeGenerator.LastCreatedObject == null))
                        {
                            if (GUILayout.Button(APT_Loc.T("AP_PT_ACTION_REVEAL_LAST")))
                            {
                                Selection.activeGameObject = APT_ProbeGenerator.LastCreatedObject;
                                EditorGUIUtility.PingObject(APT_ProbeGenerator.LastCreatedObject);
                            }
                        }
                    }
                }
            }
        }

        private static void DrawSelectionSummary(APT_GenerationPreview.Preview preview)
        {
            if (preview == null || !preview.HasSelection)
            {
                DrawSubtleNote(APT_Loc.T("AP_PT_SELECTION_NONE"));
                return;
            }

            List<string> lines = new List<string>();
            lines.Add(string.Format(APT_Loc.T("AP_PT_SELECTION_COUNT_FMT"), preview.SelectionCount));
            lines.Add(string.Format(APT_Loc.T("AP_PT_TARGET_ROOT_FMT"), APT_Settings.RootName));

            if (preview.HasBounds)
            {
                Vector3 size = preview.CombinedSize;
                lines.Add(string.Format(APT_Loc.T("AP_PT_SELECTION_BOUNDS_FMT"), size.x.ToString("0.##"), size.y.ToString("0.##"), size.z.ToString("0.##")));
            }

            if (preview.ZoneCount > 0)
            {
                lines.Add(string.Format(APT_Loc.T("AP_PT_ZONE_COUNT_FMT"), preview.ZoneCount));
            }

            DrawSubtleNote(string.Join("  •  ", lines.ToArray()));
        }

        private static void DrawOverviewFoldout()
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                APT_Settings.FoldoutOverview = EditorGUILayout.Foldout(APT_Settings.FoldoutOverview, APT_Loc.T("AP_PT_SECTION_SUMMARY"), true);
                if (!APT_Settings.FoldoutOverview)
                {
                    return;
                }

                GUILayout.Space(4f);
                using (new EditorGUILayout.HorizontalScope())
                {
                    DrawCountTile(APT_Loc.T("AP_PT_SUMMARY_BASE_COUNT"), 3);
                    DrawCountTile(APT_Loc.T("AP_PT_SUMMARY_OPTIONAL_COUNT"), APT_OptionalFeatures.GetInstalledCount());
                }
            }
        }

        private static void DrawCountTile(string label, int value)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                GUILayout.Label(label, new GUIStyle(EditorStyles.miniLabel)
                {
                    alignment = TextAnchor.MiddleCenter,
                    wordWrap = true
                });
                GUILayout.Space(2f);
                GUILayout.Label(value.ToString(), new GUIStyle(EditorStyles.boldLabel)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 16
                });
            }
        }

        private static void DrawOptionalFeaturesFoldout()
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                APT_Settings.FoldoutOptional = EditorGUILayout.Foldout(APT_Settings.FoldoutOptional, APT_Loc.T("AP_PT_SECTION_OPTIONAL"), true);
                if (!APT_Settings.FoldoutOptional)
                {
                    return;
                }

                GUILayout.Space(4f);
                var cards = APT_OptionalFeatures.GetCards();
                for (int i = 0; i < cards.Count; i++)
                {
                    DrawOptionalCard(cards[i]);
                    if (i < cards.Count - 1)
                    {
                        GUILayout.Space(4f);
                    }
                }
            }
        }

        private static void DrawOptionalCard(APT_OptionalFeatures.FeatureCard card)
        {
            bool installed = card != null && card.IsInstalled != null && card.IsInstalled();
            MessageType type = installed ? card.StateTypeWhenInstalled : card.StateTypeWhenMissing;
            string stateKey = installed ? card.InstalledStateLocKey : card.MissingStateLocKey;

            using (new EditorGUILayout.VerticalScope("box"))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label(APT_Loc.T(card.TitleLocKey), EditorStyles.boldLabel);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(APT_Loc.T(stateKey), EditorStyles.miniBoldLabel);
                }

                GUILayout.Space(2f);
                if (type == MessageType.None)
                {
                    GUILayout.Label(APT_Loc.T(card.SummaryLocKey), EditorStyles.wordWrappedMiniLabel);
                }
                else
                {
                    EditorGUILayout.HelpBox(APT_Loc.T(card.SummaryLocKey), type);
                }
            }
        }

        private static void DrawStatusFoldout(bool isStandalone, bool compact)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                APT_Settings.FoldoutStatus = EditorGUILayout.Foldout(APT_Settings.FoldoutStatus, APT_Loc.T("AP_PT_SECTION_STATUS"), true);
                if (!APT_Settings.FoldoutStatus)
                {
                    return;
                }

                GUILayout.Space(4f);
                if (compact && !isStandalone)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        DrawStatusChip(APT_Loc.T("AP_PT_STATUS_AP"), APT_PackageDetector.HasAngelPanelCore);
                        DrawStatusChip(APT_Loc.T("AP_PT_STATUS_VRCSDK"), APT_PackageDetector.HasVrcSdkWorlds);
                        DrawStatusChip(APT_Loc.T("AP_PT_STATUS_UDON"), APT_PackageDetector.HasUdonSharp);
                    }
                }
                else
                {
                    DrawStatusLine(APT_Loc.T("AP_PT_STATUS_AP"), APT_PackageDetector.HasAngelPanelCore);
                    DrawStatusLine(APT_Loc.T("AP_PT_STATUS_VRCSDK"), APT_PackageDetector.HasVrcSdkWorlds);
                    DrawStatusLine(APT_Loc.T("AP_PT_STATUS_UDON"), APT_PackageDetector.HasUdonSharp);
                }

                GUILayout.Space(4f);
                using (new EditorGUI.DisabledScope(APT_PackageDetector.IsScanning))
                {
                    if (GUILayout.Button(APT_PackageDetector.IsScanning ? APT_Loc.T("AP_PT_STATUS_SCANNING") : APT_Loc.T("AP_PT_STATUS_SCAN")))
                    {
                        APT_PackageDetector.RefreshNow();
                    }
                }
            }
        }

        private static void DrawStatusChip(string label, bool isReady)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                GUILayout.Label(label, new GUIStyle(EditorStyles.miniLabel)
                {
                    alignment = TextAnchor.MiddleCenter,
                    wordWrap = true
                });
                GUILayout.Space(2f);
                GUILayout.Label(isReady ? APT_Loc.T("AP_PT_STATE_READY") : APT_Loc.T("AP_PT_STATE_MISSING"), new GUIStyle(EditorStyles.miniBoldLabel)
                {
                    alignment = TextAnchor.MiddleCenter
                });
            }
        }

        private static void DrawStatusLine(string label, bool isReady)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(label, GUILayout.Width(140f));
                GUILayout.Label(isReady ? APT_Loc.T("AP_PT_STATE_READY") : APT_Loc.T("AP_PT_STATE_MISSING"), EditorStyles.miniBoldLabel);
            }
        }

        private static void DrawSubtleNote(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            GUILayout.Label(text, EditorStyles.wordWrappedMiniLabel);
        }

        private static float GetHostWidth(object hostContext)
        {
            if (hostContext == null)
            {
                return 760f;
            }

            try
            {
                Type type = hostContext.GetType();
                PropertyInfo prop = type.GetProperty("ContentWidth", BindingFlags.Public | BindingFlags.Instance);
                if (prop != null && prop.PropertyType == typeof(float))
                {
                    return Mathf.Max(320f, (float)prop.GetValue(hostContext, null));
                }

                FieldInfo field = type.GetField("ContentWidth", BindingFlags.Public | BindingFlags.Instance);
                if (field != null && field.FieldType == typeof(float))
                {
                    return Mathf.Max(320f, (float)field.GetValue(hostContext));
                }
            }
            catch
            {
            }

            return 760f;
        }
    }
}
#endif
