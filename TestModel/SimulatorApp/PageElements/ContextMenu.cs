using CTCAutoTests.Utilities;
using OpenQA.Selenium.Appium.Windows;
using static CTCAutoTests.Utilities.SimulatorBaseMethods;

namespace CTCAutoTests.TestModel.SimulatorApp.PageElements
{
    public class ContextMenu
    {
        public ContextMenuItem findItem;
        public static WindowsDriver<WindowsElement> menuSession;

        public ContextMenu()
        {
            findItem = new ContextMenuItem("Найти");
        }

        /// <summary>
        /// Создает новую сессию драйвера для доступа к меню.
        /// </summary>
        /// <returns>Сессия драйвера для доступа к меню.</returns>
        public static void CreateMenuSession()
        {
            CreateRootSession();
            var menu = FindElement(SearchCondition.Name, "Контекст", false, rootSession);
            var topLevelWindowHandle = GetTopLevelWindowHandle(menu);
            menuSession = AppManager.CreateWinAppSession("appTopLevelWindow", topLevelWindowHandle);
        }
    }
}
