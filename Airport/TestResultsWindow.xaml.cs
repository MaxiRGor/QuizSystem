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
        private List<ExcelModelTestResultsOfEmployeesList> _excelModelTestResultsOfEmployeesLists;
        private string _testName;

        public TestResultsWindow(string testName, List<ExcelModelTestResultsOfEmployeesList> excelModelTestResultsOfEmployeesLists)
        {
            InitializeComponent();
            _excelModelTestResultsOfEmployeesLists = excelModelTestResultsOfEmployeesLists;
            _testName = testName;
            testTitle.Content = testName;
            PopulateDataGrid();

        }

        private void PopulateDataGrid()
        {
           foreach(ExcelModelTestResultsOfEmployeesList item in _excelModelTestResultsOfEmployeesLists)
            {
                dataGrid.Items.Add(item);
            }
        }

        private void CreateExcelFileButtonClick(object sender, RoutedEventArgs e)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var p = new ExcelPackage())
            {
                var ws = p.Workbook.Worksheets.Add("Sheet1");

                ws.Cells["A1"].Value = _testName;
                ws.Cells["A1"].Style.Font.Size = 24;
                ws.Cells["A1"].Style.Font.Bold = true;

                ws.Cells["A2"].Value = "Сотрудник";
                ws.Cells["B2"].Value = "Отметка обучения";
                ws.Cells["C2"].Value = "Дата прохождения";
                ws.Cells["D2"].Value = "Результат";
                ws.Cells["E2"].Value = "Допуск";

                ws.Cells["A1:E1"].Merge = true;
                ws.Cells["A2:E2"].Style.Font.Bold = true;

                ws.Cells[3, 1].LoadFromCollection(_excelModelTestResultsOfEmployeesLists);
                SaveAndStartExcelDocument(ws, p);

            }
        }

        private void SaveAndStartExcelDocument(ExcelWorksheet ws, ExcelPackage p)
        {
            ws.Cells.AutoFitColumns();

            string directory = "excelData";
            Directory.CreateDirectory(directory);
            string fileName = System.IO.Path.GetFileName(DateTime.Now.Ticks + ".xlsx");
            fileName = System.IO.Path.Combine(directory, fileName);
            p.SaveAs(new FileInfo(fileName));

            new Process
            {
                StartInfo = new ProcessStartInfo(fileName)
                {
                    UseShellExecute = true
                }
            }.Start();
        }
    }
}
