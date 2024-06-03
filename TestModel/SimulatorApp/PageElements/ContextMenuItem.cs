using CTCAutoTests.Utilities;
using System;

namespace CTCAutoTests.TestModel.SimulatorApp.PageElements
{
    public class ContextMenuItem
    {
        private string name;
        public SubMenuItem @object;

        public ContextMenuItem(string name)
        {            
            this.name = name;
            @object = new SubMenuItem("объект");
        }

        // Метод щелчка мышью по пункту меню
        public void Click()
        {
            Console.WriteLine($"Выбор пункта меню \"{name}\"");
            SimulatorBaseMethods.ClickElement(name, ContextMenu.menuSession);
        }
    }
}
