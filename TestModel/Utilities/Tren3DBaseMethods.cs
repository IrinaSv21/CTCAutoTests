using AltTester.AltTesterUnitySDK.Driver;
using CTCAutoTests.TestModel.Tren3DApp.PageObjects;
using System;
using System.Drawing;
using System.Numerics;
using System.Text.RegularExpressions;

namespace CTCAutoTests.Utilities
{
    public class Tren3DBaseMethods : AppManager
    {
        public static Point mouseCoords = new Point(0, 0);

        // Метод для вычисления координат центра экрана Tren3D_LABUPO_KF
        public static Point GetScreenCenter()
        {
            UnityDriver.EnsureAltDriverConnected(20); // Проверка подключения к altDriver.

            // Получаем размер экрана приложения
            var screenSize = UnityDriver.altDriver.GetApplicationScreenSize();

            // Возвращаем координаты центра экрана
            return new Point((int)screenSize.x / 2, (int)screenSize.y / 2);
        }

        // Метод для вычисления шага поворота
        public static int CalculateRotationStep(Point screenCenter) =>
            screenCenter.X / 30;

        // Метод для приведения угла камеры к диапазону от -180 до 180 градусов
        public static double NormalizeAngle(Vector3 rotation)
        {
            var adjustedCameraAngle = 90 - rotation.Y;

            if (adjustedCameraAngle <= -180)
                adjustedCameraAngle += 360;

            return adjustedCameraAngle;
        }

        // Метод для вычисления угла наклона объекта относительно камеры
        public static double CalculateTargetToCameraAngle(AltObject target, AltObject cameraPoint)
        {
            var relativeX = target.worldX - cameraPoint.worldX;
            var relativeY = target.worldZ - cameraPoint.worldZ;
            var angle = Math.Atan2(relativeY, relativeX) * 180 / Math.PI;

            return angle;
        }

        // Метод для корректировки значения шага в зависимости от угла поворота камеры и угла объекта к камере
        public static void AdjustStepBasedOnCameraAngle(ref int step, double adjustedCameraAngle, double targetToCameraAngle)
        {
            if (adjustedCameraAngle < Math.Abs(targetToCameraAngle))
                step *= -1;
        }

        // Метод для поворота камеры, пока объект не окажется в фокусе
        public static void RotateCameraToFocus(AltObject target, int step, Point screenCenter, int offset, string objName)
        {
            try
            {
                for (int i = 1; i < 200; i++)
                {
                    // Проверяем, находится ли цель за пределами экрана
                    bool isTargetOutsideScreen = target.x < 0 || target.x > screenCenter.X * 2 || target.z <= 0;

                    // Если цель за пределами экрана, двигаем камеру быстрее
                    if (isTargetOutsideScreen)
                    {
                        mouseCoords.X += step * 7;
                    }
                    else
                    {
                        mouseCoords.X += step;
                    }

                    // Перемещаем мышь на новую позицию
                    UnityDriver.altDriver.MoveMouse(new AltVector2(mouseCoords.X, mouseCoords.Y));

                    // Повторно находим цель после перемещения камеры
                    target = UnityDriver.altDriver.FindObject(By.NAME, objName);

                    // Если цель приблизилась к центру экрана и находится в положительной области z, завершаем поворот камеры
                    if (Math.Abs(target.x - screenCenter.X + offset) < offset / 5 + screenCenter.X / 30 && target.z > 0)
                    {
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка во время поворота камеры: {ex.Message}");
            }
        }

        // Метод для разбора текстового значения параметра объекта
        public static (string name, double value, string unit) ParseObjectTextValue(string colorObjectText)
        {
            if (colorObjectText != null)
            {
                // Разбиение входной строки на отдельные части
                var tokens = colorObjectText.Split('\n');

                if (tokens.Length >= 2)
                {
                    // Получение наименования параметра объекта
                    var name = tokens[0].Trim();

                    // Поиск числового значения в строке
                    var valueUnitMatch = Regex.Match(tokens[1], @"[0-9]*[.,]?[0-9]+"); // Соответствие числовому значению без единиц измерения

                    if (valueUnitMatch.Success)
                    {
                        // Попытка преобразования числового значения в тип float
                        if (double.TryParse(valueUnitMatch.Value, out double value))
                        {
                            double roundedValue = Math.Round(value, 3);

                            // Получение единицы измерения
                            var unit = tokens[1].Substring(valueUnitMatch.Index + valueUnitMatch.Length).Trim();

                            // Возврат разобранных значений параметра объекта
                            return (name, roundedValue, unit);
                        }
                        else
                        {
                            Console.WriteLine("Ошибка: невозможно распарсить числовое значение параметра объекта");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: неверный формат значения параметра объекта");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: недостаточное количество строк для разбора параметра объекта");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: пустое значение параметра объекта");
            }

            // Возврат значений по умолчанию в случае ошибок
            return (null, 0, null);
        }

        // Метод для вывода информации о значении параметра объекта
        public static void PrintObjectValueInfo(string name, double value, string unit) =>
            Console.WriteLine($"Значение параметра объекта {name} = {value}{unit}");

        // Метод для расчета общего расстояния
        public static float CalculateTotalDistance(float distX, float distZ) =>
            (float)Math.Sqrt(Math.Pow(distX, 2) + Math.Pow(distZ, 2));

        // Метод для вычисления шага
        public static float CalculateStepDistance(float stepX, float stepZ) =>
            (float)Math.Sqrt(Math.Pow(stepX, 2) + Math.Pow(stepZ, 2));


    }
}


//пишем лог
//var logs = new List<string>();
//logs.Add($"#{counter}. lastDistX: {lastDistX}, stepX: {stepX}, lastDistZ: {lastDistZ}, stepZ: {stepZ}");


