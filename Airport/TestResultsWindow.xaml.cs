using Airport.Model;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Airport
{
    /// <summary>
    /// Interaction logic for TestResultsWindow.xaml
    /// </summary>
    public partial class TestResultsWindow : Window
    {
        private AppContext _appContext;
        private List<ExcelModelTestResultsOfEmployeesList> _excelModelTestResultsOfEmployeesLists;
        private string _testName;
        private bool _isFinalTest;

        public TestResultsWindow(bool isFinalTest, AppContext appContext, string testName, List<ExcelModelTestResultsOfEmployeesList> excelModelTestResultsOfEmployeesLists)
        {
            InitializeComponent();
            _appContext = appContext;
            _excelModelTestResultsOfEmployeesLists = excelModelTestResultsOfEmployeesLists;
            _testName = testName;
            testTitle.Content = testName;
            _isFinalTest = isFinalTest;
            PopulateDataGrid();

        }

        private void PopulateDataGrid()
        {
            foreach (ExcelModelTestResultsOfEmployeesList item in _excelModelTestResultsOfEmployeesLists)
            {
                dataGrid.Items.Add(item);
            }
        }

        private void CreateExcelFileButtonClick(object sender, RoutedEventArgs e)
        {
            ExcelBuilder.CreateExcelFileButtonClick(_testName, _excelModelTestResultsOfEmployeesLists, true, "Сотрудник", "Отметка обучения", "Дата прохождения"
                , "Результат", "Количество вопросов", "Количество верных ответов", "Допуск");
        }

        private void ShowEmployeeAnswersButtonClick(object sender, RoutedEventArgs e)
        {
            WindowCreator.ShowAnswersOfSelectedTestViaMenuClickOnTestResultsDataGrid(sender, _appContext, _isFinalTest, this);
        }

    }
}
