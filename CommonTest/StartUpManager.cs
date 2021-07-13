using NUnit.Framework;
using System.Windows;
using System.Windows.Controls;

namespace CommonTest
{
    [Apartment(System.Threading.ApartmentState.STA)]
    public class StartUpManager
    {
        public void ShowControl(Control control)
        {
            Window window = new Window();
            window.Content = control;

            Application app = new Application();
            app.Run(window);
        }

        public string GetResourcesDirectory()
        {
            return @"C:\Users\bhatt\Programming\Common\CommonTest\Resources\";
        }
    }
}
