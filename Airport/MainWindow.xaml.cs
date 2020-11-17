
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.UI.Xaml.Controls.Primitives;

namespace Airport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AirportService _currentService;
        private readonly AppContext _context = new AppContext();

        private CollectionViewSource servicesViewSource;
      //  private ObservableCollection<Job> jobs;

        public MainWindow(AirportService currentService)
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
                // Filter out products with price 25 or above
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

        private void ButtonSaveClick(object sender, RoutedEventArgs e)
        {
            _context.SaveChanges();
            RefreshGrids();

            if (questionDetails.Visibility == Visibility.Visible)
            {
                SaveQuestion();
            }
        }

        private void RefreshGrids()
        {
            // this forces the grid to refresh to latest values
            jobsDataGrid1.Items.Refresh();
            jobsDataGrid2.Items.Refresh();
            jobsDataGrid3.Items.Refresh();
            categoriesDataGrid.Items.Refresh();
            themesDataGrid.Items.Refresh();
            questionsDataGrid.Items.Refresh();
            employeesDataGrid.Items.Refresh();
            testResultsDataGrid.Items.Refresh();
        }

        private void SaveQuestion()
        {
            try
            {
                int.TryParse(questionIdTextBox.Content.ToString(), out int questionId);
                _context.Questions.Find(questionId).Text = questionTextTextBox.Text;
                _context.Questions.Find(questionId).RightAnswer = int.Parse(rigthAnswerTextBox.Text);
                _context.Questions.Find(questionId).Cost = int.Parse(costTextBox.Text);
                _context.Questions.Find(questionId).Answer1 = answer1TextBox.Text;
                _context.Questions.Find(questionId).Answer2 = answer2TextBox.Text;
                _context.Questions.Find(questionId).Answer3 = answer3TextBox.Text;
                _context.Questions.Find(questionId).Answer4 = answer4TextBox.Text;
                _context.Questions.Find(questionId).Answer5 = answer5TextBox.Text;
                _context.Questions.Find(questionId).IsWithImage = isWithImageCheckBox.IsChecked.Value;
                if (isWithImageCheckBox.IsChecked.Value)
                {
                    _context.Questions.Find(questionId).Image1Path = image1.Source.ToString();
                    _context.Questions.Find(questionId).Image2Path = image2.Source.ToString();
                    _context.Questions.Find(questionId).Image3Path = image3.Source.ToString();
                    _context.Questions.Find(questionId).Image4Path = image4.Source.ToString();
                    _context.Questions.Find(questionId).Image5Path = image5.Source.ToString();
                }
            }
            catch
            {

            }

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // clean up database connections
            _context.Dispose();
            base.OnClosing(e);

            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
        }

        private void OnQuestionImageLoaded(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog
            {
                Title = "Выберите изображение",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png"
            };
            if (op.ShowDialog() == true)
            {
                string imageFileName = GetCopiedFileName(@"images", op.FileName);
                if (!imageFileName.IsNullOrEmpty())
                {
                    Image image = (Image)e.Source;
                    image.Source = new BitmapImage(new Uri(@System.IO.Path.Combine(Environment.CurrentDirectory, @imageFileName)));
                }
            }
        }


        private void ButtonSetPathToTutorialClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Theme currentTheme = GetTheme(sender);

                if (currentTheme.ThemeId == 0)
                {
                    ButtonSaveClick(null, null);
                }

                if (!(currentTheme is null))
                {


                    string fileName = GetFileName();
                    if (!fileName.IsNullOrEmpty())
                    {
                        fileName = GetCopiedFileName(@"tutorials", fileName);
                        int themeId = currentTheme.ThemeId;
                        _context.Themes.Find(themeId).TutorialPath = fileName;
                        themesDataGrid.Items.Refresh();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Сначала введите тему, к которой добавить обучение, и сохраните изменения");
            }
        }

        private Theme GetTheme(object sender)
        {
            var menuItem = (MenuItem)sender;
            var contextMenu = (ContextMenu)menuItem.Parent;
            var item = (DataGrid)contextMenu.PlacementTarget;
            return (Theme)item.SelectedCells[0].Item;
        }

        private string GetCopiedFileName(string newDir, string fileName)
        {
            Directory.CreateDirectory(newDir);
            string curFile = System.IO.Path.GetFileName(fileName);
            string newPathToFile = System.IO.Path.Combine(newDir, curFile);
            try
            {
                File.Copy(fileName, newPathToFile);
            }
            catch
            {
                //todo remove
                MessageBox.Show("Файл уже находится в директории");
            }
            return newPathToFile;
        }

        private string GetFileName()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Выберите обучающий файл";
            fileDialog.InitialDirectory = "";
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == true)
            {
                return fileDialog.FileName;
            }
            return "";
        }

        private void ButtonOpenTutorialClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Theme currentTheme = GetTheme(sender);
                new Process
                {
                    StartInfo = new ProcessStartInfo(currentTheme.TutorialPath)
                    {
                        UseShellExecute = true
                    }
                }.Start();
            }
            catch
            {
                MessageBox.Show("Нельзя открыть файл");
            }
        }

        private void SetTextAnswers(object sender, RoutedEventArgs e)
        {
            imageAnswers.Visibility = Visibility.Collapsed;
            textAnswers.Visibility = Visibility.Visible;

        }

        private void SetImageAnswers(object sender, RoutedEventArgs e)
        {
            textAnswers.Visibility = Visibility.Collapsed;
            imageAnswers.Visibility = Visibility.Visible;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void OneDigitNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^1-5]+");
            e.Handled = !(!regex.IsMatch(e.Text) && ((TextBox)sender).Text.Length < 1);
        }

        private void OnQuestionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                questionDetails.Visibility = Visibility.Visible;
                Question question = (Question)questionsDataGrid.SelectedItem;
                _context.SaveChanges();
                questionTextTextBox.Text = question.Text;
                questionIdTextBox.Content = question.QuestionId;
                rigthAnswerTextBox.Text = (question.RightAnswer == 0) ? "1" : question.RightAnswer.ToString();
                costTextBox.Text = question.Cost.ToString();
                answer1TextBox.Text = question.Answer1;
                answer2TextBox.Text = question.Answer2;
                answer3TextBox.Text = question.Answer3;
                answer4TextBox.Text = question.Answer4;
                answer5TextBox.Text = question.Answer5;
                isWithImageCheckBox.IsChecked = question.IsWithImage;

                SetImage(image1, question.Image1Path);
                SetImage(image2, question.Image2Path);
                SetImage(image3, question.Image3Path);
                SetImage(image4, question.Image4Path);
                SetImage(image5, question.Image5Path);


            }
            catch
            {
                questionDetails.Visibility = Visibility.Collapsed;
            }

        }

        private void SetImage(Image image, string imagePath)
        {
            try
            {
                if (imagePath.IsNullOrEmpty())
                {
                    image.Source = new BitmapImage(new Uri(@"/images/150_output-onlinepngtools.png", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    image.Source = new BitmapImage(new Uri(@imagePath));
                }
            }

            catch
            {
                MessageBox.Show("Путь к изображению некорректен: " + @imagePath);
            }
        }

        private void OnEmployeeCreated(object sender, AddingNewItemEventArgs e)
        {
            e.NewItem = new Employee(DateTime.Now);
        }



        private void ButtonStartTestingClick(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Employee> employees = GetSelectedEmployeesList();
                List<Theme> themes = GetSelectedThemesList();

                if (employees.Count < 1 || themes.Count < 1)
                {
                    throw new ArgumentOutOfRangeException();
                }

                TestingWindow testingWindow = new TestingWindow(employees, themes)
                {
                    Owner = this
                };
                testingWindow.Show();

            }
            catch
            {
                MessageBox.Show("Выберите сотрудников и тесты");
            }
        }

        private List<Employee> GetSelectedEmployeesList()
        {
            var SelectedList = new List<Employee>();
            for (int i = 0; i < employeesDataGrid2.Items.Count; i++)
            {
                var item = employeesDataGrid2.Items[i];
                var mycheckbox = employeesDataGrid2.Columns[2].GetCellContent(item) as CheckBox;
                if ((bool)mycheckbox.IsChecked)
                {
                    var myTextBlock = employeesDataGrid2.Columns[0].GetCellContent(item) as TextBlock;
                    int index = int.Parse(myTextBlock.Text.ToString());
                    SelectedList.Add(_context.Employees.Find(index));
                }
            }

            return SelectedList;
        }

        private List<Theme> GetSelectedThemesList()
        {
            var SelectedList = new List<Theme>();
            for (int i = 0; i < themesDataGrid2.Items.Count; i++)
            {
                var item = themesDataGrid2.Items[i];
                var mycheckbox = themesDataGrid2.Columns[2].GetCellContent(item) as CheckBox;
                if ((bool)mycheckbox.IsChecked)
                {
                    var myTextBlock = themesDataGrid2.Columns[0].GetCellContent(item) as TextBlock;
                    int index = int.Parse(myTextBlock.Text.ToString());
                    SelectedList.Add(_context.Themes.Find(index));
                }
            }

            return SelectedList;
        }

        public void AddTestResultToEmployee(Employee employee, TestResult testResult)
        {
            _context.Employees.Find(employee.EmployeeId).TestResults.Add(testResult);
            _context.SaveChanges();
            RefreshGrids();
        }

    }
}