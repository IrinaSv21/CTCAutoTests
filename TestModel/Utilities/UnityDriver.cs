using AltTester.AltTesterUnitySDK.Driver;
using System;

namespace CTCAutoTests.Utilities
{
    public static class UnityDriver
    {
        public static AltDriver altDriver;

        // Метод инициализации AltDriver
        public static void InitAltDriver()
        {
            try
            {
                altDriver = new AltDriver("127.0.0.1", 13000, false, 60, "Trend");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        // Метод для проверки подключения к altDriver.
        public static void EnsureAltDriverConnected(int timeoutInSeconds)
        {
            int elapsedTime = 0;
            while (altDriver == null)
            {
                System.Threading.Thread.Sleep(1000); // Подождать 1 секунду
                elapsedTime++;
                if (elapsedTime > timeoutInSeconds)
                {
                    throw new Exception("altDriver не подключен после заданного таймаута");
                }
            }
        }

    }
}
