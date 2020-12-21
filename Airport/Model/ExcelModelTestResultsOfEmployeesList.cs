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

        public ExcelModelTestResultsOfEmployeesList(string employeeName, bool tutorialMark, DateTime date, int result, int numberOfQuestions, int numberOfAnswered, bool isPassed, int testResultId)
        {
            EmployeeName = employeeName;
            TutorialMark = tutorialMark ? "Да" : "";
            Date = date.ToString("dd.MM.yyyy");
            Result = result;
            IsPassed = isPassed ? "Да" : "Нет";
            NumberOfQuestions = numberOfQuestions;
            NumberOfAnswered = numberOfAnswered;
            TestResultId = testResultId;
        }

        public string EmployeeName { get; set; }
        public string TutorialMark { get; set; }
        public string Date { get; set; }
        public int Result { get; set; }

        public int NumberOfQuestions { get; set; }
        public int NumberOfAnswered { get; set; }
        public string IsPassed { get; set; }
        public int TestResultId { get; set; }
    }
}
