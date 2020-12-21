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
    /// Interaction logic for AnswersWindow.xaml
    /// </summary>
    public partial class AnswersWindow : Window
    {
        private List<ExcelModelAnswerOfEmployee> _excelModelAnswersOfEmployee;
        private string _testName;

        public AnswersWindow(string testName, List<ExcelModelAnswerOfEmployee> excelModelAnswersOfEmployee)
        {
            InitializeComponent();
            _excelModelAnswersOfEmployee = excelModelAnswersOfEmployee;
            _testName = testName;
            testTitle.Content = testName;
            PopulateDataGrid();

        }

        private void PopulateDataGrid()
        {
            foreach (ExcelModelAnswerOfEmployee item in _excelModelAnswersOfEmployee)
            {
                dataGrid.Items.Add(item);
            }
        }

        private void CreateExcelFileButtonClick(object sender, RoutedEventArgs e)
        {
            ExcelBuilder.CreateExcelFileButtonClick(_testName, _excelModelAnswersOfEmployee, false, "Ответ", "Правильный ответ", "Данный ответ", "Результат");
        }

    }
}