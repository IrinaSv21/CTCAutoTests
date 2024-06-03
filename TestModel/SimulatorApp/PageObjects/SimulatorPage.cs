using CTCAutoTests.TestModel.SimulatorApp.PageElements;
using System.Collections.Generic;

namespace CTCAutoTests.TestModel.SimulatorApp.PageObjects
{
    public class SimulatorPage
    {
        public static Button startButton;
        public static Button okButton;
        public static ContextMenu contextMenu;

        public SimulatorPage()
        {
            startButton = new Button("40084", "Старт");
            okButton = new Button("1", "Ok");
            contextMenu = new ContextMenu();
        }

        /// <summary>
        /// Снимает скриншоты окна приложения, пока неактивна кнопка Старт, но не более заданного количества секунд.
        /// </summary>
        /// <param name="maxDurationSec">Максимальное количество секунд.</param>
        /// <returns>Список байтовых массивов, содержащих скриншоты.</returns>
        public static List<byte[]> TakeScreenshots(int maxDurationSec)
        {
            List<byte[]> screenshotList = new List<byte[]>();
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            do
            {
                byte[] screenshotBytes = Simulator.AppSession.GetScreenshot().AsByteArray;
                screenshotList.Add(screenshotBytes);

                if (stopwatch.Elapsed.TotalSeconds >= maxDurationSec)
                    break;
            }
            while (!startButton.IsActive());

            stopwatch.Stop();
            return screenshotList;
        }
    }
}
