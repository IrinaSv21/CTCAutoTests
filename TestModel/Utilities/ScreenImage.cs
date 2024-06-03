using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Tesseract;

namespace CTCAutoTests.Utilities
{
    public class ScreenImage
    {
        public static readonly TesseractEngine engine = new TesseractEngine(@"./tessdata", "rus", EngineMode.Default);

        /// <summary>
        /// Подготавливает изображение для распознавания, конвертируя его в черно-белый формат, обрезая и инвертируя цвета.
        /// </summary>
        /// <param name="originalImage">Исходное изображение.</param>
        /// <param name="area">Область для обрезки.</param>
        /// <returns>Подготовленное изображение.</returns>
        public static Bitmap PrepareImageForRecognition(Bitmap originalImage, Rectangle area)
        {
            var bwImage = ConvertToBlackAndWhite(originalImage); // Конвертируем изображение в черно-белый формат
            var croppedImage = CropImageArea(bwImage, area); // Обрезаем изображение 
            var invImage = new Bitmap(croppedImage);
            invImage = InvertColors(invImage); // Инвертируем изображение в бело-черный формат
            return invImage;
        }

        /// <summary>
        /// Распознает цифры из изображения.
        /// </summary>
        /// <param name="image">Изображение для распознавания.</param>
        /// <returns>Распознанный текст.</returns>
        public static string RecognizeNumericFromImage(Bitmap image)
        {
            using (Tesseract.Page page = engine.Process(image, PageSegMode.SingleLine))
            {
                return page.GetText().Trim().Replace("\n", "");
            }
        }

        /// <summary>
        /// Распознает текст из изображения.
        /// </summary>
        /// <param name="image">Изображение для распознавания.</param>
        /// <returns>Распознанный текст.</returns>
        public static string RecognizeTextFromImage(Bitmap image)
        {
            using (Tesseract.Page page = engine.Process(image, PageSegMode.SparseText))
            {
                return page.GetText().Trim().Replace("\n", "");
            }
        }

        /// <summary>
        /// Дополнительно распознает цифры из изображения с повышением масштаба.
        /// </summary>
        /// <param name="image">Изображение для распознавания.</param>
        /// <param name="originalText">Исходный распознанный текст.</param>
        /// <returns>Распознанный текст.</returns>
        public static string ReRecognizeNumeric(Bitmap image, string originalText)
        {
            for (int i = 1; i < 3; i++)
            {
                int scaleFactor = i + 1;
                image = ScaleImage(image, scaleFactor);
                string recognizedText = RecognizeNumericFromImage(image);
                if (int.TryParse(recognizedText, out int value))
                    return $"Количество шагов: {recognizedText}. Распознано со {i + 1}-й попытки";
            }
            return $"Ошибка приведения {originalText}";
        }

        /// <summary>
        /// Преобразует изображение в черно-белый формат.
        /// </summary>
        /// <param name="image">Исходное изображение.</param>
        /// <returns>Черно-белое изображение.</returns>
        public static Bitmap ConvertToBlackAndWhite(Bitmap image) =>
            image.Clone(new Rectangle(0, 0, image.Width, image.Height), PixelFormat.Format1bppIndexed);

