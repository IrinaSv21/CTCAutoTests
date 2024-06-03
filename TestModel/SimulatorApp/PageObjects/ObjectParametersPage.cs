using CTCAutoTests.Utilities;
using CTCAutoTests.TestModel.SimulatorApp.PageElements;

namespace CTCAutoTests.TestModel.SimulatorApp.PageObjects
{
    public class ObjectParametersPage
    {
        public static Parameter enterParameter;
        public static Parameter exitParameter;
        public static Parameter engineFailureParameter;
        public static Parameter restorationOfPerformanceParameter;
        public static Button plus10PercentButton;
        public static Button minus10PercentButton;
        public ObjectParametersPage()
        {
            enterParameter = new Parameter("Вход", "Поле", "Задвижки");
            exitParameter = new Parameter("Выход", "Поле", "Задвижки");
            engineFailureParameter = new Parameter("Выход двигателя из строя", "Отказы", null);
            restorationOfPerformanceParameter = new Parameter("Восстановление работоспособности", "Отказы", null);
            plus10PercentButton = new Button("1323", "10% +");
            minus10PercentButton = new Button("1322", "10% -");
        }

        /// <summary>
        /// Определяет имя заголовка окна Параметры объекта.
        /// </summary>
        /// <returns>Строку с именем заголовка окна Параметры объекта.</returns>
        public static string GetName()
        {
            var window = SimulatorBaseMethods.FindElement(SimulatorBaseMethods.SearchCondition.ClassName, "#32770", false, DiagramPage.DiagramSession);
            return window.Text;
        }
    }
}
