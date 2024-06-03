using AltTester.AltTesterUnitySDK.Driver;
using CTCAutoTests.TestModel.Tren3DApp.PageObjects;
using CTCAutoTests.Utilities;
using System;
using System.Threading;
using static CTCAutoTests.Utilities.AppManager;
using static CTCAutoTests.Utilities.Environment;
using static CTCAutoTests.Utilities.SimulatorBaseMethods;

namespace CTCAutoTests.Tren3D.Steps
{
    public class Tren3DSteps
    {
        /// <summary>
        /// Запустить Tren3D.
        /// </summary>
        public Tren3DSteps StartTren3D()
        {
            LaunchApp(TREN3D_NAME);
            return this;
        }

        /// <summary>
        /// Авторизоватьсяя в Tren3D_LABUPO_KF без сервера, через анонимный вход.
        /// </summary>
        public Tren3DSteps LoginAsAnonymousWithoutServer()
        {
            UnityDriver.InitAltDriver(); // Инициализация AltDriver
            UnityDriver.EnsureAltDriverConnected(60); // Проверка подключения к altDriver.
            StartMenuPage.menuScene.WaitForCurrentSceneToBe(); // Ожидание загрузки меню
            StartMenuPage.startWithoutServerButton.WaitForAppear(); // Ожидание загрузки кнопки Запуститься без сервера
            StartMenuPage.startWithoutServerButton.Click(); // Нажатие на кнопку Запуститься без сервера
            StartMenuPage.anonymousButton.WaitForAppear(); // Ожидание загрузки кнопки Анонимный вход
            StartMenuPage.anonymousButton.Click(); // Нажатие на кнопку Анонимный вход
            StartMenuPage.mainScene.WaitForCurrentSceneToBe(); // Ожидание загрузки главной сцены

            return this;
        }

        /// <summary>
        /// Подойти к объекту ValveFWManipulator_01_0220_014.
        /// </summary>
        public Tren3DSteps ApproachTheObject()
        {
            MainScenePage.@operator.TurnToObject(MainScenePage.target, 150);
            MainScenePage.@operator.MoveToObjectWithRightOffset(MainScenePage.target);
            MainScenePage.@operator.TurnToObject(MainScenePage.target);
            MainScenePage.@operator.MoveToObjectDirectly(MainScenePage.target);

            return this;
        }

        /// <summary>
        /// Навести фокус на объект до получения подсвеченной рамки объекта.
        /// </summary>
        public Tren3DSteps FocusAndVerifyObjectHighlighting()
        {
            // Находим целевой объект
            AltObject target = MainScenePage.target.FindObjectByName();

            // Перемещаем фокус экрана в центр целевого объекта
            MainScenePage.@operator.FocuseOnCenterOfTarget(target);

            // Проверяем, появилась ли рамка у объекта
            bool isHighlighted = MainScenePage.target.IsObjectHighlighted();
            if (!isHighlighted)
                throw new Exception($"Объект {MainScenePage.target.UnityObjectName} не подсвечен. Возможно есть проблемы с соединением с сервером.");

            return this;
        }

        /// <summary>
        /// Получить имя и значение параметра объекта с подсвеченной рамкой.
        /// </summary>
        public Tren3DSteps GetHighlightedObjectValue(out string objectName, out double objectValue)
        {
            // Находим объект с подсвеченной рамкой
            var frame = MainScenePage.mainInfo.FindObjectByName();

            // Получаем текстовое значение компонента
            var colorObjectText = MainScenePage.target.GetObjectTextValue(frame);

            // Разбираем текстовое значение на части
            var (name, dValue, unit) = Tren3DBaseMethods.ParseObjectTextValue(colorObjectText);

            // Выводим информацию о параметре объекта
            Tren3DBaseMethods.PrintObjectValueInfo(name, dValue, unit);

            objectName = name;
            objectValue = dValue;
            return this;
        }

        /// <summary>
        /// Переключить окно Tren3D на передний план.
        /// </summary>
        public Tren3DSteps GoToTren3D()
        {
            SwitchAppToForeground(TREN3D_NAME, Sessions[TREN3D_NAME]);
            return this;
        }

        /// <summary>
        /// Покрутить роликом мыши вниз для изменения значения параметра.
        /// </summary>
        public Tren3DSteps MouseScrollDown()
        {
            UnityDriver.altDriver.Scroll(-500, 2);
            Thread.Sleep(300);
            return this;
        }

        /// <summary>
        /// Телепортироваться к объекту.
        /// </summary>
        internal Tren3DSteps TeleportToTheObject()
        {
            MainScenePage.@operator.TeleportToObject();
            return this;
        }

        /// <summary>
        /// Проверить, что объект горит.
        /// </summary>
        internal Tren3DSteps CheckThatTheObjectIsOnFire()
        {
            var objectIsOnFire = MainScenePage.target.IsObjectOnFire();
            if (!objectIsOnFire)
                throw new Exception($"Ожидалось, что объект горит, но огонь отсутствует");
            return this;
        }

        /// <summary>
        /// Проверить, что объект не горит.
        /// </summary>
        internal Tren3DSteps CheckThatTheObjectIsNotOnFire()
        {
            var objectIsOnFire = MainScenePage.target.IsObjectOnFire();
            if (objectIsOnFire)
                throw new Exception($"Ожидалось, что объект не горит, но огонь присутствует");
            return this;
        }

    }
}
