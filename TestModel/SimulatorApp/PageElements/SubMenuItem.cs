using CTCAutoTests.TestModel.SimulatorApp.PageObjects;
using CTCAutoTests.Utilities;
using System;

namespace CTCAutoTests.TestModel.SimulatorApp.PageElements
{
    public class SubMenuItem
    {
        private string name;

        public SubMenuItem(string name)
        {            
            this.name = name;
        }

        /// <summary>
        /// Щелчок мышью по пункту меню.
        /// </summary>
        public void Click()
        {
            Console.WriteLine($"Выбор подпункта меню \"{name}\"");
            SimulatorBaseMethods.ClickElement(name, Simulator.AppSession);
        }
    }
}
