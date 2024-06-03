using CTCAutoTests.Utilities;


namespace CTCAutoTests.TestModel.Tren3DApp.PageElements
{
    public class Scene
    {
        private string name;
        public Scene(string value)
        {
            this.name = value;
        }

        // Ожидание загрузки сцены
        public void WaitForCurrentSceneToBe()
        {
            UnityDriver.altDriver.WaitForCurrentSceneToBe(name);
        }

    }
}
