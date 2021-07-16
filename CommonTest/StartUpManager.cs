using NUnit.Framework;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace CommonTest
{
    [Apartment(System.Threading.ApartmentState.STA)]
    public class StartUpManager
    {
        public static string TestDirectory { get; } = @"C:\Users\bhatt\Programming\Common\CommonTest\";
        public static string ResourcesDirectory { get; } = Path.Combine(TestDirectory, "Resources");

        public void ShowControl(Control control)
        {
            Window window = new Window();
            window.Content = control;

            Application app = new Application();
            app.Run(window);
        }
    }
}
