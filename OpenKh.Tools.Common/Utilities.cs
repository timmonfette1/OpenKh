using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace OpenKh.Tools.Common
{
    public static class Utilities
    {
        public static Window GetCurrentWindow() =>
            Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);

        public static string GetApplicationName()
        {
            var assembly = Assembly.GetEntryAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.ProductName;
        }

        public static void ShowError(string message, string title = "Error") =>
            MessageBox.Show(GetCurrentWindow(), message, title, MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
