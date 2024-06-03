using CTCAutoTests.TestModel.SimulatorApp.PageObjects;
using CTCAutoTests.TestModel.Tren3DApp.PageObjects;
using CTCAutoTests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using static CTCAutoTests.Utilities.AppManager;
using static CTCAutoTests.Utilities.Environment;
using static CTCAutoTests.Utilities.SimulatorBaseMethods;


namespace CTCAutoTests.TestModel.Steps
{
    public class SimulatorSteps
    {
        /// <summary>
        /// Запустить Симулятор.
        /// </summary>
        public SimulatorSteps StartSimulator()
        {
            LaunchApp(SIMULATOR_NAME);
            Simulator.AppSession = Sessions[SIMULATOR_NAME];
            return this;
        }

        /// <summary>
        /// Запустить инициализацию модели.
        /// </summary>
        public SimulatorSteps ModelInitialization()
        {
            SimulatorPage.startButton.ClickButton(false, " - Инициализация модели");
            return this;
        }

        /// <summary>
        /// Запустить модель, дождавшись активного состояния кнопки Старт.
        /// </summary>
        public SimulatorSteps StartModel()
        {
            SimulatorPage.startButton.ClickButton(true, " - Запуск модели");
            return this;
        }

        /// <summary>
        /// Переключить окно Симулятора на передний план.
        /// </summary>
        public SimulatorSteps GoToSimulator()
        {
            SwitchAppToForeground(Simulator.simulatorName);
            return this;
        }

        /// <summary>
        /// Переключить окно со схемой на передний план.
        /// </summary>
        public SimulatorSteps GoToSimulatorDiagram()
        {
            SwitchAppToForeground(DIAGRAM_NAME, DiagramPage.DiagramSession);
            return this;
        }

        /// <summary>
        /// Открыть контекстное меню в Симуляторе.
        /// </summary>
        public SimulatorSteps OpenContextMenu()
        {
            // Откроем контекстное меню, щелкнув правой клавишей мыши в точке с координатами (45, 291)
            SimulatorBaseMethods.OpenContextMenu(Simulator.simulatorName, Simulator.AppSession, new Point(45, 291));
            return this;
        }

        /// <summary>
        /// Выбрать в контекстном меню пункт "Найти", подпункт "объект".
        /// </summary>
        public SimulatorSteps SelectFindObjectInContextMenu()
        {
            SimulatorPage.contextMenu.findItem.Click();
            SimulatorPage.contextMenu.findItem.@object.Click(); 
            return this;
        }

        /// <summary>
        /// Ввести в поле поиска страницы Все объекты имя искомого объекта и нажать кнопку "!".
        /// </summary>
        /// <param name="objectName">Имя искомого объекта.</param>
        public SimulatorSteps EnterObjNameAndClickExclamButtonOnAllObjectsPage(string objectName = null)
        {
            objectName = objectName ?? MainScenePage.target.name;
            AllObjectsPage.FillSearchField(objectName); 
            AllObjectsPage.exclamationButton.ClickButton(); 
            return this;
        }

        /// <summary>
        /// Сделать двойной щелчок мышью по найденной на странице Все объекты записи.
        /// </summary>
        public SimulatorSteps DoubleClickOnSearchResultOnAllObjectsPage()
        {
            if (AllObjectsPage.searchResultItem != null)
                AllObjectsPage.searchResultItem.DoubleClick();
            else
                Console.WriteLine("Элемент поиска не был найден");
            return this;
        }

        /// <summary>
        /// Щелкнуть мышью по подсвеченному элементу на схеме.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public SimulatorSteps ClickHighlightedElementOnDiagram()
        {
            Console.WriteLine($"Выбор объекта на схеме для просмотра его параметров");
            var coordinates = DiagramPage.diagram.FindHighlightedElementCoordinates();
            if (coordinates != null)
                DiagramPage.diagram.ClickAtPointOnDiagram(CalculateMidpoint(coordinates));
            else
                throw new InvalidOperationException("Выделение объекта не обнаружено на схеме");

            return this;
        }

        /// <summary>
        /// Проверить, содержит ли окно параметров имя искомого объекта.
        /// </summary>
        /// <param name="objectName"></param>
        public SimulatorSteps CheckIfParametersWindowContainsObjectName(string objectName)
        {
            Console.WriteLine($"Проверка, что открылось окно параметров объекта {objectName}");
            Assert.IsTrue(ObjectParametersPage.GetName().Contains(objectName),
                $"Окно параметров объекта {objectName} не было открыто");

            return this;
        }

        /// <summary>
        /// Перейти в папку Поле - Задвижки.
        /// </summary>
        public SimulatorSteps GoToField_GatesFolderAndSelectExitParameter()
        {
            ObjectParametersPage.exitParameter.OpenParameterFolder();
            ObjectParametersPage.exitParameter.SelectParameter();

            return this;
        }

