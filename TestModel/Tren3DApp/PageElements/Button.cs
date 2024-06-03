using AltTester.AltTesterUnitySDK.Driver;
using CTCAutoTests.Utilities;

namespace CTCAutoTests.TestModel.Tren3DApp.PageElements
{
    public class Button
    {
         private string name;

        public Button(string name)
        {            
            this.name = name;
        }

        // Ожидание появления кнопки
        public void WaitForAppear()
        {
            UnityDriver.altDriver.WaitForObject(By.NAME, name); 
        }

        // Нажатие кнопки
        public void Click()
        {
            UnityDriver.altDriver.FindObject(By.NAME, name).Tap();
        }
    }
}
