using CTCAutoTests.TestModel.SimulatorApp.PageObjects;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;
using CTCAutoTests.TestModel.SimulatorApp.PageElements;

namespace CTCAutoTests.Utilities

{
    public class SimulatorBaseMethods : AppManager
    {
        public enum SearchCondition
        {
            Name,
            Id,
            XPath,
            ClassName
        }

        public static WindowsDriver<WindowsElement> rootSession;

        // Метод создания новой сессии драйвера для доступа к меню
        public static void CreateRootSession()
        {
            rootSession = CreateWinAppSession("app", "Root");
        }

        // Метод получения элемента
        public static WindowsElement FindElement(SearchCondition searchCondition, string searchConditionValue, bool wait = false, WindowsDriver<WindowsElement> session = null)
        {
            session = session ?? Simulator.AppSession;

            // Найдем элемент по указанному условию
            WindowsElement winElem = FindElementBySearchCriteria(searchCondition, searchConditionValue, session);

            // Дождемся что элемент стал доступным, если это требуется
            if (wait && winElem != null)
                WaitUntilElementIsEnabled(winElem, session);

            return winElem;
        }

        // Метод ожидания, пока элемент станет доступным
        private static void WaitUntilElementIsEnabled(WindowsElement element, WindowsDriver<WindowsElement> session)
        {
            var waiter = new WebDriverWait(session, TimeSpan.FromSeconds(30));
            waiter.Until(d => element.Enabled);
        }

        // Метод для поиска элемента по указанному условию (имени, идентификатору или XPath)
        private static WindowsElement FindElementBySearchCriteria(SearchCondition searchCondition, string searchConditionValue, WindowsDriver<WindowsElement> session)
        {
            try
            {
                // Найдем и получим элемент
                switch (searchCondition)
                {
                    case SearchCondition.Name:
                        return session.FindElementByName(searchConditionValue);
                    case SearchCondition.Id:
                        return session.FindElementByAccessibilityId(searchConditionValue);
                    case SearchCondition.XPath:
                        return session.FindElementByXPath(searchConditionValue);
                    case SearchCondition.ClassName:
                        return session.FindElementByClassName(searchConditionValue);
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                // Если элемент не найден, выводим сообщение и останавливаем выполнение теста
                Console.WriteLine($"Произошла ошибка при поиске элемента '{searchConditionValue}': {ex.Message}");
            }
            return null;
        }

        // Метод щелчка мышью по пункту меню
        public static void ClickElement(string elName, WindowsDriver<WindowsElement> session)
        {
            var menuItem = FindElement(SearchCondition.Name, elName, false, session);  // Найдем пункт меню

            // Если пункт меню найден, щелкнем по нему
            if (menuItem != null)
                menuItem.Click();
            else
                Console.WriteLine($"Элемент \"{elName}\" не найден");
        }


        // Метод для перехода в приложение
        public static void SwitchAppToForeground(string appName, WindowsDriver<WindowsElement> session = null)
        {
            session = session ?? Simulator.AppSession;
            Console.WriteLine($"Переход в приложение \"{appName}\"");
            session.SwitchTo().Window(session.WindowHandles.First());
        }

        //Метод открытия контекстного меню
        public static void OpenContextMenu(string elemName, WindowsDriver<WindowsElement> session = null, System.Drawing.Point offset = default)
        {
            // Cмещение, чтобы щелкнуть мышью по пустому полю приложения
            if (offset == default)
            {
                offset = new System.Drawing.Point(0, 0);
            }

            session = session ?? Simulator.AppSession;
            Console.WriteLine($"Открытие контекстного меню элемента \"{elemName}\"");

            // Найдем элемент для открытия контекстного меню
            var winElem = FindElement(SearchCondition.Name, elemName, false, session);
            if (winElem == null)
            {
                Console.WriteLine($"Элемент \"{elemName}\" не найден");
                return;
            }

            // Откроем контекстное меню этого элемента
            OpenContextMenuAtElement(winElem, session, offset);
        }

        // Метод открытия контекстного меню у заданного элемента
        private static void OpenContextMenuAtElement(WindowsElement element, WindowsDriver<WindowsElement> session, System.Drawing.Point offset)
        {
            PerformRightClickAtElementWithOffset(element, session, offset); // Выполним правый щелчок мышью для открытия контекстного меню
            ContextMenu.CreateMenuSession(); // Создадим сессию для контекстного меню
        }

        // Метод для щелчка правой кнопкой мыши с заданным смещением
        private static void PerformRightClickAtElementWithOffset(WindowsElement element, WindowsDriver<WindowsElement> session, System.Drawing.Point offset)
        {
            new Actions(session)
                  .MoveToElement(element, offset.X, offset.Y)
                  .ContextClick()
                  .Perform();
        }

        // Метод нахождения значения окна верхнего уровня
        public static string GetTopLevelWindowHandle(WindowsElement element)
        {
            var topLevelWindowHandle = element.GetAttribute("NativeWindowHandle");
            return int.Parse(topLevelWindowHandle).ToString("x");
        }

        // Метод для переключения раскладки клавиатуры на английский
        public static void SwitchToUsKeyboard() =>
            new Actions(Simulator.AppSession)
                    .SendKeys(Keys.Control + "0" + Keys.Control)
                    .Perform();

        // Метод выполнения двойного щелчка мышью по элементу
        public static void DoubleClickElement(SearchCondition searchCondition, string searchConditionValue, string name, bool wait = false, WindowsDriver<WindowsElement> session = null)
        {
            session = session ?? Simulator.AppSession;
            var winElem = FindElement(searchCondition, searchConditionValue, wait, session);  // Найдем элемент

            // Проверяем, был ли найден элемент
            if (winElem != null)
            {
                // Если элемент найден, выполним двойной щелчок по нему
                DblClick(winElem, session);
            }
            else
            {
                Console.WriteLine($"Элемент \"{name}\" не был найден");                
            }
        }

        // Вспомогательный метод для выполнения двойного щелчка
        private static void DblClick(WindowsElement winElem, WindowsDriver<WindowsElement> session) =>
            new Actions(session)
                    .MoveToElement(winElem)
                    .DoubleClick()
                    .Perform();

        // Метод установки времени ожидания (мин.)
        public static void WaitTimeMin(double t0)
        {
            Thread.Sleep((int)(t0 * 60000));
            Console.WriteLine("Прошло времени:" + t0 + " мин.");
        }

        // Метод ожидания активности элемента
        public static void WaitElementToBeActive(string searchConditionValue, SearchCondition searchСondition = SearchCondition.Id)
        {
            var winElem = FindElement(searchСondition, searchConditionValue);
            var waiter = new WebDriverWait(Simulator.AppSession, TimeSpan.FromSeconds(30));
            waiter.Until(d => winElem.Enabled);
        }

        // Метод нажатия заданной клавиши на клавиатуре
        public static void PerformKeyboardAction(int stepClick)
        {
            new Actions(Simulator.AppSession)
                .SendKeys(stepClick.ToString())
                .Perform();
            Thread.Sleep(1700);
        }

        // Метод для вычисления координат точки посередине между двумя заданными точками
        public static System.Drawing.Point CalculateMidpoint(Coords coordinates) =>
            new System.Drawing.Point((coordinates.Left + coordinates.Right) / 2, (coordinates.Top + coordinates.Bottom) / 2);
    }
}
