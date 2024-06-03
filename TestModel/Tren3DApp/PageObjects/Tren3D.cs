using CTCAutoTests.Tren3D.Steps;

namespace CTCAutoTests.TestModel.Tren3DApp.PageObjects
{
    public class Tren3D
    {
        public StartMenuPage startMenuPage = new StartMenuPage();
        public Tren3DSteps tren3DSteps;
        public MainScenePage mainScenePage;

        public Tren3D(string target)
        {
            mainScenePage = new MainScenePage(target);
            tren3DSteps = new Tren3DSteps();
        }
    }
 }

