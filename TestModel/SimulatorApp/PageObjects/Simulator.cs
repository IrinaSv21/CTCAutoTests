using CTCAutoTests.TestModel.Steps;
using CTCAutoTests.Utilities;
using OpenQA.Selenium.Appium.Windows;

namespace CTCAutoTests.TestModel.SimulatorApp.PageObjects
{
    public class Simulator
    {
        public const string simulatorName = Environment.SIMULATOR_NAME;
        public string simulatorPath;
        public static WindowsDriver<WindowsElement> AppSession;
        public SimulatorPage simulatorPage = new SimulatorPage();
        public DiagramPage diagramPage = new DiagramPage();
        public AllObjectsPage allObjectsPage = new AllObjectsPage();
        public ObjectParametersPage objectParametersPage = new ObjectParametersPage();
        public SimulatorSteps simulatorSteps = new SimulatorSteps();

        public Simulator(string simulatorPagePath)
        {
            simulatorPath = simulatorPagePath;
            if (AppManager.appPaths.ContainsKey(Environment.SIMULATOR_NAME))
                AppManager.appPaths.Remove(Environment.SIMULATOR_NAME);

            AppManager.appPaths.Add(Environment.SIMULATOR_NAME, Environment.SIMULATOR_PATH_1);

        }
    }
}
