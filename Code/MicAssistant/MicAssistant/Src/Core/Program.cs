using UnityEngine;

namespace MicAssistant
{
    public static class Program
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Main()
        {
            new GameObject("AppManager").AddComponent<AppManager>();
        }
    }
}
