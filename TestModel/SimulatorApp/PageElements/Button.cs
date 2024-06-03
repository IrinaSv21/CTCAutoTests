using OpenQA.Selenium.Appium.Windows;
using System;
using static CTCAutoTests.Utilities.SimulatorBaseMethods;

namespace CTCAutoTests.TestModel.SimulatorApp.PageElements
{
    public class Button
    {
        public string id;
        private string name;

        public Button(string id, string name)
        {            
            this.id = id;
            this.name = name;
        }

        /// <summary>
        /// Находит кнопку по ее AutomationID и нажимает на нее.
        /// </summary>
        /// <param name="wait">(необязательно): если установлено в значение true, будет ждать, пока кнопка станет активной.</param>
        /// <param name="action">(необязательно): дополнительная информация о действии нажатия на кнопку, которая будет отображаться в журнале.</param>
        public Button ClickButton(bool wait = false, string action = "", WindowsDriver<WindowsElement> session = null)
        {
            Console.WriteLine($"Нажатие кнопки \"{name}\"{action}");
            var button = FindElement(SearchCondition.Id, id, wait, session); // Найдем кнопку

            // Если кнопка найдена, нажать на нее
            if (button != null)
                button.Click();
            else
                Console.WriteLine($"Кнопка \"{name}\" не найдена");
            return this;
        }

        /// <summary>
        /// Ожидает, пока кнопка с указанным идентификатором станет активной.
        /// </summary>
        public void WaitIsActive()
        {
            WaitElementToBeActive(id);
        }

        /// <summary>
        /// Проверяет, активна ли кнопка с указанным идентификатором.
        /// </summary>
        public bool IsActive()
        {
            try
            {
                var winElem = FindElement(SearchCondition.Id, id);
                if (winElem != null)
                {
                    var isActive = winElem.Enabled;
                    return isActive;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Не удалось найти элемент: " + ex.Message);
            }
            throw new Exception("Элемент не найден");
        }
    }
}