        /// <summary>
        /// Инвертирует цвета изображения.
        /// </summary>
        /// <param name="image">Исходное изображение.</param>
        /// <returns>Изображение с инвертированными цветами.</returns>
        private static Bitmap InvertColors(Bitmap image)
        {
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color inv = image.GetPixel(x, y);
                    inv = Color.FromArgb(inv.A, (255 - inv.R), (255 - inv.G), (255 - inv.B));
                    image.SetPixel(x, y, inv);
                }
            }
            return image;
        }

        /// <summary>
        /// Обрезает изображение по указанной области.
        /// </summary>
        /// <param name="image">Исходное изображение.</param>
        /// <param name="area">Область для обрезки.</param>
        /// <returns>Обрезанное изображение.</returns>
        private static Bitmap CropImageArea(Bitmap image, Rectangle area) =>
            image.Clone(area, PixelFormat.DontCare);

        /// <summary>
        /// Увеличивает масштаб изображения.
        /// </summary>
        /// <param name="image">Исходное изображение.</param>
        /// <param name="scaleFactor">Фактор масштабирования.</param>
        /// <returns>Увеличенное изображение.</returns>
        private static Bitmap ScaleImage(Bitmap image, int scaleFactor) =>
            new Bitmap(image, new System.Drawing.Size(image.Width * scaleFactor, image.Height * scaleFactor));


        /// <summary>
        /// Обрабатывает список скриншотов, извлекая уникальные скриншоты и распознавая текст.
        /// </summary>
        /// <param name="screenshotList">Список скриншотов.</param>
        /// <param name="uniqueScreenshots">Словарь, содержащий уникальные скриншоты.</param>
        /// <param name="recognizedTextBuilder">Строковый конструктор для распознанного текста.</param>
        public static void ProcessScreenshots(List<byte[]> screenshotList, Dictionary<string, byte[]> uniqueScreenshots, StringBuilder recognizedTextBuilder)
        {
            foreach (var screenshotBytes in screenshotList)
            {
                using (MemoryStream ms = new MemoryStream(screenshotBytes))
                {
                    using (Bitmap originalImage = new Bitmap(ms))
                    {
                        ProcessSingleScreenshot(originalImage, uniqueScreenshots, recognizedTextBuilder);
                    }
                }
            }
        }

        /// <summary>
        /// Обрабатывает отдельный скриншот, извлекая уникальный скриншот и распознавая текст.
        /// </summary>
        /// <param name="originalImage">Исходное изображение.</param>
        /// <param name="uniqueScreenshots">Словарь, содержащий уникальные скриншоты.</param>
        /// <param name="recognizedTextBuilder">Строковый конструктор для распознанного текста.</param>
        private static void ProcessSingleScreenshot(Bitmap originalImage, Dictionary<string, byte[]> uniqueScreenshots, StringBuilder recognizedTextBuilder)
        {
            Rectangle cropRect = new Rectangle(0, 670, 370, 30);
            Bitmap croppedImage = originalImage.Clone(cropRect, originalImage.PixelFormat);

            using (MemoryStream croppedMs = new MemoryStream())
            {
                croppedImage.Save(croppedMs, System.Drawing.Imaging.ImageFormat.Png);
                string hash = GetImageHash(croppedMs);

                if (!uniqueScreenshots.ContainsKey(hash))
                {
                    uniqueScreenshots.Add(hash, croppedMs.ToArray());
                    RecognizeTextAndBuild(croppedImage, recognizedTextBuilder);
                }
            }
        }

        /// <summary>
        /// Вычисляет хэш изображения.
        /// </summary>
        private static string GetImageHash(MemoryStream ms)
        {
            string hash = BitConverter.ToString(new SHA1CryptoServiceProvider().ComputeHash(ms.ToArray()));
            return hash;
        }

        /// <summary>
        /// Распознает текст на изображении и добавляет его в строковый конструктор.
        /// </summary>
        /// <param name="croppedImage">Обрезанное изображение.</param>
        /// <param name="recognizedTextBuilder">Строковый конструктор для распознанного текста.</param>
        private static void RecognizeTextAndBuild(Bitmap croppedImage, StringBuilder recognizedTextBuilder)
        {
            croppedImage = new Bitmap(croppedImage, new System.Drawing.Size(croppedImage.Width * 3, croppedImage.Height * 3));
            string recognizedText = RecognizeTextFromImage(croppedImage);
            recognizedTextBuilder.AppendLine(recognizedText);
        }

        /// <summary>
        /// Сохраняет распознанный текст в файл.
        /// </summary>
        /// <param name="recognizedTextBuilder">Строковый конструктор для распознанного текста.</param>
        /// <param name="screenFileFolder">Папка для сохранения файла.</param>
        public static void SaveRecognizedText(StringBuilder recognizedTextBuilder, string screenFileFolder)
        {
            string textFilePath = Path.Combine(Environment.RESOURCES_FOLDER, "recognized_text.txt");
            File.WriteAllText(textFilePath, recognizedTextBuilder.ToString());
        }

        /// <summary>
        /// Сравнивает распознанный текст с эталонным текстом.
        /// </summary>
        /// <param name="file1">Путь к файлу с распознанным текстом.</param>
        /// <param name="file2">Путь к файлу с эталонным текстом.</param>
        /// <returns>True, если распознанный текст совпадает с эталонным, иначе false.</returns>
        public static bool CompareTextWithEtalon(string file1, string file2)
        {
            List<string> lines1 = File.ReadAllLines(file1).ToList();
            List<string> lines2 = File.ReadAllLines(file2).ToList();

            HashSet<string> lines2Set = new HashSet<string>(lines2);

            int matchingLinesCount = 0;
            foreach (string line in lines1)
            {
                if (lines2Set.Contains(line))
                {
                    matchingLinesCount++;
                    if (matchingLinesCount >= 2)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Сравнивает скриншоты с эталонными.
        /// </summary>
        /// <param name="screenFileFolder">Папка со скриншотами.</param>
        /// <returns>True, если все скриншоты совпадают с эталонными, иначе false.</returns>
        private bool CompareScreenshotsWithEtalons(string screenFileFolder)
        {
            string etalonFolder = "D:\\Work\\Auto\\Screenshots\\etalon";
            int matchingCount = 0;

            if (Directory.Exists(etalonFolder))
            {
                var etalonFiles = Directory.GetFiles(etalonFolder);
                var screenshotFiles = Directory.GetFiles(screenFileFolder);

                if (screenshotFiles.Length != 0)
                {
                    foreach (var screenshot in screenshotFiles)
                    {
                        matchingCount += CompareWithEtalons(screenshot, etalonFiles);
                    }
                }

                if (matchingCount == screenshotFiles.Length)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Подсчитывает количество изображений в папке, совпадающих с изображениями в папке с эталонами.
        /// </summary>
        /// <param name="uniqueScreenshot">Путь к уникальному скриншоту.</param>
        /// <param name="etalonFiles">Массив путей к эталонным изображениям.</param>
        /// <returns>Количество совпадающих изображений.</returns>
        private int CompareWithEtalons(string uniqueScreenshot, string[] etalonFiles)
        {
            int matchingCount = 0;
            foreach (var etalonFile in etalonFiles)
            {
                if (CompareImages(etalonFile, uniqueScreenshot))
                    matchingCount++;
            }
            return matchingCount;
        }

        /// <summary>
        /// Сравнивает два изображения.
        /// </summary>
        /// <param name="img1">Путь к первому изображению.</param>
        /// <param name="img2">Путь ко второму изображению.</param>
        /// <returns>True, если изображения совпадают, иначе false.</returns>
        private bool CompareImages(string img1, string img2)
        {
            Mat image1 = Cv2.ImRead(img1, ImreadModes.Grayscale);
            Mat image2 = Cv2.ImRead(img2, ImreadModes.Grayscale);

            Mat diff = new Mat();
            Cv2.Absdiff(image1, image2, diff);

            Scalar diffMean = Cv2.Mean(diff);

            if (diffMean.Val0 < 1)
                return true;

            return false;
        }


    }
}
