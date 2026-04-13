#if UNITY_EDITOR
namespace AngelProbeTools.Editor
{
    public static class APT_HostPage
    {
        public static void DrawEmbedded(object hostContext)
        {
            APT_UI.DrawEmbeddedHostPage(hostContext);
        }
    }
}
#endif