        /// <summary>
        /// Получить значение параметра Поле - Задвижки - Выход.
        /// </summary>
        public SimulatorSteps RetrieveField_Gates_ExitValue(out double parametrValue)
        {
            parametrValue = ObjectParametersPage.exitParameter.GetValue();

            return this;
        }

        /// <summary>
        /// Перейти в папку Отказы и выбрать параметр Вывод двигателя из строя.
        /// </summary>
        public SimulatorSteps GoToFailureFolderAndSelectEngineFailureParameter()
        {
            ObjectParametersPage.engineFailureParameter.OpenParameterFolder();
            ObjectParametersPage.engineFailureParameter.SelectParameter();

            return this;
        }

        /// <summary>
        /// Перейти в папку Отказы и выбрать параметр Восстановление работоспособности.
        /// </summary>
        public SimulatorSteps GoToFailureFolderAndSelectRestorationOfPerformanceParameter()
        {
            ObjectParametersPage.restorationOfPerformanceParameter.OpenParameterFolder();
            ObjectParametersPage.restorationOfPerformanceParameter.SelectParameter();

            return this;
        }

        /// <summary>
        /// Нажать кнопку "10% +".
        /// </summary>
        public SimulatorSteps ClickPlus10PercentButton()
        {
            ObjectParametersPage.plus10PercentButton.ClickButton(false, "", DiagramPage.DiagramSession);
            return this;
        }

        /// <summary>
        /// Нажать кнопку "10% +".
        /// </summary>
        public SimulatorSteps ClickMinus10PercentButton()
        {
            ObjectParametersPage.minus10PercentButton.ClickButton(false, "", DiagramPage.DiagramSession);
            return this;
        }


        /// <summary>
        /// Проверить, что произошла успешная инициализация модели.
        /// </summary>
        public SimulatorSteps CheckThatModelWasSuccessfullyInitialized()
        {
            Console.WriteLine("Проверка процесса инициализации модели");

            // Создаем словарь уникальных скриншотов и конструктор распознанного текста.
            Dictionary<string, byte[]> uniqueScreenshots = new Dictionary<string, byte[]>();
            StringBuilder recognizedTextBuilder = new StringBuilder();

            // Снимаем скриншоты в течение 30 секунд, пока кнопка "Старт" неактивна.
            List<byte[]> screenshotList = SimulatorPage.TakeScreenshots(30);

            // Обрабатываем скриншоты, извлекая уникальные скриншоты и распознавая текст.
            ScreenImage.ProcessScreenshots(screenshotList, uniqueScreenshots, recognizedTextBuilder);

            // Сохраняем распознанный текст в файл.
            ScreenImage.SaveRecognizedText(recognizedTextBuilder, RESOURCES_FOLDER);

            // Сравниваем распознанный текст с эталонным текстом.
            if (ScreenImage.CompareTextWithEtalon(RESOURCES_FOLDER + "\\recognized_text.txt", Utilities.Environment.RESOURCES_FOLDER + "\\etalon.txt"))
                Console.WriteLine("Успешная инициализация модели");
            else
                Console.WriteLine("Инициализация модели не стартовала");

            return this;
        }

        /// <summary>
        /// Ввести новое значение параметра Поле - Задвижки - Выход.
        /// </summary>
        public SimulatorSteps SetField_Gates_ExitValue(int parametrValue)
        {
            ObjectParametersPage.exitParameter.SelectParameter();
            ObjectParametersPage.exitParameter.SetValue(parametrValue);

            return this;
        }

        /// <summary>
        /// Получить значение каждого шага в Симуляторе.
        /// </summary>
        public void GetEveryStepCount(int limit, int stepClick)
        {
            //Рассчитаем требуемое количество нажатий на клавишу
            var st = limit / stepClick;

            for (int i = 0; i <= st - 1; i++)
            {
                PerformKeyboardAction(stepClick); // Нажмем на заданную клавишу
                Console.WriteLine($"Шаг: {(i + 1) * stepClick}");
                GetStepNumber(); // Получим значение количества шагов, отображаемых Симулятором
            }
        }

        /// <summary>
        /// Распознать значение шага в Симуляторе.
        /// </summary>
        public void GetStepNumber()
        {
            string output = "";
            // Снимем скриншот окна приложения
            Screenshot screenshot = Simulator.AppSession.GetScreenshot();

            using (var fulImage = new Bitmap(new MemoryStream(screenshot.AsByteArray)))
            {
                // Подготавливаем изображение для распознавания
                var preparedImage = ScreenImage.PrepareImageForRecognition(fulImage, new Rectangle(170, 200, 200, 50));

                // Распознаем текст на изображении
                string recognizedText = ScreenImage.RecognizeNumericFromImage(preparedImage);

                // Проверяем - если распознанный текст - число, выводим его
                if (int.TryParse(recognizedText, out int value))
                    output = $"Количество шагов: {recognizedText}. Распознано с первой попытки";

                // Если распознанный текст - не число, масштабируем изображение и распознаем повторно
                else
                    output = ScreenImage.ReRecognizeNumeric(preparedImage, recognizedText);

                Console.WriteLine(output);
            }
        }
    }
}
