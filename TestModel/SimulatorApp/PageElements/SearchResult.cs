using CTCAutoTests.TestModel.SimulatorApp.PageObjects;
using System;
using static CTCAutoTests.Utilities.SimulatorBaseMethods;

namespace CTCAutoTests.TestModel.SimulatorApp.PageElements
{
    public class SearchResult
    {
        private string id;
        private string name;

        public SearchResult(string id, string name)
        {            
            this.id = id;
            this.name = name;
        }

        /// <summary>
        /// Даойной щелчок по результату поиска.
        /// </summary>
        public void DoubleClick()
        {
            SwitchAppToForeground(Simulator.simulatorName);
            Console.WriteLine($"Двойной щелчок мыши по результату поиска");
            DoubleClickElement(SearchCondition.Id, id, name);
        }

    }
}
