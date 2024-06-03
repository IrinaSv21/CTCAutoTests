using AltTester.AltTesterUnitySDK.Driver;
using CTCAutoTests.Utilities;
using System;

namespace CTCAutoTests.TestModel.Tren3DApp.PageElements
{
    public class Object
    {
        public string name;

        public Object(string name)
        {
            this.name = name;
        }

        // Метод для поиска объекта по имени
        public AltObject FindObjectByName()
        {
            try
            {
                // Ожидание появления объекта
                UnityDriver.altDriver.WaitForObject(By.NAME, name);

                // Нахождение объекта
                return UnityDriver.altDriver.FindObject(By.NAME, name);
            }
            catch (Exception ex)
            {
                // Вывод сообщения об ошибке
                Console.WriteLine($"Объект с именем {name} не найден. Ошибка: {ex.Message}");

                // Возврат null
                return null;
            }
        }
    }
}
