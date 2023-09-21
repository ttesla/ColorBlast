using System.Diagnostics;

namespace Utility
{
    /// <summary>
    /// Handy log class, which logs only in Unity Editor
    /// Removes all Log calls in builds. 
    /// 
    /// This is not a service because log calls are omitted from the build.
    /// Seperate Log Service may be created if it is required.
    /// </summary>
    public class Logman
    {
        [Conditional("UNITY_EDITOR")]
        public static void Log(object msg)
        {
            UnityEngine.Debug.Log(msg);
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogWarning(object msg)
        {
            UnityEngine.Debug.LogWarning(msg);
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogError(object msg)
        {
            UnityEngine.Debug.LogError(msg);
        }
    }
}
