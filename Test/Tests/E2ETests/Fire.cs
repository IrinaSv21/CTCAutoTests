using CTCAutoTests.TestModel.SimulatorApp.PageObjects;
using CTCAutoTests.TestModel.Tren3DApp.PageObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using static CTCAutoTests.Utilities.Environment;

namespace E2ETests
{
    [TestClass]
    public class Fire
    {
        private static Tren3D tren3D;
        private static Simulator simulator1;
        private static string target = "P-306B";


        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            tren3D = new Tren3D(target);
            simulator1 = new Simulator(SIMULATOR_PATH_1);
        }

        [TestMethod]
        public void TeleportAndStartFire()
        {
            simulator1.simulatorSteps
                .StartSimulator()
                .ModelInitialization()
                .StartModel();

            tren3D.tren3DSteps
                .StartTren3D()
                .LoginAsAnonymousWithoutServer()
                .TeleportToTheObject();

            simulator1.simulatorSteps
                .GoToSimulator()
                .OpenContextMenu()
                .SelectFindObjectInContextMenu()
                .EnterObjNameAndClickExclamButtonOnAllObjectsPage()
                .DoubleClickOnSearchResultOnAllObjectsPage()
                .ClickHighlightedElementOnDiagram()
                .GoToFailureFolderAndSelectEngineFailureParameter()
                .ClickPlus10PercentButton()
                .ClickPlus10PercentButton();

            tren3D.tren3DSteps
                .GoToTren3D()
                .CheckThatTheObjectIsOnFire();

            simulator1.simulatorSteps
                .GoToSimulator()
                .GoToFailureFolderAndSelectRestorationOfPerformanceParameter()
                .ClickMinus10PercentButton()
                .ClickMinus10PercentButton();
            Thread.Sleep(1000);

            tren3D.tren3DSteps
                .GoToTren3D()
                .CheckThatTheObjectIsNotOnFire();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            //TearDown();
        }
    }
}
