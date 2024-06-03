using AltTester.AltTesterUnitySDK.Driver;
using CTCAutoTests.Utilities;
using System;
using System.Drawing;
using CTCAutoTests.TestModel.Tren3DApp.PageObjects;
using System.Numerics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTCAutoTests.TestModel.Tren3DApp.PageElements
{
    public class Operator : Object
    {
        int blindArea = 3;
        AltKeyCode[] keysShiftUp = { AltKeyCode.UpArrow, AltKeyCode.RightShift };
        AltKeyCode[] keysUp = { AltKeyCode.UpArrow };
        AltKeyCode[] keysRight = { AltKeyCode.RightArrow };
        AltKeyCode[] keysShiftRight = { AltKeyCode.RightArrow, AltKeyCode.RightShift };
        AltKeyCode[] moveKeys;

        public Operator(string name) : base(name)
        {
            this.name = name;
        }

        /// <summary>
        /// Метод для поворота оператора к объекту
        /// </summary>
        /// <param name="objName">Имя объекта</param>
        /// <param name="offset">Смещение фокуса относительно объекта (=150)</param>
        /// <returns></returns>
        public Operator TurnToObject(Target objName, int offset = 0)
        {
            Point screenCenter = Tren3DBaseMethods.GetScreenCenter(); // Получаем координаты центра экрана

            // Находим целевой объект
            var target = MainScenePage.target.FindObjectByName();
            if (target == null)
                return this;

            // Вычисляем шаг вращения
            var step = Tren3DBaseMethods.CalculateRotationStep(screenCenter);

            // Находим камеру
            var cameraPoint = MainScenePage.mainCamera.FindObjectByName();
            if (cameraPoint == null)
                return this;

            // Получаем угол поворота камеры и угол объекта к камере
            var adjustedCameraAngle = MainScenePage.mainCamera.GetAdjustedCameraAngle(cameraPoint);
            var targetToCameraAngle = Tren3DBaseMethods.CalculateTargetToCameraAngle(target, cameraPoint);

            // Корректируем шаг в зависимости от угла поворота камеры и угла объекта к камере
            Tren3DBaseMethods.AdjustStepBasedOnCameraAngle(ref step, adjustedCameraAngle, targetToCameraAngle);

            // Поворачиваем камеру, пока объект не окажется в фокусе
            Tren3DBaseMethods.RotateCameraToFocus(target, step, screenCenter, offset, objName.name);
            return this;
        }

        // Метод для движения к объекту после поворота со смещением
        public Operator MoveToObjectWithRightOffset(Target objName) =>
            MoveToObject(objName, true);

        // Метод для движения к прямо объекту
        public Operator MoveToObjectDirectly(Target objName) =>
            MoveToObject(objName, false);

        // Метод для перемещения к объекту с учетом возможного смещения
        private Operator MoveToObject(Target objName, bool offset)
        {
            // Поиск объекта и инициализация переменных
            AltObject target = UnityDriver.altDriver.FindObject(By.NAME, objName.name), prewCameraPoint, lastCameraPoint;
            int counter = 0;
            moveKeys = keysShiftUp;
            float lastDistZ, stepZ, lastDistX, stepX, step, dist;

            do
            {
                // Получение предыдущей и текущей позиций камеры и вычисление шагов и расстояний
                prewCameraPoint = MainScenePage.mainCamera.GetCameraPosition();
                UnityDriver.altDriver.PressKeys(moveKeys, 1, .5f);
                lastCameraPoint = MainScenePage.mainCamera.GetCameraPosition();
                lastDistX = Math.Abs(target.worldX - lastCameraPoint.worldX);
                lastDistZ = Math.Abs(target.worldZ - lastCameraPoint.worldZ);
                stepX = Math.Abs(prewCameraPoint.worldX - lastCameraPoint.worldX);
                stepZ = Math.Abs(prewCameraPoint.worldZ - lastCameraPoint.worldZ);
                step = Tren3DBaseMethods.CalculateStepDistance(stepX, stepZ);
                dist = Tren3DBaseMethods.CalculateTotalDistance(lastDistX, lastDistZ);

                // Выполнение шага вправо, если обнаружено препятствие
                if (step <= 0.003)
                    UnityDriver.altDriver.PressKeys(keysRight, 1, .5f);

                // Замедление при приближении к объекту
                if (!offset && dist < step + 3)
                    moveKeys = keysUp;
                if (offset && lastDistX < stepX + 3 || lastDistZ < stepZ + 3)
                    moveKeys = keysUp;

            } while ((!offset || lastDistX > stepX + 1 && lastDistZ > stepZ + 1) && dist > blindArea && ++counter < 30);

            return this;
        }

        // Метод для фокусировки на объекте
        public void FocuseOnCenterOfTarget(AltObject target)
        {
            Point screenCenter = Tren3DBaseMethods.GetScreenCenter(); // Получаем центр экрана

            // Перемещаем фокус камеры в центр целевого объекта
            for (int i = 1; i < 200; i++)
            {
                Tren3DBaseMethods.mouseCoords.X += (target.x - screenCenter.X) / 2;
                Tren3DBaseMethods.mouseCoords.Y += (target.y - screenCenter.Y) / 2;
                UnityDriver.altDriver.MoveMouse(new AltVector2(Tren3DBaseMethods.mouseCoords.X, Tren3DBaseMethods.mouseCoords.Y));

                target = MainScenePage.target.FindObjectByName();

                // Проверяем, достиг ли фокус экрана центра целевого объекта
                if (Math.Abs(screenCenter.X - target.x) < 15 && Math.Abs(screenCenter.Y - target.y) < 15)
                {
                    break;
                }
            }
        }

        public Operator TeleportToObject()
        {
            var offset = - 2.5f;
            Console.WriteLine($"Телепортация к объекту \"{MainScenePage.target.name}\"");
            // Находим целевой объект
            var target = MainScenePage.target.FindObjectByName();
            Assert.IsNotNull(target, "Целевой объект не найден");

            // Находим оператора
            var operatorPoint = MainScenePage.@operator.FindObjectByName();
            Assert.IsNotNull(operatorPoint, "Оператор не найден");

            const string charControllerComponentName = "UnityEngine.CharacterController";
            const string characterAssembly = "UnityEngine.PhysicsModule";
            const string enableName = "enabled";
            const string componentName = "UnityEngine.Transform";
            const string methodName = "localPosition";
            const string assemblyName = "UnityEngine.CoreModule";
            var angs = target.GetComponentProperty<Vector3>(componentName, "eulerAngles", assemblyName);
            if (angs != null && angs.Z <= 45 && angs.Z >= - 45)
                offset = 2.5f;
            operatorPoint.SetComponentProperty(charControllerComponentName,
                                               enableName, false, characterAssembly);
            operatorPoint.SetComponentProperty(componentName, methodName, new AltVector3(target.worldX + offset, 0, target.worldZ), assemblyName);
            operatorPoint.SetComponentProperty(charControllerComponentName,
                                               enableName, true, characterAssembly);
            MainScenePage.@operator.TurnToObject(MainScenePage.target);

            return this;
        }
    }
}
