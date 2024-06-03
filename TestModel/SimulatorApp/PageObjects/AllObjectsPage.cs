using CTCAutoTests.TestModel.SimulatorApp.PageElements;

namespace CTCAutoTests.TestModel.SimulatorApp.PageObjects
{
    public class AllObjectsPage
    {
        public static Button exclamationButton;
        public static SearchField searchFieldElement;
        public static SearchResult searchResultItem;

        public AllObjectsPage()
        {
            exclamationButton = new Button("1002", "!");
            searchFieldElement = new SearchField("1004", "Поиск");
            searchResultItem = new SearchResult("ListViewItem-0", "Результат поиска");
        }

        /// <summary>
        /// Заполняет поле поиска.
        /// </summary>
        /// <param name="searchText">Текст для ввода в поле поиска.</param>
        public static void FillSearchField(string searchText) =>
            searchFieldElement.Fill(searchText);
    }
}
