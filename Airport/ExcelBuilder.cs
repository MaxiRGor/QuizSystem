using Airport.Model;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Airport
{
    public static class ExcelBuilder
    {

        public static void CreateExcelFileOfEmployeeTestResultsViaMenuClickOnEmployeeGrid(object sender)
        {
            object employee = DataGridHelper.GetSelectedObject(sender);

            if (employee != null && employee is Employee)
            {
                CreateExcelFileOfEmployeeTestResults((Employee)employee);
            }
        }

        public static void CreateExcelFileButtonClick<T>(string title, List<T> data, bool removeLastColumn, params string[] columnNames)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var p = new ExcelPackage())
            {
                var ws = p.Workbook.Worksheets.Add("Sheet1");

                ws.Cells["A1"].Value = title;
                ws.Cells["A1"].Style.Font.Size = 24;
                ws.Cells["A1"].Style.Font.Bold = true;

                string alphabet = "ABCDEFGHIKJLMNOPRSTUVWXYZ";
                int index = 0;

                foreach (string columnName in columnNames)
                {
                    ws.Cells[alphabet[index] + "2"].Value = columnName;
                    index++;
                }

                ws.Cells["A1:" + alphabet[index - 1] + "1"].Merge = true;
                ws.Cells["A2:" + alphabet[index - 1] + "2"].Style.Font.Bold = true;

                ws.Cells[3, 1].LoadFromCollection(data);

                if (removeLastColumn)
                {                    
                    ws.DeleteColumn(index + 1);
                }

                SaveAndStartExcelDocument(ws, p);
            }
        }

        private static void CreateExcelFileOfEmployeeTestResults(Employee employee)
        {
            List<ExcelModeTestResultOfDistinctEmployee> excelModeTestResults = new List<ExcelModeTestResultOfDistinctEmployee>();
            foreach (TestResult testResult in employee.TestResults)
            {
                excelModeTestResults.Add(new ExcelModeTestResultOfDistinctEmployee(testResult.Theme.Title, testResult.TutorialWathed, testResult.DatePass, testResult.Result, testResult.Answers.Count, testResult.Answers.Count - testResult.WrongAnswersCount, testResult.IsPassed, testResult.TestResultId));
            }

            CreateExcelFileButtonClick(employee.Name + ", " + employee.Job.Title, excelModeTestResults, false, "Название теста", "Отметка обучения",
                "Дата", "Результат", "Количество вопросов", "Количество верных ответов", "Допуск");
        }

        private static void SaveAndStartExcelDocument(ExcelWorksheet ws, ExcelPackage p)
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
