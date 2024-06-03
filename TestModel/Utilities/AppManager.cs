//123456
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CTCAutoTests.TestModel.SimulatorApp.PageObjects;

namespace CTCAutoTests.Utilities
{
    public class AppManager
    {
        private static readonly StringDictionary _procNames = new StringDictionary
        {
            { Environment.SIMULATOR_NAME, Environment.SIMULATOR_PROC_NAME },
        };

        public static StringDictionary appPaths = new StringDictionary
        {
            { Environment.WINAPPDRIVER_NAME, Environment.WINAPPDRIVER_PATH},
            { Environment.SIMULATOR_NAME, Environment.SIMULATOR_PATH_1},
            { Environment.TREN3D_NAME, Environment.TREN3D_PATH },
            { Environment.ALTTESTERDESKTOP_NAME, Environment.ALTTESTERDESKTOP_PATH},
        };
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        public static Dictionary<string, WindowsDriver<WindowsElement>> Sessions = new Dictionary<string, WindowsDriver<WindowsElement>>();
        public static string AppName;
        public static string appPath;

        /// <summary>
        /// Метод для завершения всех процессов приложения.
        /// </summary>
        /// <param name="appName">Имя приложения.</param>
        public static void KillProcessesByName(string appName)
        {
            var processName = GetProcessName(appName); // Получаем имя процесса
            var processes = GetProcessesByName(processName); // Находим процесс

            foreach (var process in processes)
                KillTree(process.Id); // Закрываем процесс и все его дочерние процессы
        }

        // Метод для получения имени процесса.
        private static string GetProcessName(string appName) =>
        _procNames[appName] ?? appName;

        // Получает массив процессов по указанному имени.
        private static Process[] GetProcessesByName(string processName) =>
            Process.GetProcesses().Where(pr => pr.ProcessName == processName).ToArray();

        // Завершает указанный процесс и все его дочерние процессы.
        private static void KillTree(int processId)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("taskkill", "/T /F /PID " + processId)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
            process.WaitForExit();
            process.Close();
        }

        /// <summary>
        /// Запускает указанное приложение.
        /// </summary>
        /// <param name="appName">Имя приложения, которое нужно запустить.</param>
        public static void StartAppProcess(string appName)
        {
            Console.WriteLine($"Запуск {appName}");
            try
            {
                Process.Start(GetAppPath(appName));
                Console.WriteLine($"{appName} запущен");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка при запуске {appName}: {ex.Message}");
            }
        }

        /// <summary>
        /// Получает путь к указанному приложению.
        /// </summary>
        /// <param name="appName">Имя приложения, путь к которому нужно получить.</param>
        /// <returns>Путь к указанному приложению.</returns>
        public static string GetAppPath(string appName) =>
            appPaths[appName] ?? appName;

        /// <summary>
        /// Запускает указанное приложение.
        /// </summary>
        /// <param name="appName">Имя приложения, которое необходимо запустить.</param>
        public static void LaunchApp(string appName)
        {
            // Устанавливаем имя приложения и получаем его путь
            SetAppNameAndPath(appName);

            // Обрабатываем специальный случай для приложения "Tren3D_LABUPO_KF"
            HandleSpecialCaseForTren3D(appName);

            try
            {
                // Закрываем приложение, если оно было запущено
                ShutdownRunningApp(appName);

                // Запускаем WinAppDriver
                StartWinAppDriver();

                // Создаем сессию приложения, устанавливаем время ожидания и добавляем сессию в словарь
                CreateAndAddAppSession(appName);

                // Проверяем, запущено ли приложение
                CheckIfAppIsRunning(appName);
            }
            catch (Exception ex)
            {
                // Обрабатываем исключение
                HandleAppLaunchError(ex, appName);
            }
        }

        // Метод устанавливает имя приложения и получает его путь.
        private static void SetAppNameAndPath(string appName)
        {
            AppName = appName;
            appPath = GetAppPath(appName);
            if (appPath == null)
                throw new ArgumentException("Не удалось определить URL для указанного приложения");
        }

        // Метод обрабатывает специальный случай для приложения "Tren3D_LABUPO_KF".
        private static void HandleSpecialCaseForTren3D(string appName)
        {
            if (appName == Environment.TREN3D_NAME)
            {
                KillProcessesByName(Environment.ALTTESTERDESKTOP_NAME);
                StartAppProcess(Environment.ALTTESTERDESKTOP_NAME);
            }
        }

