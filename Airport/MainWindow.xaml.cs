
using Airport.Model;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using ModernWpf.Controls;
using OfficeOpenXml;
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
        private bool _reopenAuthWindow = true;

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
            SetJobsCombobox();
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

        private void ButtonSaveClick(object sender, RoutedEventArgs e)
        {
            _context.SaveChanges();
            RefreshElements();

            if (questionDetails.Visibility == Visibility.Visible)
            {
                SaveQuestion();
            }
        }

        private void RefreshElements()
        {
            try
            {
                categoriesDataGrid.Items.Refresh();
                jobsDataGrid1.Items.Refresh();
                jobsDataGrid2.Items.Refresh();
                themesDataGrid.Items.Refresh();
                questionsDataGrid.Items.Refresh();
                employeesDataGrid.Items.Refresh();
                testResultsDataGrid.Items.Refresh();
                SetJobsCombobox();
            }
            catch
            {

            }

        }

        private void SetJobsCombobox()
        {
            AddJobToCategoryMenu.Items.Clear();
            Job[] jobs = _context.Jobs.ToArray();
            foreach (Job job in jobs)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Header = job.Title;
                menuItem.Tag = job.JobId;
                menuItem.Click += new RoutedEventHandler(OnAddingCategoryToNewJob);
                AddJobToCategoryMenu.Items.Add(menuItem);
            }
        }

        public void OnAddingCategoryToNewJob(object sender, RoutedEventArgs e)
        {
            Category category = GetSelectedCategory();
            Job job = GetSelectedJob();
            if (category != null)
            {
                int.TryParse(((MenuItem)sender).Tag.ToString(), out int nextJobId);
                if (job != null && nextJobId != 0)
                {
                    if (job.JobId == nextJobId)
                    {
                        message.Content = "Выберите должность, которая\n отличается от уже выбранной";
                        DialogHost.IsOpen = true;
                    }
                    else
                    {
                        if (_context.Jobs.Find(nextJobId).Categories.Contains(category)){
                            message.Content = "Выберите должность уже содержит выбранную категорию";
                            DialogHost.IsOpen = true;
                        }
                        else
                        {
                            _context.Jobs.Find(nextJobId).Categories.Add(category);
                            _context.SaveChanges();
                            RefreshElements();
                        }

                    }
                }
            }
        }

        private Category GetSelectedCategory()
        {
            try
            {
                return (Category)categoriesDataGrid.SelectedCells[0].Item;

            }
            catch
            {
                return null;
            }
        }

        private Job GetSelectedJob()
        {
            try
            {
                return (Job)jobsDataGrid1.SelectedCells[0].Item;

            }
            catch
            {
                return null;
            }
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
                _context.Questions.Find(questionId).Answer6 = answer6TextBox.Text;
                _context.Questions.Find(questionId).IsWithAnswerImage = isAnswersWithImageCheckBox.IsChecked.Value;
                _context.Questions.Find(questionId).IsWithQuestionImage = isQuestionWithImageCheckBox.IsChecked.Value;
                if (isAnswersWithImageCheckBox.IsChecked.Value)
                {
                    _context.Questions.Find(questionId).Image1Path = image1.Source.ToString();
                    _context.Questions.Find(questionId).Image2Path = image2.Source.ToString();
                    _context.Questions.Find(questionId).Image3Path = image3.Source.ToString();
                    _context.Questions.Find(questionId).Image4Path = image4.Source.ToString();
                    _context.Questions.Find(questionId).Image5Path = image5.Source.ToString();
                    _context.Questions.Find(questionId).Image6Path = image6.Source.ToString();
                }
                if (isQuestionWithImageCheckBox.IsChecked.Value)
                {
                    _context.Questions.Find(questionId).QuestionImagePath = questionImage.Source.ToString();
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
            if (_reopenAuthWindow)
            {
                AuthorizationWindow authorizationWindow = new AuthorizationWindow();
                authorizationWindow.Show();
            }
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
                Theme currentTheme = (Theme)DataGridHelper.GetSelectedObject(sender);

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
                try
                {

                    message.Content = "Сначала введите тему, к которой добавить обучение,\n и сохраните изменения";
                    DialogHost.IsOpen = true;
                }
                catch
                {

                }
            }
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
                Theme currentTheme = (Theme)DataGridHelper.GetSelectedObject(sender);
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
                try
                {
                    message.Content = "Нельзя открыть файл";
                    DialogHost.IsOpen = true;
                }
                catch
                {

                }

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


        private void DisableQuestionImage(object sender, RoutedEventArgs e)
        {
            questionImagePanel.Visibility = Visibility.Collapsed;

        }

        private void EnableQuestionImage(object sender, RoutedEventArgs e)
        {
            questionImagePanel.Visibility = Visibility.Visible;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void OneDigitNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^1-6]+");
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
                answer6TextBox.Text = question.Answer6;
                isAnswersWithImageCheckBox.IsChecked = question.IsWithAnswerImage;
                isQuestionWithImageCheckBox.IsChecked = question.IsWithQuestionImage;

                SetImage(image1, question.Image1Path);
                SetImage(image2, question.Image2Path);
                SetImage(image3, question.Image3Path);
                SetImage(image4, question.Image4Path);
                SetImage(image5, question.Image5Path);
                SetImage(image6, question.Image6Path);
                SetImage(questionImage, question.QuestionImagePath);

            }
            catch
            {
                questionDetails.Visibility = Visibility.Collapsed;
                questionImagePanel.Visibility = Visibility.Collapsed;
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
                message.Content = "Путь к изображению некорректен: " + @imagePath;
                DialogHost.IsOpen = true;
            }
        }


        public void ShowDeleteRowDialog(object sender, KeyEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            if (dg != null)
            {
                if (e.Key == Key.Delete)
                {
                    var result = MessageBox.Show(
                        "Вы хотите удалить данные. Это действие необратимо.\n\nПродолжить?",
                        "Удаление",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question,
                        MessageBoxResult.No);
                    e.Handled = (result == MessageBoxResult.No);
                }
            }
        }

        private void OpenSelectTestWindowButtonClick(object sender, RoutedEventArgs e)
        {
            _context.SaveChanges();
            TestSelectionWindow testSelectionWindow = new TestSelectionWindow(_currentService);
            testSelectionWindow.Show();
            _reopenAuthWindow = false;
            Close();
        }

        private void ShowTestResultsClick(object sender, RoutedEventArgs e)
        {
            WindowCreator.ShowTestResultsOfCurrentThemeViaMenuClickOnThemesDataDrid(sender, _context, this);
        }

        private void ShowFinalTestResultsClick(object sender, RoutedEventArgs e)
        {
            WindowCreator.ShowFinalTestResultsViaMenuClickOnJobsGrid(sender, _context, this);
        }


        private void CreateExcelFileOfEmployeeTestResultsButtonClick(object sender, RoutedEventArgs e)
        {
            ExcelBuilder.CreateExcelFileOfEmployeeTestResultsViaMenuClickOnEmployeeGrid(sender);
        }

        private void ShowEmployeeAnswersButtonClick(object sender, RoutedEventArgs e)
        {
            WindowCreator.ShowAnswersOfSelectedTestViaMenuClickOnTestResultsDataGrid(sender, _context, false, this);
        }

        private void DeleteCategoryFromCurrentJob(object sender, RoutedEventArgs e)
        {
            Category category = GetSelectedCategory();
            Job job = GetSelectedJob();
            if (category != null && job != null)
            {
                _context.Jobs.Find(job.JobId).Categories.Remove(category);
                _context.SaveChanges();
                RefreshElements();
            }
        }
    }
}
