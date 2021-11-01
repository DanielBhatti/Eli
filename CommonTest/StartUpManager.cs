using NUnit.Framework;
using System.IO;

namespace CommonTest
{
    [Apartment(System.Threading.ApartmentState.STA)]
    public class StartUpManager
    {
        public static string TestDirectory { get; } = @"C:\Users\bhatt\Repositories\Common\CommonTest\";
        public static string ResourcesDirectory { get; } = Path.Combine(TestDirectory, "Resources");
    }
}
