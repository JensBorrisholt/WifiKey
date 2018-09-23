using System.Windows;

namespace WifiKey.Splash
{
    /// <summary>
    /// Helper to show or close given splash window
    /// </summary>
    public static class Splasher
    {
        /// <summary>
        /// Get or set the splash screen window
        /// </summary>
        public static Window Splash { get; private set; }

        /// <summary>
        /// Show splash screen
        /// </summary>
        public static void ShowSplash()
        {
            if (Splash == null)
            {
                Splash = new SplashScreen();
                Splash.Closed += (sender, e) => Splash = null;
            }

            Splash?.Show();
        }

        /// <summary>
        /// Close splash screen
        /// </summary>
        public static void CloseSplash() => Splash?.Close();
    }
}
