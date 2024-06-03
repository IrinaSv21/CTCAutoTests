using OpenQA.Selenium.Interactions;
using CTCAutoTests.TestModel.SimulatorApp.PageObjects;
using System;
using System.Globalization;
using static CTCAutoTests.Utilities.SimulatorBaseMethods;

namespace CTCAutoTests.TestModel.SimulatorApp.PageElements
{
    public class Parameter
    {
        private string parameterName;
        private string folderName;
        private string subfolderName;
        private double parameterValue;

        public Parameter(string name, string folder, string subfolder = null)
        {
            this.parameterName = name;
            this.folderName = folder;
            this.subfolderName = subfolder;
        }

        /// <summary>
        /// Открывает папку указанного параметра на диаграмме.
        /// </summary>
        public void OpenParameterFolder()
        {
            // Выбор основной папки
            Console.WriteLine($"Выбор папки \"{folderName}\"");
            DoubleClickElement
                (SearchCondition.Name, folderName, folderName, false, DiagramPage.DiagramSession);

            // Если есть дополнительная подпапка
            if (subfolderName != null)
            {
                Console.WriteLine($"Выбор подпапки \"{subfolderName}\"");
                ClickElement(subfolderName, DiagramPage.DiagramSession);
            }
        }

        /// <summary>
        /// Выбирает указанный параметр на диаграмме.
        /// </summary>
        public void SelectParameter()
        {
            Console.WriteLine($"Выбор параметра \"{parameterName}\"");
            // Найдем параметр
            var param = FindElement
                (SearchCondition.XPath,$"//ListItem[@Name=\"{parameterName}\"]/Text[@Name=\"{parameterName}\"]",
                false, DiagramPage.DiagramSession);

            // Если параметр найден, щелкнем по нему
            if (param != null)
                param.Click();
            else
                Console.WriteLine($"Параметр \"{parameterName}\" не найден");
        }

        /// <summary>
        /// Получает значение указанного параметра на диаграмме.
        /// </summary>
        /// <returns>Значение указанного параметра.</returns>
        public double GetValue()
        {
            Console.WriteLine($"Получение значения параметра \"{parameterName}\"");
            // Найдем параметр
            var param = FindElement
                (SearchCondition.XPath, $"//ListItem[@Name=\"{parameterName}\"]/Text[@AutomationId=\"ListViewSubItem-1\"]", 
                false, DiagramPage.DiagramSession);

            // Если параметр найден, получим его значение
            if (param != null)
            {
                if (double.TryParse(param.GetAttribute("Name"), NumberStyles.Number, 
                    CultureInfo.InvariantCulture, out parameterValue))
                    Console.WriteLine($"Значение параметра \"{parameterName}\" = {parameterValue}");
                else
                    Console.WriteLine($"Не удалось получить значение параметра \"{parameterName}\"");
            }
            else
                Console.WriteLine($"Параметр \"{parameterName}\" не найден");

            return parameterValue;
        }

        /// <summary>
        /// Вводит указанное значение в указанный параметр на диаграмме.
        /// </summary>
        /// <param name="value">Значение, которое нужно ввести.</param>
        public void SetValue(int value)
        {
            Console.WriteLine($"Ввод значения параметра \"{parameterName}\" = {value}");
            // Найдем параметр
            var param = FindElement
                (SearchCondition.XPath, $"//Window[@Name=\"FV102 (Клапан в сборке)\"]/List[@ClassName=\"SysListView32\"]/ListItem[@Name=\"Выход\"][@AutomationId =\"ListViewItem-1\"]/Text[@AutomationId =\"ListViewSubItem-1\"]",
                false, DiagramPage.DiagramSession);

            // Если параметр найден, введем новое значение
            if (param != null)
            {
                //param.Click();

                new Actions(DiagramPage.DiagramSession)
                    .Click(param)
                    .SendKeys($"{value}")
                    .SendKeys(OpenQA.Selenium.Keys.Enter)
                    .Perform();
            }
            else
                Console.WriteLine($"Параметр \"{parameterName}\" не найден");
        }
    }
}
