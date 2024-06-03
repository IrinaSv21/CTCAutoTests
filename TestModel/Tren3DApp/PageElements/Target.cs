//string objName = "sand_PLK_025"; //справа впереди
//string objName = "ventilation_008"; //слева сзади
//string objName = "WatchmanHouse_001"; //слева впереди
//string objName = "fire_extinguisher_stand_007"; //справа на горизонте

using AltTester.AltTesterUnitySDK.Driver;
using CTCAutoTests.TestModel.Tren3DApp.PageObjects;
using CTCAutoTests.Utilities;
using System;
using System.Collections.Generic;

namespace CTCAutoTests.TestModel.Tren3DApp.PageElements
{
    public class Target : Object
    {
        static Dictionary<string, string> TargetNameMatching = new Dictionary<string, string>
        {
            { "fv102", "ValveFWManipulator_01_0220_014" },
            { "P-306B", "P-306B" }
        };
        public string UnityObjectName;
        public static TargetFire targetFire;
        public bool isOnFire;
        public Target(string name) : base(TargetNameMatching[name])
        {
            UnityObjectName = TargetNameMatching[name];
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

        //Метод для проверки, горит ли объект
        public bool IsObjectOnFire()
        {
            try
            {
                UnityDriver.altDriver.FindObject(By.PATH, $"//{MainScenePage.target.name}/FlameStream(Clone)");
                Console.WriteLine($"Объект горит");
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine($"Объект не горит");
                return false;
            }
        }

    }
}
