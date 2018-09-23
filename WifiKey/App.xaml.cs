using System.Windows;
using WifiKey.Splash;


namespace WifiKey
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Splasher.ShowSplash();
            base.OnStartup(e);
        }
        
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Splasher.CloseSplash();
        }
    }
}
