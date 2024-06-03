using CTCAutoTests.TestModel.SimulatorApp.PageObjects;
using CTCAutoTests.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using System;
using System.Drawing;
using System.IO;
using static CTCAutoTests.Utilities.AppManager;
using static CTCAutoTests.Utilities.SimulatorBaseMethods;
using static CTCAutoTests.Utilities.Environment;


namespace CTCAutoTests.TestModel.SimulatorApp.PageElements
{
    public class Diagram
    {
        private WindowsElement diagram;


        /// <summary>
        /// Находит координаты выделенного элемента на схеме.
        /// </summary>
        /// <returns>Координаты выделенного элемента на схеме или null, если выделенный элемент не найден.</returns>
        public Coords FindHighlightedElementCoordinates()
        {
            // Инициализация координат
            var coordinates = new Coords(0);

            // Нахождение элемента схемы
            diagram = FindElement(SearchCondition.XPath, $"//Window[@ClassName=\"AfxFrameOrView90d\"][contains(@Name, \"{DIAGRAM_NAME}\")]", false, rootSession);

            // Получение идентификатора верхнего уровня схемы 
            var topLevelWindowHandle = GetTopLevelWindowHandle(diagram);

            // Создание сессии для схемы
            DiagramPage.DiagramSession = CreateWinAppSession("appTopLevelWindow", topLevelWindowHandle);

            // Получение скриншота схемы
            Screenshot screenshot = DiagramPage.DiagramSession.GetScreenshot();

            // Определение целевого цвета
            Color targetColor = Color.FromArgb(120, 120, 0);

            // Создание изображения из скриншота
            using (var image = new Bitmap(new MemoryStream(screenshot.AsByteArray)))
            {
                // Поиск координат цвета на изображении
                coordinates = FindColorCoordinates(image, targetColor);
            }

            return coordinates;
        }

        /// <summary>
        /// Щелчок мышью по заданным координатам на диаграмме.
        /// </summary>
        /// <param name="point">Координаты точки, на которую нужно щелкнуть.</param>
        public void ClickAtPointOnDiagram(Point point) =>
            new Actions(rootSession)
                .MoveToElement(diagram, 0, 0)
                .MoveByOffset(point.X, point.Y)
                .Click()
                .Perform();

        /// <summary>
        /// Определяет координаты первого и последнего пикселей в заданном цвете на изображении.
        /// </summary>
        /// <param name="image">Изображение, на котором нужно найти координаты.</param>
        /// <param name="color">Цвет, который нужно найти.</param>
        /// <returns>Координаты верхнего левого и нижнего правого углов области с заданным цветом или null, если область с заданным цветом не найдена.</returns>
        public Coords FindColorCoordinates(Bitmap image, Color color)
        {
            var coords = new Coords();

            // Поиск координат пикселей заданного цвета
            for (int x = 0; x < image.Width; x += 10)
                for (int y = 0; y < image.Height; y += 10)
                    if (image.GetPixel(x, y) == color)
                        coords.SetBounds(Math.Min(coords.Left, x), Math.Max(coords.Right, x), Math.Min(coords.Top, y), Math.Max(coords.Bottom, y));

            // Проверка, были ли найдены координаты с заданным цветом
            coords = coords == new Coords() ? null : coords;
            if (coords == null)
                Console.WriteLine("Выделение объекта не обнаружено на схеме");

            return coords;
        }
    }
}
