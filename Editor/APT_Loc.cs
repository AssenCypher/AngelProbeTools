#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace AngelProbeTools.Editor
{
    public static class APT_Loc
    {
        private const string PrefLangIndex = "AngelPanel_Lang";
        private const string LegacyPrefLangIndex = "DemonPanel_Lang";

        private static readonly Dictionary<string, string[]> Entries = new Dictionary<string, string[]>(StringComparer.Ordinal)
        {
            { "AP_PT_NAME", new[] { "Angel Probe Tools", "Angel Probe Tools", "Angel Probe Tools", "Angel Probe Tools" } },
            { "AP_PT_SUMMARY", new[] {
                "Selection-based probe generator for Reflection Probe, LightProbeGroup, bounds expansion, zone split, and optional trigger scaffold.",
                "基于选区的 Probe 生成工具，包含 Reflection Probe、LightProbeGroup、Bounds 外扩、Zone 拆分，以及可选 Trigger Scaffold。",
                "基於選取區的 Probe 生成工具，包含 Reflection Probe、LightProbeGroup、Bounds 外擴、Zone 拆分，以及可選 Trigger Scaffold。",
                "Reflection Probe・LightProbeGroup・Bounds 拡張・Zone 分割・任意 Trigger Scaffold に対応する選択ベースの Probe 生成ツールです。"
            } },
            { "AP_PT_WINDOW_TITLE", new[] { "Angel Probe Tools", "Angel Probe Tools", "Angel Probe Tools", "Angel Probe Tools" } },

            { "AP_PT_SECTION_SUMMARY", new[] { "Overview", "概览", "概覽", "Overview" } },
            { "AP_PT_SECTION_GEN", new[] { "Generation", "生成设置", "生成設定", "Generation" } },
            { "AP_PT_SECTION_ACTIONS", new[] { "Actions", "操作", "操作", "Actions" } },
            { "AP_PT_SECTION_STATUS", new[] { "Environment", "环境状态", "環境狀態", "Environment" } },
            { "AP_PT_SECTION_AUTO", new[] { "Auto Split", "自动拆分", "自動拆分", "Auto Split" } },
            { "AP_PT_SECTION_OPTIONAL", new[] { "Extensions", "扩展", "擴充", "Extensions" } },

            { "AP_PT_SUMMARY_BASE_COUNT", new[] { "Base generators", "基础生成器", "基礎生成器", "Base generators" } },
            { "AP_PT_SUMMARY_OPTIONAL_COUNT", new[] { "Detected extensions", "已检测扩展", "已偵測擴充", "Detected extensions" } },

            { "AP_PT_ROOT_NAME", new[] { "Root Name", "根名称", "根名稱", "Root Name" } },
            { "AP_PT_ROOT_NAME_NOTE", new[] {
                "Use different root names to separate floors, rooms, or areas.",
                "可通过不同根名称区分楼层、房间或区域。",
                "可透過不同根名稱區分樓層、房間或區域。",
                "ルート名を分けることで、階層・部屋・エリアを分離できます。"
            } },

            { "AP_PT_ACTION_USE_SELECTION_ROOT", new[] { "Use Selection Name", "使用选中名称", "使用選取名稱", "Use Selection Name" } },
            { "AP_PT_ACTION_RESET_ROOT", new[] { "Reset", "重置", "重置", "Reset" } },
            { "AP_PT_SELECTION_NONE", new[] { "No selection. Select one or more objects to preview the target range.", "当前未选中对象，请先选择一个或多个对象。", "目前未選取物件，請先選擇一個或多個物件。", "未選択です。対象範囲を確認するには 1 つ以上選択してください。" } },
            { "AP_PT_SELECTION_COUNT_FMT", new[] { "Selected: {0}", "已选中：{0}", "已選取：{0}", "Selected: {0}" } },
            { "AP_PT_TARGET_ROOT_FMT", new[] { "Root: {0}", "目标根：{0}", "目標根：{0}", "Root: {0}" } },
            { "AP_PT_SELECTION_BOUNDS_FMT", new[] { "Bounds: {0} × {1} × {2}", "范围：{0} × {1} × {2}", "範圍：{0} × {1} × {2}", "Bounds: {0} × {1} × {2}" } },

            { "AP_PT_ZONE_COUNT_FMT", new[] { "Zones: {0}", "Zone 数：{0}", "Zone 數：{0}", "Zones: {0}" } },
            { "AP_PT_PLAN_FMT", new[] { "Plan: {0} zone(s) • estimated LPG probes {1} • mode {2}", "计划：{0} 个 Zone • 预计 LPG 探针 {1} 个 • 模式 {2}", "計畫：{0} 個 Zone • 預計 LPG 探針 {1} 個 • 模式 {2}", "Plan: {0} zone(s) • estimated LPG probes {1} • mode {2}" } },
            { "AP_PT_PLAN_HEAVY_LPG", new[] {
                "This plan is relatively heavy. Consider a larger LPG step or fewer zones before final bake.",
                "这次生成量偏大，正式烘焙前建议适当增大 LPG 步长或减少 Zone 数。",
                "這次生成量偏大，正式烘焙前建議適度增大 LPG 步長或減少 Zone 數。",
                "今回の生成量はやや重めです。最終ベイク前に LPG ステップを広げるか Zone 数を減らしてください。"
            } },
            { "AP_PT_PLAN_NO_FEATURES", new[] {
                "No generator is enabled right now.",
                "当前没有启用任何生成器。",
                "目前沒有啟用任何生成器。",
                "現在有効なジェネレーターがありません。"
            } },
            { "AP_PT_LAST_RUN_FMT", new[] {
                "Last run • root {0} • zones {1} • created {2} • updated {3} • replaced {4} • LPG probes {5} • mode {6}",
                "上次结果 • 根 {0} • Zone {1} • 新建 {2} • 更新 {3} • 替换 {4} • LPG 探针 {5} • 模式 {6}",
                "上次結果 • 根 {0} • Zone {1} • 新建 {2} • 更新 {3} • 替換 {4} • LPG 探針 {5} • 模式 {6}",
                "前回結果 • ルート {0} • Zone {1} • 新規 {2} • 更新 {3} • 置換 {4} • LPG プローブ {5} • モード {6}"
            } },

            { "AP_PT_HIERARCHY_MODE", new[] { "Hierarchy Mode", "层级模式", "層級模式", "Hierarchy Mode" } },
            { "AP_PT_HIERARCHY_SHARED", new[] { "Shared named root", "共享命名根", "共享命名根", "Shared named root" } },
            { "AP_PT_HIERARCHY_LEGACY", new[] { "Per-run root", "每次生成独立根", "每次生成獨立根", "Per-run root" } },

            { "AP_PT_ZONE_MODE", new[] { "Zone Mode", "Zone 模式", "Zone 模式", "Zone Mode" } },
            { "AP_PT_ZONE_COMBINED", new[] { "Single combined bounds", "合并为单一 Bounds", "合併為單一 Bounds", "Single combined bounds" } },
            { "AP_PT_ZONE_PER_ROOT", new[] { "Per selected root", "按选中根分别生成", "按選取根分別生成", "Per selected root" } },
            { "AP_PT_ZONE_AUTO_SPLIT", new[] { "Experimental auto split", "实验性自动拆分", "實驗性自動拆分", "Experimental auto split" } },

            { "AP_PT_RESULT_MODE", new[] { "Result Handling", "结果处理", "結果處理", "Result Handling" } },
            { "AP_PT_RESULT_CREATE", new[] { "Always create new", "始终新建", "始終新建", "Always create new" } },
            { "AP_PT_RESULT_REPLACE", new[] { "Replace matching names", "替换同名结果", "替換同名結果", "Replace matching names" } },
            { "AP_PT_RESULT_UPDATE", new[] { "Update matching names", "更新同名结果", "更新同名結果", "Update matching names" } },
            { "AP_PT_RESULT_NOTE", new[] {
                "Update mode keeps the hierarchy cleaner when you regenerate the same area.",
                "更新模式更适合反复迭代同一区域，层级会更干净。",
                "更新模式更適合反覆迭代同一區域，層級會更乾淨。",
                "同じエリアを何度も作り直す場合は Update が最も整理しやすいです。"
            } },

            { "AP_PT_RESULT_PER_FEATURE", new[] {
                "Use separate handling per generator",
                "为各生成器单独设置结果处理",
                "為各生成器單獨設定結果處理",
                "ジェネレーターごとに結果処理を分ける"
            } },
            { "AP_PT_RESULT_PER_FEATURE_NOTE", new[] {
                "Useful when you want LPG to update cleanly but keep Reflection or Trigger behavior different.",
                "适合需要让 LPG 保持干净更新、同时让 Reflection 或 Trigger 走不同策略的情况。",
                "適合需要讓 LPG 保持乾淨更新、同時讓 Reflection 或 Trigger 採用不同策略的情況。",
                "LPG はきれいに更新しつつ、Reflection や Trigger は別挙動にしたい場合に便利です。"
            } },
            { "AP_PT_RESULT_REFLECTION", new[] { "Reflection Handling", "Reflection 处理", "Reflection 處理", "Reflection Handling" } },
            { "AP_PT_RESULT_LPG", new[] { "LPG Handling", "LPG 处理", "LPG 處理", "LPG Handling" } },
            { "AP_PT_RESULT_TRIGGER", new[] { "Trigger Handling", "Trigger 处理", "Trigger 處理", "Trigger Handling" } },
            { "AP_PT_RESULT_MIXED", new[] { "Per feature", "按生成器分别处理", "按生成器分別處理", "Per feature" } },

            { "AP_PT_BOUNDS_EXPAND", new[] { "Bounds Expand (%)", "Bounds 外扩 (%)", "Bounds 外擴 (%)", "Bounds Expand (%)" } },
            { "AP_PT_GRID_STEP", new[] { "LPG Grid Step", "LPG 网格步长", "LPG 網格步長", "LPG Grid Step" } },
            { "AP_PT_CREATE_REFLECTION", new[] { "Create Reflection Probe", "创建 Reflection Probe", "建立 Reflection Probe", "Create Reflection Probe" } },
            { "AP_PT_CREATE_LPG", new[] { "Create LightProbeGroup", "创建 LightProbeGroup", "建立 LightProbeGroup", "Create LightProbeGroup" } },
            { "AP_PT_CREATE_TRIGGER", new[] { "Create Box Trigger Scaffold", "创建 Box Trigger Scaffold", "建立 Box Trigger Scaffold", "Create Box Trigger Scaffold" } },
            { "AP_PT_TRIGGER_NOTE", new[] {
                "Creates only BoxCollider and Rigidbody. No runtime logic is attached.",
                "这里只会创建 BoxCollider 和 Rigidbody，不会挂运行时逻辑。",
                "這裡只會建立 BoxCollider 和 Rigidbody，不會掛執行時邏輯。",
                "BoxCollider と Rigidbody のみを生成し、ランタイムロジックは付与しません。"
            } },
            { "AP_PT_LPG_NOTE", new[] {
                "LPG samples are anchored to bounds edges for cleaner coverage.",
                "LPG 采样会锚定到 Bounds 边缘，让探针网格覆盖更干净。",
                "LPG 採樣會錨定到 Bounds 邊緣，讓探針網格覆蓋更乾淨。",
                "LPG サンプルは Bounds の端に固定され、より整ったカバーになります。"
            } },

            { "AP_PT_AUTO_NOTE", new[] {
                "Auto split is heuristic. Check the generated zones before final use.",
                "自动拆分是启发式结果，正式使用前请检查生成的 Zone。",
                "自動拆分是啟發式結果，正式使用前請檢查生成的 Zone。",
                "自動分割はヒューリスティックです。実運用前に生成 Zone を確認してください。"
            } },
            { "AP_PT_AUTO_MIN_SIZE", new[] { "Min Candidate Size", "最小候选尺寸", "最小候選尺寸", "Min Candidate Size" } },
            { "AP_PT_AUTO_SPLIT_GAP", new[] { "Split Gap Threshold", "拆分间隙阈值", "拆分間隙閾值", "Split Gap Threshold" } },
            { "AP_PT_AUTO_PADDING", new[] { "Extra Padding", "额外 Padding", "額外 Padding", "Extra Padding" } },
            { "AP_PT_AUTO_MAX_COUNT", new[] { "Max Zone Count", "最大 Zone 数", "最大 Zone 數", "Max Zone Count" } },
            { "AP_PT_AUTO_XZ_ONLY", new[] { "Prefer XZ splitting", "优先按 XZ 拆分", "優先按 XZ 拆分", "Prefer XZ splitting" } },

            { "AP_PT_ACTION_GENERATE", new[] { "Generate For Selection", "为当前选中生成", "為目前選取生成", "Generate For Selection" } },
            { "AP_PT_ACTION_GENERATE_REPLACE", new[] { "Replace For Selection", "替换当前选中结果", "替換目前選取結果", "Replace For Selection" } },
            { "AP_PT_ACTION_GENERATE_UPDATE", new[] { "Generate / Update Selection", "为当前选中生成或更新", "為目前選取生成或更新", "Generate / Update Selection" } },
            { "AP_PT_ACTION_FOCUS_ROOT", new[] { "Focus Current Root", "定位当前根", "定位目前根", "Focus Current Root" } },
            { "AP_PT_ACTION_REVEAL_LAST", new[] { "Ping Last Created", "定位上次创建对象", "定位上次建立物件", "Ping Last Created" } },
            { "AP_PT_ACTION_OPEN_PACKAGE_ROOT", new[] { "Open APT Root", "打开 APT 目录", "打開 APT 目錄", "APT ルートを開く" } },

            { "AP_PT_OPT_DLP_NAME", new[] { "Lighting Tools / DLP", "Lighting Tools / DLP", "Lighting Tools / DLP", "Lighting Tools / DLP" } },
            { "AP_PT_OPT_DLP_SUMMARY", new[] {
                "Lighting Tools",
                "Lighting Tools",
                "Lighting Tools",
                "Lighting Tools"
            } },
            { "AP_PT_OPT_MLP_NAME", new[] { "Magic Light Probes", "Magic Light Probes", "Magic Light Probes", "Magic Light Probes" } },
            { "AP_PT_OPT_MLP_SUMMARY", new[] {
                "Lighting Tools",
                "Lighting Tools",
                "Lighting Tools",
                "Lighting Tools"
            } },
            { "AP_PT_OPT_VRCLV_NAME", new[] { "VRCLightVolumes", "VRCLightVolumes", "VRCLightVolumes", "VRCLightVolumes" } },
            { "AP_PT_OPT_VRCLV_SUMMARY", new[] {
                "Lighting Tools",
                "Lighting Tools",
                "Lighting Tools",
                "Lighting Tools"
            } },
            { "AP_PT_OPT_STATE_INSTALLED", new[] { "Installed", "已安装", "已安裝", "Installed" } },
            { "AP_PT_OPT_STATE_RESERVED", new[] { "Waiting", "待接入", "待接入", "Waiting" } },
            { "AP_PT_OPT_STATE_PRESENT_ONLY", new[] { "Detected", "已检测到", "已偵測到", "Detected" } },
            { "AP_PT_OPT_STATE_NOT_PRESENT", new[] { "Not detected", "未检测到", "未偵測到", "Not detected" } },

            { "AP_PT_STATUS_AP", new[] { "AngelPanel Core", "AngelPanel Core", "AngelPanel Core", "AngelPanel Core" } },
            { "AP_PT_STATUS_VRCSDK", new[] { "VRCSDK Worlds", "VRCSDK Worlds", "VRCSDK Worlds", "VRCSDK Worlds" } },
            { "AP_PT_STATUS_UDON", new[] { "UdonSharp", "UdonSharp", "UdonSharp", "UdonSharp" } },
            { "AP_PT_STATUS_SCAN", new[] { "Refresh Scan", "刷新检测", "重新偵測", "Refresh Scan" } },
            { "AP_PT_STATUS_SCANNING", new[] { "Scanning loaded assemblies…", "正在扫描已加载程序集……", "正在掃描已載入組件……", "読み込まれたアセンブリを走査中…" } },
            { "AP_PT_STATE_READY", new[] { "Ready", "已检测到", "已偵測到", "Ready" } },
            { "AP_PT_STATE_MISSING", new[] { "Missing", "未检测到", "未偵測到", "Missing" } },
            { "AP_PT_ACTION_OPEN_STANDALONE", new[] { "Open Standalone Window", "打开独立窗口", "打開獨立視窗", "独立ウィンドウを開く" } },
            { "AP_PT_ACTION_OPEN_IN_AP", new[] { "Open in AngelPanel", "在 AngelPanel 中打开", "在 AngelPanel 中開啟", "AngelPanel で開く" } },

            { "AP_PT_DIALOG_NO_SELECTION", new[] { "No Selection", "未选中对象", "未選取物件", "No Selection" } },
            { "AP_PT_DIALOG_NO_SELECTION_BODY", new[] { "Select one or more objects first.", "请先选中一个或多个对象。", "請先選取一個或多個物件。", "先に 1 つ以上のオブジェクトを選択してください。" } },
            { "AP_PT_DIALOG_NO_BOUNDS", new[] { "No Bounds", "没有可用 Bounds", "沒有可用 Bounds", "No Bounds" } },
            { "AP_PT_DIALOG_NO_BOUNDS_BODY", new[] { "No renderer or collider bounds were found under the current selection.", "当前选中下没有找到可用的 renderer 或 collider bounds。", "目前選取下沒有找到可用的 renderer 或 collider bounds。", "現在の選択範囲から renderer または collider の bounds を見つけられませんでした。" } },
            { "AP_PT_DIALOG_NOTHING_ENABLED", new[] { "Nothing Enabled", "没有启用生成器", "沒有啟用生成器", "Nothing Enabled" } },
            { "AP_PT_DIALOG_NOTHING_ENABLED_BODY", new[] { "Enable at least one generator before you run APT.", "请至少启用一个生成器后再执行 APT。", "請至少啟用一個生成器後再執行 APT。", "APT を実行する前に少なくとも 1 つのジェネレーターを有効化してください。" } },
            { "AP_PT_OK", new[] { "OK", "确定", "確定", "OK" } },
            { "AP_PT_DONE", new[] { "Generation finished.", "生成完成。", "生成完成。", "Generation finished." } },
        };

        public static IEnumerable<KeyValuePair<string, string[]>> AllEntries => Entries;

        public static int LangIndex
        {
            get
            {
                int index = 0;
                if (EditorPrefs.HasKey(PrefLangIndex))
                {
                    index = EditorPrefs.GetInt(PrefLangIndex, 0);
                }
                else if (EditorPrefs.HasKey(LegacyPrefLangIndex))
                {
                    index = EditorPrefs.GetInt(LegacyPrefLangIndex, 0);
                }

                if (index < 0) index = 0;
                if (index > 3) index = 3;
                return index;
            }
        }

        public static void Ensure()
        {
            APT_APBridge.EnsureRegistered();
        }

        public static string T(string key)
        {
            Ensure();

            if (string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }

            if (TryResolveFromAngelPanel(key, out string external) && !string.IsNullOrWhiteSpace(external))
            {
                return external;
            }

            return ResolveLocal(key);
        }

        private static string ResolveLocal(string key)
        {
            if (!Entries.TryGetValue(key.Trim(), out string[] values) || values == null || values.Length == 0)
            {
                return key;
            }

            int index = LangIndex;
            if (index >= 0 && index < values.Length && !string.IsNullOrWhiteSpace(values[index]))
            {
                return values[index];
            }

            return !string.IsNullOrWhiteSpace(values[0]) ? values[0] : key;
        }

        private static bool TryResolveFromAngelPanel(string key, out string value)
        {
            value = string.Empty;

            Type apLocType = FindType("AngelPanel.Editor.AP_Loc");
            if (apLocType == null)
            {
                return false;
            }

            MethodInfo method = apLocType.GetMethod("T", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string) }, null);
            if (method == null)
            {
                return false;
            }

            try
            {
                object result = method.Invoke(null, new object[] { key });
                if (result is string text && !string.IsNullOrWhiteSpace(text) && !string.Equals(text, key, StringComparison.Ordinal))
                {
                    value = text;
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        private static Type FindType(string fullName)
        {
            Type type = Type.GetType(fullName + ", AngelPanel.Core.Editor");
            if (type != null)
            {
                return type;
            }

            AppDomain domain = AppDomain.CurrentDomain;
            if (domain == null)
            {
                return null;
            }

            Assembly[] assemblies = domain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                Assembly assembly = assemblies[i];
                if (assembly == null)
                {
                    continue;
                }

                try
                {
                    type = assembly.GetType(fullName, false);
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
