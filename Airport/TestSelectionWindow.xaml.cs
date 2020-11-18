using Airport.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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
    /// Interaction logic for TestSelectionWindow.xaml
    /// </summary>
    public partial class TestSelectionWindow : Window
    {

        private AirportService _currentService;
        private readonly AppContext _context = new AppContext();

        private CollectionViewSource servicesViewSource;


        public TestSelectionWindow(AirportService currentService)
        {

            InitializeComponent();
            _currentService = currentService;
            servicesViewSource = (CollectionViewSource)FindResource(nameof(servicesViewSource));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            _context.Database.EnsureCreated();
            _context.AirportServices.Load();
            servicesViewSource.Source = _context.AirportServices.Local.ToObservableCollection();
            servicesViewSource.Filter += ServicesViewSource_Filter;
        }

        private void ServicesViewSource_Filter(object sender, FilterEventArgs e)
        {
            AirportService service = e.Item as AirportService;
            if (service != null)
            {
                if (service.AirportServiceId == _currentService.AirportServiceId)
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
        }

        private void EmployeesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Employee employee = GetSelectedEmployee();
            if (employee != null)
            {
                if (employeesDataGrid.SelectedCells.Count == 1)
                {
                    employeeNameLabel.Content = employee.Name;
                }
                else
                {
                    employeeNameLabel.Content = "КОЛИЧЕСТВО СОТРУДНИКОВ ВЫБРАНО = " + employeesDataGrid.SelectedCells.Count;
                }

            }
            else
            {
                employeeNameLabel.Content = "СОТРУДНИК НЕ ВЫБРАН";
            }

        }

        private void ThemesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Theme theme = GetSelectedTheme();
            if ((bool)!IsTestFinalCheckBox.IsChecked)
                testTitleLabel.Content = theme == null ? "ТЕСТ НЕ ВЫБРАН" : theme.Title;
        }

        private Employee GetSelectedEmployee()
        {
            try
            {
                return (Employee)employeesDataGrid.SelectedCells[0].Item;

            }
            catch
            {
                return null;
            }
        }

        private Theme GetSelectedTheme()
        {
            try
            {
                return (Theme)themesDataGrid.SelectedCells[0].Item;

            }
            catch
            {
                return null;
            }
        }



        protected override void OnClosing(CancelEventArgs e)
        {
            _context.Dispose();
            base.OnClosing(e);

            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
        }

        private void StartTutorialButtonClick(object sender, RoutedEventArgs e)
        {
            if (EmployeeAndTestSelected())
            {
                try
                {
                    new Process
                    {
                        StartInfo = new ProcessStartInfo(GetSelectedTheme().TutorialPath)
                        {
                            UseShellExecute = true
                        }
                    }.Start();

                    SetTutorialWathed();


                }
                catch
                {
                    message.Content = "Файл с обучением не задан";
                    DialogHost.IsOpen = true;
                }
            }
        }

        private void SetTutorialWathed()
        {
            for (int i = 0; i < employeesDataGrid.SelectedCells.Count; i++)
            {
                Employee employee = (Employee)employeesDataGrid.SelectedCells[i].Item;
                _context.Employees.Find(employee.EmployeeId)
                    .TestResults.Add(new TestResult(employee.EmployeeId, employee, GetSelectedTheme().ThemeId, GetSelectedTheme(), DateTime.Now, true));
            }

            _context.SaveChanges();
        }

        private void StartTestButtonClick(object sender, RoutedEventArgs e)
        {
            if (EmployeeAndTestSelected())
            {

                if (employeesDataGrid.SelectedCells.Count == 1)
                {
                    if ((bool)IsTestFinalCheckBox.IsChecked)
                    {
                        List<Question> finalQuestions = GetFinalQuestions(GetSelectedEmployee());
                        if (finalQuestions.Count == 0)
                        {
                            message.Content = "Количество вопросов в итоговом тесте равно нулю";
                            DialogHost.IsOpen = true;
                        }
                        else
                        {

                            DistinctTestWindow distinctTestWindow = new DistinctTestWindow(GetSelectedEmployee(), null, finalQuestions);
                            distinctTestWindow.Owner = this;
                            distinctTestWindow.Show();
                        }
                    }
                    else
                    {
                        if (GetSelectedTheme().Questions.Count == 0)
                        {
                            message.Content = "Количество вопросов в выбранном тесте равно нулю";
                            DialogHost.IsOpen = true;
                        }
                        else
                        {
                            DistinctTestWindow distinctTestWindow = new DistinctTestWindow(GetSelectedEmployee(), GetSelectedTheme(), GetQuestions(GetSelectedTheme().NumberOfQuestions, GetSelectedTheme().Questions.ToList()));
                            distinctTestWindow.Owner = this;
                            distinctTestWindow.Show();
                        }

                    }
                }
                else
                {
                    message.Content = "Выберите только одного сотрудника";
                    DialogHost.IsOpen = true;
                }

            }
        }

        private List<Question> GetFinalQuestions(Employee employee)
        {
            List<Question> finalQuestions = new List<Question>();
            foreach (var category in employee.Job.Categories)
            {
                foreach (var theme in category.Themes)
                {
                    finalQuestions.AddRange(GetQuestions(theme.NumberOfFinalQuestions, theme.Questions.ToList()));
                }
            }
            finalQuestions = Shuffle(finalQuestions);
            return finalQuestions;
        }

        internal void AddFinalTestResultToEmployee(Employee employee, FinalTestResult finalTestResult)
        {
            _context.Employees.Find(employee.EmployeeId).FinalTestResults.Add(finalTestResult);
            _context.SaveChanges();
        }

        private List<Question> GetQuestions(int numberOfQuestions, List<Question> questions)
        {
            questions = Shuffle(questions);
            if (numberOfQuestions < questions.Count)
            {
                return questions.GetRange(0, numberOfQuestions);
            }
            else
            {
                return questions;
            }
        }

        private List<T> Shuffle<T>(List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }

        private bool EmployeeAndTestSelected()
        {
            if (GetSelectedTheme() != null && GetSelectedEmployee() != null)
            {
                return true;
            }
            else
            {
                message.Content = "Тест или сотрудник не выбран";
                DialogHost.IsOpen = true;
                return false;
            }
        }

        public void AddTestResultToEmployee(Employee employee, TestResult testResult, List<Model.UserAnswer> wrongAnswers)
        {
            _context.Employees.Find(employee.EmployeeId).TestResults.Add(testResult);
            _context.SaveChanges();
            TestResult testResult1 = _context.Employees.Find(employee.EmployeeId).TestResults.Last();
            foreach (var item in wrongAnswers)
            {
                testResult1.WrongAnswers.Add(new Model.UserAnswer(item.Question, item.UserAnswerInt, item.RightAnswerInt));
            }
            _context.SaveChanges();
        }

        private void IsTestFinalCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            testTitleLabel.Content = "ИТОГОВЫЙ ТЕСТ";
            startTutorialButton.IsEnabled = false;
            categoriesDataGrid.IsEnabled = false;
            themesDataGrid.IsEnabled = false;
        }

        private void IsTestFinalCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            startTutorialButton.IsEnabled = true;
            categoriesDataGrid.IsEnabled = true;
            themesDataGrid.IsEnabled = true;
            ThemesSelectionChanged(null, null);
        }


        private void ShowTestResultsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Theme currentTheme = (Theme)GetSelectedObject(sender);
                List<TestResult> testResults = _context.TestResults.Where(res => res.ThemeId == currentTheme.ThemeId).ToList();
                List<ExcelModelTestResultsOfEmployeesList> excelModelTestResultsOfEmployeesLists = new List<ExcelModelTestResultsOfEmployeesList>();
                foreach (TestResult testResult in testResults)
                {
                    excelModelTestResultsOfEmployeesLists.Add(new ExcelModelTestResultsOfEmployeesList(testResult.Employee.Name, testResult.TutorialWathed, testResult.DatePass, testResult.Result, testResult.IsPassed));
                }
                TestResultsWindow testResultsWindow = new TestResultsWindow(currentTheme.Category.Title + ", " + currentTheme.Title, excelModelTestResultsOfEmployeesLists);
                testResultsWindow.Owner = this;
                testResultsWindow.Show();

            }
            catch
            {

            }

        }

        private void ShowFinalTestResultsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Job currentJob = (Job)GetSelectedObject(sender);
                List<FinalTestResult> testResults = _context.FinalTestResults.Where(res => res.Employee.Job == currentJob).ToList();
                List<ExcelModelTestResultsOfEmployeesList> excelModelTestResultsOfEmployeesLists = new List<ExcelModelTestResultsOfEmployeesList>();
                foreach (FinalTestResult testResult in testResults)
                {
                    excelModelTestResultsOfEmployeesLists.Add(new ExcelModelTestResultsOfEmployeesList(testResult.Employee.Name, false, testResult.DatePass, testResult.Result, testResult.IsPassed));
                }
                TestResultsWindow testResultsWindow = new TestResultsWindow(currentJob.Title + ", " + "Итоговый тест", excelModelTestResultsOfEmployeesLists);
                testResultsWindow.Owner = this;
                testResultsWindow.Show();

            }
            catch
            {

            }
        }

        private object GetSelectedObject(object sender)
        {
            var menuItem = (MenuItem)sender;
            var contextMenu = (ContextMenu)menuItem.Parent;
            var item = (DataGrid)contextMenu.PlacementTarget;
            return item.SelectedCells[0].Item;
        }
    }
}
