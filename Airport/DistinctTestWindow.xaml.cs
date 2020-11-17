using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for DistinctTestWindow.xaml
    /// </summary>
    public partial class DistinctTestWindow : Window
    {
        private int _questionIndex;
        private Employee _employee;
        private Theme _theme;
        private List<Question> _questions;
        private bool _tutorialWathed;
        private int _right_answers;
        private int _points;

        public DistinctTestWindow()
        {
            InitializeComponent();
        }

        public DistinctTestWindow(Employee employee, Theme theme, List<Question> questions, bool tutorialWathed)
        {
            InitializeComponent();
            _questionIndex = 0;
            _employee = employee;
            _questions = questions;
            _theme = theme;
            _tutorialWathed = tutorialWathed;
            _right_answers = 0;
            _points = 0;

            if (_questions.Count == 0)
            {
                MessageBox.Show("Количество вопросов в данном тесте равно нулю!");
            }
            else
            {
                SetView();
            }
        }

        private void SetView()
        {
            SetProgress();
            Question current_question = _questions[_questionIndex];
            questionTextTextBox.Text = current_question.Text;
            if (current_question.IsWithImage)
            {
                imageAnswers.Visibility = Visibility.Visible;
                textAnswers.Visibility = Visibility.Collapsed;
                SetImage(image1, current_question.Image1Path);
                SetImage(image2, current_question.Image2Path);
                SetImage(image3, current_question.Image3Path);
                SetImage(image4, current_question.Image4Path);
                SetImage(image5, current_question.Image5Path);
            }
            else
            {
                imageAnswers.Visibility = Visibility.Collapsed;
                textAnswers.Visibility = Visibility.Visible;
                answer1TextBox.Text = current_question.Answer1;
                answer2TextBox.Text = current_question.Answer2;
                answer3TextBox.Text = current_question.Answer3;
                answer4TextBox.Text = current_question.Answer4;
                answer5TextBox.Text = current_question.Answer5;
            }
        }


        private void CreateTestResult()
        {
            int max_points = 0;
            foreach (var q in _questions)
            {
                max_points += q.Cost;
            }

            int result = max_points==0?(_right_answers*100/_questions.Count):(_points * 100 / max_points) ;

            MessageBox.Show("Вы, " + _employee.Name + ",\n" +
                "прошли тест " + _theme.Title + "\n" +
                "в категории " + _theme.Category.Title + "\n" +
                "с результатом " + result + " из 100.\n" +
                "Правильных ответов " + _right_answers + " из " + _questions.Count + ".");

            TestResult testResult = new TestResult(_employee.EmployeeId, _employee, _theme.ThemeId, _theme, result, DateTime.Now, _tutorialWathed);

            ((MainWindow)((TestingWindow)Owner).Owner).AddTestResultToEmployee(_employee, testResult);

            this.Close();

        }

        private void SetProgress()
        {
            progressBar.Value = (_questionIndex * 100 / _questions.Count) ;
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

        private void OneDigitNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^1-5]+");
            e.Handled = !(!regex.IsMatch(e.Text) && ((TextBox)sender).Text.Length < 1);
        }

        private void OnQuestionImageTapped(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)e.Source;
            answerTextBox.Text = int.Parse(image.Tag.ToString()).ToString();
        }

        private void CheckAnswer(object sender, RoutedEventArgs e)
        {
            Question current_question = _questions[_questionIndex];
            if (!answerTextBox.Text.IsNullOrEmpty())
            {
                int userAnswer = int.Parse(answerTextBox.Text.ToString());
                if (current_question.RightAnswer == userAnswer)
                {
                    _points += current_question.Cost;
                    _right_answers++;
                }
            }
            if (_questionIndex + 1 < _questions.Count)
            {
                _questionIndex++;
                SetView();
            }
            else
            {
                CreateTestResult();
            }
        }

        private void OnTextBoxTapped(object sender, MouseButtonEventArgs e)
        {
            TextBox image = (TextBox)e.Source;
            answerTextBox.Text = int.Parse(image.Tag.ToString()).ToString();
        }
    }
}
