using Microsoft.VisualStudio.TestTools.UnitTesting;
using CTCAutoTests.Utilities;
using CTCAutoTests.TestModel.SimulatorApp.PageObjects;

namespace SimulatorTests
{
    [TestClass]
    public class TesseractTests
    {
        private static Simulator simulator1;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {            
            simulator1 = new Simulator(Environment.SIMULATOR_PATH_1);
            simulator1.simulatorSteps
                .StartSimulator();
        }

        [TestMethod]
        public void CountOfStepsAfterSetTime()
        {
            double timeMin = 0.5;
            simulator1.simulatorSteps
                .ModelInitialization()
                .StartModel();
                // Добавить проверку что модель стартовала - кнопка стала неактивной
            SimulatorBaseMethods
                .WaitTimeMin(timeMin);
            simulator1.simulatorSteps
                .GetStepNumber();
        }

        [TestMethod]
        public void GetSteps()
        {
            simulator1.simulatorSteps.ModelInitialization();
            SimulatorPage
                .startButton.WaitIsActive();
            simulator1.simulatorSteps
                .GetEveryStepCount(10, 1);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            //TearDown();
        }
    }
}
