using OpenQA.Selenium.Appium.Windows;
using CTCAutoTests.TestModel.SimulatorApp.PageElements;

namespace CTCAutoTests.TestModel.SimulatorApp.PageObjects
{
    public class DiagramPage
    {
        public static WindowsDriver<WindowsElement> DiagramSession;
        public static Diagram diagram;

        public DiagramPage()
        {
            diagram = new Diagram();
        }
    }
}
