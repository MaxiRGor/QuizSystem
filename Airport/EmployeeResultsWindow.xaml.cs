using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Airport.Model;

namespace Airport
{
    /// <summary>
    /// Interaction logic for EmployeeResultsWindow.xaml
    /// </summary>
    public partial class EmployeeResultsWindow : Window
    {

        private AppContext _appContext;
        private List<ExcelModeTestResultOfDistinctEmployee> _excelModeTestResultOfDistinctEmployee;
        private string _emoloyeeName;


        public EmployeeResultsWindow(AppContext appContext, string emoloyeeName, List<ExcelModeTestResultOfDistinctEmployee> excelModeTestResultOfDistinctEmployee)
        {
            InitializeComponent();
            _appContext = appContext;
            _emoloyeeName = emoloyeeName;
            _excelModeTestResultOfDistinctEmployee = excelModeTestResultOfDistinctEmployee;
            employeeNameTextButton.Content = emoloyeeName ; 
            PopulateDataGrid();

        }

        private void PopulateDataGrid()
        {
            foreach (ExcelModeTestResultOfDistinctEmployee item in _excelModeTestResultOfDistinctEmployee)
            {
                dataGrid.Items.Add(item);
            }
        }

        private void CreateExcelFileButtonClick(object sender, RoutedEventArgs e)
        {
            ExcelBuilder.CreateExcelFileButtonClick(_emoloyeeName, _excelModeTestResultOfDistinctEmployee, true, "Название теста", "Отметка обучения", "Дата прохождения"
                , "Результат", "Количество вопросов", "Количество верных ответов", "Допуск");
        }

        private void ShowEmployeeAnswersButtonClick(object sender, RoutedEventArgs e)
        {
            WindowCreator.ShowAnswersOfSelectedTestViaMenuClickOnTestResultsDataGrid(sender, _appContext, false, this);
        }


    }
}
