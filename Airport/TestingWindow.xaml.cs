using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for TestingWindow.xaml
    /// </summary>
    public partial class TestingWindow : Window
    {
        ObservableCollection<Employee> employees;
        ObservableCollection<Theme> themes;

        private CollectionViewSource employeesViewSource;
        private CollectionViewSource themesViewSource;

        public TestingWindow(List<Employee> selectedEmployees, List<Theme> selectedThemes)
        {
            InitializeComponent();
            employees = new ObservableCollection<Employee>(selectedEmployees);
            themes = new ObservableCollection<Theme>(selectedThemes);

            employeesViewSource = (CollectionViewSource)FindResource(nameof(employeesViewSource));
            themesViewSource = (CollectionViewSource)FindResource(nameof(themesViewSource));

            employeesViewSource.Source = employees;
            themesViewSource.Source = themes;
        }

        private void StartTest(object sender, RoutedEventArgs e)
        {
            if ((bool)isTutorialNeeded.IsChecked)
            {
                try
                {
                    var tutorialTextBlock = themesDataGrid.Columns[3].GetCellContent(themesDataGrid.SelectedCells[0].Item) as TextBlock;

                    new Process
                    {
                        StartInfo = new ProcessStartInfo(tutorialTextBlock.Text)
                        {
                            UseShellExecute = true
                        }
                    }.Start();
                }
                catch
                {
                    MessageBox.Show("Файл с обучением не задан");
                }
            }

            try
            {
                 

                var themeRow = themesDataGrid.SelectedCells[0];
                var themeIdTextBlock = themesDataGrid.Columns[0].GetCellContent(themeRow.Item) as TextBlock;
                int themeId = int.Parse(themeIdTextBlock.Text);
                Theme theme = themes.Single(t => t.ThemeId == themeId);

                try
                {
                    if (theme.Questions.ToList().Count == 0)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
                catch
                {
                    MessageBox.Show("Количество вопросов в данном тесте равно нулю!");
                    return;
                }


                var employeeRow = employeesDataGrid.SelectedCells[0];
                var employeeIdTextBlock = employeesDataGrid.Columns[0].GetCellContent(employeeRow.Item) as TextBlock;
                int employeeId = int.Parse(employeeIdTextBlock.Text);
                Employee employee = employees.Single(emp => emp.EmployeeId == employeeId);

                DistinctTestWindow distinctTestWindow = new DistinctTestWindow(employee, theme, theme.Questions.ToList(), (bool)isTutorialNeeded.IsChecked);
                distinctTestWindow.Owner = this;
                distinctTestWindow.Show();
            }
            catch
            {
                MessageBox.Show("Тест или сотрудник не выбран");
            }
        }
    }
}
