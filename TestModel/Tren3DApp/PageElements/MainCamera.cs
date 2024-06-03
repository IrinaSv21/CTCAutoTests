using AltTester.AltTesterUnitySDK.Driver;
using CTCAutoTests.Utilities;
using System.Numerics;

namespace CTCAutoTests.TestModel.Tren3DApp.PageElements
{
    public class MainCamera : Object
    {
        public MainCamera(string name) : base(name)
        {
            this.name = name;
        }

        // Метод для получения позиции камеры
        public AltObject GetCameraPosition() =>
            UnityDriver.altDriver.FindObject(By.NAME, name);

        // Метод для получения угла поворота камеры, приведенного к диапазону от -180 до 180 градусов
        public double GetAdjustedCameraAngle(AltObject cameraPoint)
        {
            // Получаем угол поворота камеры из свойства eulerAngles
            Vector3 initialRotation = GetCameraRotation(cameraPoint);

            // Приводим угол к нужному диапазону
            var adjustedCameraAngle = Tren3DBaseMethods.NormalizeAngle(initialRotation);

            return adjustedCameraAngle;
        }



        // Метод для получения угла поворота камеры
        public Vector3 GetCameraRotation(AltObject cameraPoint)
        {
            const string componentName = "UnityEngine.Transform";
            const string methodName = "eulerAngles";
            const string assemblyName = "UnityEngine.CoreModule";
            return cameraPoint.GetComponentProperty<Vector3>(componentName, methodName, assemblyName);
        }


    }
}