        // Метод закрывает запущенное приложение.
        private static void ShutdownRunningApp(string appName) =>
            KillProcessesByName(appName);

        // Метод для запуска WinAppDriver
        private static void StartWinAppDriver() =>
            StartAppProcess(Environment.WINAPPDRIVER_NAME);

        // Метод для создания сессии приложения, установки времени ожидания и добавления сессии в словарь
        private static void CreateAndAddAppSession(string appName)
        {
            Console.WriteLine($"Запуск приложения \"{appName}\"");
            var appSession = CreateWinAppSession("app", appPath);
            SetImplicitWaitTime(appSession, TimeSpan.FromSeconds(30));
            if (Sessions.ContainsKey(appName))
                Sessions.Remove(appName);
            Sessions.Add(appName, appSession);
        }

        // Метод для проверки, запущено ли приложение
        private static void CheckIfAppIsRunning(string appName)
        {
            var appSession = Sessions[appName];
            if (!IsAppRunning(appSession, appName))
                throw new ApplicationException($"Не удалось запустить приложение \"{appName}\"");
            Console.WriteLine($"Приложение \"{appName}\" успешно запущено");
        }

        // Метод для обработки ошибки при запуске приложения
        private static void HandleAppLaunchError(Exception ex, string appName)
        {
            Console.WriteLine($"Произошла ошибка при запуске приложения: {ex.Message}");
            throw new ApplicationException($"Произошла ошибка при запуске приложения \"{appName}\": {ex.Message}");
        }

        /// <summary>
        /// Метод создает сессию WinAppDriver для указанного приложения.
        /// </summary>
        /// <param name="capabilityName">Название возможности, которая будет добавлена к возможностям сессии.</param>
        /// <param name="capabilityValue">Значение возможности, которая будет добавлена к возможностям сессии.</param>
        /// <returns>Объект `WindowsDriver<WindowsElement>`, представляющий созданную сессию.</returns>
        public static WindowsDriver<WindowsElement> CreateWinAppSession(string capabilityName, string capabilityValue)
        {
            // Получаем путь к приложению
            appPath = GetAppPath(capabilityValue);

            // Создаем новые возможности и добавляем к ним необходимые для приложения
            var appCapabilities = new AppiumOptions();
            appCapabilities.AddAdditionalCapability(capabilityName, capabilityValue);
            appCapabilities.AddAdditionalCapability("deviceName", "WindowsPC");

            // Пауза, чтобы убедиться, что WinAppDriver запущен и может принимать соединения
            Thread.Sleep(1000);

            // Создаем новую сессию WindowsDriver с заданными возможностями и возвращаем ее
            var winSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);

            // Проверяем, что сессия успешно создана
            Assert.IsNotNull(winSession);

            return winSession;
        }

        // Метод для установки времени ожидания
        private static void SetImplicitWaitTime(WindowsDriver<WindowsElement> appSession, TimeSpan waitTime)
        {
            try
            {
                // Устанавливаем время ожидания
                appSession.Manage().Timeouts().ImplicitWait = waitTime;
            }
            catch (Exception ex)
            {
                // Обрабатываем исключение и выводим сообщение об ошибке
                Console.WriteLine($"Произошла ошибка при установке времени ожидания: {ex.Message}");
            }
        }

        // Метод для проверки, запущено ли приложение
        private static bool IsAppRunning(WindowsDriver<WindowsElement> appSession, string appName)
        {
            try
            {
                // Ищем элементы с именем приложения и проверяем их наличие
                bool appRunning = appSession.FindElementsByName(appName).Count > 0;
                return appRunning;
            }
            catch (Exception ex)
            {
                // Обрабатываем исключение и выводим сообщение об ошибке
                Console.WriteLine($"Произошла ошибка при проверке, что приложение \"{appName}\" запущено: {ex.Message}");
                return false;
            }
        }

        // Метод для закрытия приложения и удаления сессий после прохождения теста
        public void TearDown()
        {
            if (Simulator.AppSession != null)
            {
                SimulatorPage.startButton.ClickButton();
                SimulatorPage.okButton.ClickButton();
                Simulator.AppSession.Quit();
                Simulator.AppSession = null;
            }
            var winAppDriverProcesses = Process.GetProcesses().
            Where(pr => pr.ProcessName == Environment.WINAPPDRIVER_NAME);

            foreach (var process in winAppDriverProcesses)
            {
                process.Kill();
            }
        }
    }
}
