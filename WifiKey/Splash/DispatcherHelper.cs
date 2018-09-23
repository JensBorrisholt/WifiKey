using System;
using System.Security.Permissions;
using System.Windows.Threading;

namespace WifiKey.Splash
{
    public static class DispatcherHelper
    {
        /// <summary>
        /// Simulate Application.DoEvents function of <see>
        ///         <cref xml:space="preserve"> System.Windows.Forms.Application</cref>
        ///     </see>
        ///     class.
        /// </summary>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrames), frame);

            try
            {
                Dispatcher.PushFrame(frame);
            }
            catch (InvalidOperationException)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        private static object ExitFrames(object frame)
        {
            ((DispatcherFrame)frame).Continue = false;
            return null;
        }
    }
}
