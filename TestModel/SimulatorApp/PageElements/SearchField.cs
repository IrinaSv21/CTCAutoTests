using System;
using static CTCAutoTests.Utilities.SimulatorBaseMethods;

namespace CTCAutoTests.TestModel.SimulatorApp.PageElements
{
    public class SearchField
    {
        private string id;
        private string name;

        public SearchField(string id, string name)
        {            
            this.id = id;
            this.name = name;
        }

        /// <summary>
        /// Вводит указанный текст в поле поиска.
        /// </summary>
        /// <param name="searchText">Текст, который нужно ввести.</param>
        public void Fill(string searchText)
        {
            Console.WriteLine($"Ввод текста \"{searchText}\" в поле \"{name}\"");
            var field = FindElement(SearchCondition.Id, id); // Найдем поле поиска

            // Если поле найдено, введем в него текст
            if (field != null)
            {
                SwitchToUsKeyboard(); // Переключим раскладку клавиатуры на английский язык
                field?.SendKeys(searchText);
            }
            else
            {
                Console.WriteLine($"Поле \"{name}\" не найдено");
            }
        }

    }
}
