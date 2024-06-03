using CTCAutoTests.TestModel.Tren3DApp.PageElements;

namespace CTCAutoTests.TestModel.Tren3DApp.PageObjects
{
    public class MainScenePage
    {
        public static Operator @operator;
        public static Target target;
        public static MainCamera mainCamera;
        public static Object mainInfo;

        public MainScenePage(string name)
        {
            @operator = new Operator("Operator03(Clone)");
            target = new Target(name);
            mainCamera = new MainCamera("MainCamera");
            mainInfo = new Object("MainInfo");            
        }

    }
}
