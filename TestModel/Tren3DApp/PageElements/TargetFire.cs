//string objName = "sand_PLK_025"; //справа впереди
//string objName = "ventilation_008"; //слева сзади
//string objName = "WatchmanHouse_001"; //слева впереди
//string objName = "fire_extinguisher_stand_007"; //справа на горизонте

using AltTester.AltTesterUnitySDK.Driver;
using CTCAutoTests.Utilities;
using System.Collections.Generic;

namespace CTCAutoTests.TestModel.Tren3DApp.PageElements
{
    public class TargetFire
    {
        private string name;
        public TargetFire(string name)
        {
            this.name = name;
        }

        // Метод для проверки появилась ли рамка у объекта
        public bool IsObjectHighlighted()
        {
            string outlineObjectName = name + "Outline";
            return UnityDriver.altDriver.FindObjects(By.NAME, outlineObjectName).Count != 0;
        }

        // Метод для получения текстового значения компонента объекта
        public string GetObjectTextValue(AltObject mainInfo)
        {
            const string componentName = "TMPro.TextMeshProUGUI";
            const string propertyName = "text";
            const string assemblyName = "Unity.TextMeshPro";
            return mainInfo.GetComponentProperty<string>(componentName, propertyName, assemblyName);
        }
    }
}
