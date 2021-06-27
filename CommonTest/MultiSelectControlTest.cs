using Common.Wpf;
using NUnit.Framework;

namespace CommonTest
{
    [TestFixture]
    public class MultiSelectControlTest
    {
        StartUpManager csu;

        [SetUp]
        public void Setup()
        {
            csu = new StartUpManager();
        }

        //[Test, Apartment(System.Threading.ApartmentState.STA)]
        public void DisplayItems()
        {
            MultiSelectControl msc = new MultiSelectControl();
            msc.Items.Add(new MultiSelectControlItem("test1"));
            msc.Items.Add(new MultiSelectControlItem("test2", true));
            msc.Items.Add(new MultiSelectControlItem("test3"));
            msc.Items.Add(new MultiSelectControlItem("test4"));
            csu.ShowControl(msc);
            Assert.Pass();
        }

        //[Test, Apartment(System.Threading.ApartmentState.STA)]
        public void CheckChangedBindingWorks()
        {
            MultiSelectControl msc = new MultiSelectControl();
            for (int i = 0; i < 5; i++)
            {
                bool b = false;
                if (i % 2 == 0) b = true;
                msc.Items.Add(new MultiSelectControlItem(i.ToString(), b, new ShowMessageCommand()));
            }
            csu.ShowControl(msc);
            Assert.Pass();
        }

        private class ShowMessageCommand : Command
        {
            public override void Execute(object parameter)
            {
                string s = "TEST";
                if (parameter is not null) s = parameter.ToString();
                System.Windows.MessageBox.Show(s);
            }
        }
    }
}
