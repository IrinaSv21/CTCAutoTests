using CTCAutoTests.TestModel.Tren3DApp.PageElements;

namespace CTCAutoTests.TestModel.Tren3DApp.PageObjects
{
    public class StartMenuPage
    {
        public static Button startWithoutServerButton;
        public static Button anonymousButton;
        public static Scene menuScene;
        public static Scene mainScene;

        public StartMenuPage()
        {
            menuScene = new Scene("MenuScene");
            mainScene = new Scene("MainScene");
            startWithoutServerButton = new Button("StartHostButton");
            anonymousButton = new Button("AnonymousButton");
        }
    }
}
