using CTCAutoTests.TestModel.SimulatorApp.PageObjects;
using CTCAutoTests.TestModel.Tren3DApp.PageObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static CTCAutoTests.Utilities.Environment;

namespace E2ETests
{
    [TestClass]
    public class Smoke
    {
        private static Tren3D tren3D;
        private static Simulator simulator1;
        private static string target = "fv102";

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            tren3D = new Tren3D(target);
            simulator1 = new Simulator(SIMULATOR_PATH_1);
        }

        [TestMethod]
        public void SimulatorAndTren3DSmoke()
        {
            simulator1.simulatorSteps
                .StartSimulator()
                .ModelInitialization()
                .CheckThatModelWasSuccessfullyInitialized()
                .StartModel();
            //Добавить проверку что модель стартовала - кнопка стала неактивной

            tren3D.tren3DSteps
                .StartTren3D()
                .LoginAsAnonymousWithoutServer();

            tren3D.tren3DSteps
                .ApproachTheObject()
                .FocusAndVerifyObjectHighlighting()
                .GetHighlightedObjectValue(out string objectName, out double objectValue);

            simulator1.simulatorSteps
                .GoToSimulator()
                .OpenContextMenu()
                .SelectFindObjectInContextMenu()
                .EnterObjNameAndClickExclamButtonOnAllObjectsPage(objectName)
                .DoubleClickOnSearchResultOnAllObjectsPage()
                .ClickHighlightedElementOnDiagram()
                .CheckIfParametersWindowContainsObjectName(objectName)
                .GoToField_GatesFolderAndSelectExitParameter()
                .RetrieveField_Gates_ExitValue(out double parametrValue);

            Assert.AreEqual(objectValue, parametrValue);

            tren3D.tren3DSteps
                .GoToTren3D()
                .MouseScrollDown()
                .GetHighlightedObjectValue(out string objectName2, out double objectValue2);

            simulator1.simulatorSteps
                .GoToSimulatorDiagram()
                .RetrieveField_Gates_ExitValue(out double parametrValue2);

            Assert.AreEqual(Math.Floor(objectValue2), Math.Floor(parametrValue2));

            simulator1.simulatorSteps
                .SetField_Gates_ExitValue(25)
                .RetrieveField_Gates_ExitValue(out double parametrValue3);

            Assert.AreEqual(25.000, Math.Truncate(parametrValue3));

            tren3D.tren3DSteps
                .GoToTren3D()
                .GetHighlightedObjectValue(out string objectName3, out double objectValue3);

            Assert.AreEqual(objectValue3, parametrValue3);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            //TearDown();
        }
    }
}
