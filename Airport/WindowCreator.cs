using Airport.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Airport
{
    public static class WindowCreator
    {

        public static void ShowAnswersOfSelectedTestViaMenuClickOnTestResultsDataGrid(object sender, AppContext context, bool isFinalTest, Window ownerWindow)
        {
            object testResult = DataGridHelper.GetSelectedObject(sender);
            if (testResult != null)
            {
                if (testResult is TestResult && !isFinalTest)
                {
                    ShowEmployeeAnswersOfTestResult((TestResult)testResult, ownerWindow);
                }

                if (testResult is FinalTestResult && isFinalTest)
                {
                    ShowEmployeeAnswersOfFinalTestResult((FinalTestResult)testResult, ownerWindow);
                }

                if (testResult is ExcelModelTestResultsOfEmployeesList)
                {
                    if (isFinalTest)
                    {
                        FinalTestResult result = context.FinalTestResults.Find(((ExcelModelTestResultsOfEmployeesList)testResult).TestResultId);
                        ShowEmployeeAnswersOfFinalTestResult(result, ownerWindow);
                    }
                    else
                    {
                        TestResult result = context.TestResults.Find(((ExcelModelTestResultsOfEmployeesList)testResult).TestResultId);
                        ShowEmployeeAnswersOfTestResult(result, ownerWindow);
                    }
                }

                if(testResult is ExcelModeTestResultOfDistinctEmployee)
                {
                    TestResult result = context.TestResults.Find(((ExcelModeTestResultOfDistinctEmployee)testResult).TestResultId);
                    ShowEmployeeAnswersOfTestResult(result, ownerWindow);
                }
            }
        }

        public static void ShowFinalTestResultsViaMenuClickOnJobsGrid(object sender, AppContext context, Window ownerWindow)
        {
            object currentJob = DataGridHelper.GetSelectedObject(sender);
            if (currentJob != null && currentJob is Job)
            {
                List<FinalTestResult> testResults = context.FinalTestResults.Where(res => res.Employee.Job == currentJob).ToList();
                List<ExcelModelTestResultsOfEmployeesList> excelModelTestResultsOfEmployeesLists = new List<ExcelModelTestResultsOfEmployeesList>();
                foreach (FinalTestResult testResult in testResults)
                {
                    excelModelTestResultsOfEmployeesLists.Add(new ExcelModelTestResultsOfEmployeesList(testResult.Employee.Name, false, testResult.DatePass, testResult.Result, testResult.Answers.Count, testResult.Answers.Count - testResult.WrongAnswersCount, testResult.IsPassed, testResult.FinalTestResultId));
                }
                TestResultsWindow testResultsWindow = new TestResultsWindow(true, context, ((Job)currentJob).Title + ", " + "Итоговый тест", excelModelTestResultsOfEmployeesLists);
                testResultsWindow.Owner = ownerWindow;
                testResultsWindow.Show();
            }
        }

        public static void ShowTestResultsOfCurrentThemeViaMenuClickOnThemesDataDrid(object sender, AppContext context, Window ownerWindow)
        {
            Theme currentTheme = (Theme)DataGridHelper.GetSelectedObject(sender);
            if (currentTheme != null)
            {
                List<TestResult> testResults = context.TestResults.Where(res => res.ThemeId == currentTheme.ThemeId).ToList();
                List<ExcelModelTestResultsOfEmployeesList> excelModelTestResultsOfEmployeesLists = new List<ExcelModelTestResultsOfEmployeesList>();
                foreach (TestResult testResult in testResults)
                {
                    excelModelTestResultsOfEmployeesLists.Add(new ExcelModelTestResultsOfEmployeesList(testResult.Employee.Name, testResult.TutorialWathed, testResult.DatePass, testResult.Result, testResult.Answers.Count, testResult.Answers.Count - testResult.WrongAnswersCount, testResult.IsPassed, testResult.TestResultId));
                }
                TestResultsWindow testResultsWindow = new TestResultsWindow(false, context, currentTheme.Category.Title + ", " + currentTheme.Title, excelModelTestResultsOfEmployeesLists);
                testResultsWindow.Owner = ownerWindow;
                testResultsWindow.Show();
            }
        }

        internal static void ShowTestResultsViaMenuClickOnEmoployeeDataDrid(object sender, AppContext context, Window ownerWindow)
        {
            object employee = DataGridHelper.GetSelectedObject(sender);
            if (employee != null && employee is Employee)
            {
                List<TestResult> testResults = context.TestResults.Where(res => res.EmployeeId == ((Employee)employee).EmployeeId).ToList();
                List<ExcelModeTestResultOfDistinctEmployee> excelModeTestResultOfDistinctEmployee = new List<ExcelModeTestResultOfDistinctEmployee>();
                foreach (TestResult testResult in testResults)
                {
                    excelModeTestResultOfDistinctEmployee.Add(new ExcelModeTestResultOfDistinctEmployee(testResult.Theme.Title, testResult.TutorialWathed, testResult.DatePass, testResult.Result, testResult.Answers.Count, testResult.Answers.Count - testResult.WrongAnswersCount, testResult.IsPassed, testResult.TestResultId));
                }

                EmployeeResultsWindow employeeResultsWindow = new EmployeeResultsWindow(context, ((Employee)employee).Name, excelModeTestResultOfDistinctEmployee);
                employeeResultsWindow.Owner = ownerWindow;
                employeeResultsWindow.Show();
            }
        }

        private static void ShowEmployeeAnswersOfTestResult(TestResult testResult, Window ownerWindow)
        {
            List<ExcelModelAnswerOfEmployee> excelModelAnswersOfEmployee = new List<ExcelModelAnswerOfEmployee>();
            foreach (UserAnswer answer in testResult.Answers)
            {
                string rightAnswer = "Ответ " + answer.RightAnswerInt + ". ";
                string userAnswer = "Ответ " + answer.UserAnswerInt + ". ";

                rightAnswer = CompleteAccoardingWithAnswerType(answer.Question.IsWithAnswerImage, answer.RightAnswerInt, rightAnswer, answer.Question);
                userAnswer = CompleteAccoardingWithAnswerType(answer.Question.IsWithAnswerImage, answer.UserAnswerInt, userAnswer, answer.Question);

                excelModelAnswersOfEmployee.Add(new ExcelModelAnswerOfEmployee(answer.QuestionText, rightAnswer, userAnswer, answer.RightAnswerInt == answer.UserAnswerInt));
            }

            AnswersWindow answerWindow = new AnswersWindow(testResult.Employee.Name + ", " + testResult.Theme.Title, excelModelAnswersOfEmployee);
            answerWindow.Owner = ownerWindow;
            answerWindow.Show();
        }

        private static void ShowEmployeeAnswersOfFinalTestResult(FinalTestResult finalTestResult, Window ownerWindow)
        {
            List<ExcelModelAnswerOfEmployee> excelModelAnswersOfEmployee = new List<ExcelModelAnswerOfEmployee>();
            foreach (FinalTestResultUserAnswer answer in finalTestResult.Answers)
            {
                string rightAnswer = "Ответ " + answer.RightAnswerInt + ". ";
                string userAnswer = "Ответ " + answer.UserAnswerInt + ". ";

                rightAnswer = CompleteAccoardingWithAnswerType(answer.Question.IsWithAnswerImage, answer.RightAnswerInt, rightAnswer, answer.Question);
                userAnswer = CompleteAccoardingWithAnswerType(answer.Question.IsWithAnswerImage, answer.UserAnswerInt, userAnswer, answer.Question);

                excelModelAnswersOfEmployee.Add(new ExcelModelAnswerOfEmployee(answer.QuestionText, rightAnswer, userAnswer, answer.RightAnswerInt == answer.UserAnswerInt));
            }

            AnswersWindow answerWindow = new AnswersWindow(finalTestResult.Employee.Name + ", Итоговый тест", excelModelAnswersOfEmployee);
            answerWindow.Owner = ownerWindow;
            answerWindow.Show();
        }


        private static string CompleteAccoardingWithAnswerType(bool isWithAnswerImage, int answerInt, string answerString, Question question)
        {
            if (isWithAnswerImage)
            {
                switch (answerInt)
                {
                    case 1:
                        answerString += question.Image1Path;
                        break;
                    case 2:
                        answerString += question.Image2Path;
                        break;
                    case 3:
                        answerString += question.Image3Path;
                        break;
                    case 4:
                        answerString += question.Image4Path;
                        break;
                    case 5:
                        answerString += question.Image5Path;
                        break;
                    case 6:
                        answerString += question.Image6Path;
                        break;
                }
            }
            else
            {
                switch (answerInt)
                {
                    case 1:
                        answerString += question.Answer1;
                        break;
                    case 2:
                        answerString += question.Answer2;
                        break;
                    case 3:
                        answerString += question.Answer3;
                        break;
                    case 4:
                        answerString += question.Answer4;
                        break;
                    case 5:
                        answerString += question.Answer5;
                        break;
                    case 6:
                        answerString += question.Answer6;
                        break;
                }
            }
            return answerString;
        }


    }
}
