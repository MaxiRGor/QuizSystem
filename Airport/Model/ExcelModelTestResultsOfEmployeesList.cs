using System;
using System.Collections.Generic;
using System.Text;

namespace Airport.Model
{
    public class ExcelModelTestResultsOfEmployeesList
    {
        public ExcelModelTestResultsOfEmployeesList()
        {
        }

        public ExcelModelTestResultsOfEmployeesList(string employeeName, bool tutorialMark, DateTime date, int result, bool isPassed)
        {
            EmployeeName = employeeName;
            TutorialMark = tutorialMark ? "Да" : "";
            Date = date.ToString("dd.MM.yyyy");
            Result = result;
            IsPassed = isPassed ? "Да" : "Нет";
        }

        public string EmployeeName { get; set; }
        public string TutorialMark { get; set; }
        public string Date { get; set; }
        public int Result { get; set; }
        public string IsPassed { get; set; }
    }
}
