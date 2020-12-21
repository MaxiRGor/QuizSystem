using Airport.Model;
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
        private int _right_answers;
        private int _points;
        private List<UserAnswer> _userAnswers;


        public DistinctTestWindow(Employee employee, Theme theme, List<Question> questions)
        {
            InitializeComponent();
            _questionIndex = 0;
            _employee = employee;
            _questions = questions;
            _theme = theme;
            _right_answers = 0;
            _points = 0;
            _userAnswers = new List<UserAnswer>();

            title.Text = _theme != null ? _theme.Title : "Итоговый тест";

            SetView();
        }

        private void SetView()
        {
            SetProgress();
            Question current_question = _questions[_questionIndex];
            questionTextTextBox.Text = current_question.Text;
            if (current_question.IsWithAnswerImage)
            {
                imageAnswers.Visibility = Visibility.Visible;
                textAnswers.Visibility = Visibility.Collapsed;
                SetImage(image1, current_question.Image1Path);
                SetImage(image2, current_question.Image2Path);
                SetImage(image3, current_question.Image3Path);
                SetImage(image4, current_question.Image4Path);
                SetImage(image5, current_question.Image5Path);
                if (current_question.Image6Path.Length != 0)
                {
                    image6Panel.Visibility = Visibility.Visible;
                    SetImage(image6, current_question.Image6Path);
                }
                else
                {
                    image6Panel.Visibility = Visibility.Collapsed;
                }

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
                if (current_question.Answer6 != null && current_question.Answer6.Length != 0)
                {
                    answer6TextBox.Visibility = Visibility.Visible;
                    answer6TextBox.Text = current_question.Answer6;
                }
                else
                {
                    answer6TextBox.Visibility = Visibility.Collapsed;
                }
            }

            if (current_question.IsWithQuestionImage)
            {
                questionImagePanel.Visibility = Visibility.Visible;
                SetImage(questionImage, current_question.QuestionImagePath);
            }
            else
            {
                questionImagePanel.Visibility = Visibility.Collapsed;
            }

            answerTextBox.Text = "";
        }


        private void CreateTestResult()
        {
            int max_points = 0;
            foreach (var q in _questions)
            {
                max_points += q.Cost;
            }

            int result = max_points == 0 ? (_right_answers * 100 / _questions.Count) : (_points * 100 / max_points);

            if (_theme != null)
            {
                string verdict = (result >= _theme.ResultToPass) ? "" : "НЕ ";

                message.Content = "Вы, " + _employee.Name + ",\n" +
                       verdict +
                        "прошли тест " + _theme.Title + "\n" +
                        "в категории " + _theme.Category.Title + "\n" +
                        "с результатом " + result + " из 100.\n" +
                        "Правильных ответов " + _right_answers + " из " + _questions.Count + ".";
                DialogHost.IsOpen = true;

                TestResult testResult = new TestResult(_employee.EmployeeId, _employee, _theme.ThemeId, _theme, result, DateTime.Now, result >= _theme.ResultToPass, _questions.Count - _right_answers);
                ((TestSelectionWindow)Owner).AddTestResultToEmployee(_employee, testResult, _userAnswers);
            }
            else
            {
                string verdict = (result >= _employee.Job.ResultInFinalTestToPass) ? "" : "НЕ ";

                message.Content = "Вы, " + _employee.Name + ",\n" +
                     verdict +
                    "прошли ИТОГОВЫЙ тест " + "\n" +
                    "с результатом " + result + " из 100.\n" +
                    "Правильных ответов " + _right_answers + " из " + _questions.Count + ".";
                DialogHost.IsOpen = true;

                FinalTestResult finalTestResult = new FinalTestResult(_employee.EmployeeId, _employee, result, DateTime.Now, result >= _employee.Job.ResultInFinalTestToPass, _questions.Count - _right_answers);
                ((TestSelectionWindow)Owner).AddFinalTestResultToEmployee(_employee, finalTestResult, _userAnswers);
            }
        }

        private void SetProgress()
        {
            progressBar.Value = (_questionIndex * 100 / _questions.Count);
            progressBarPercentage.Text = _questionIndex.ToString() + " / " + _questions.Count.ToString();
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
            }
        }

        private void OneDigitNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^1-6]+");
            e.Handled = !(!regex.IsMatch(e.Text) && ((TextBox)sender).Text.Length < 1);
        }

        private void OnQuestionImageTapped(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)e.Source;
            answerTextBox.Text = int.Parse(image.Tag.ToString()).ToString();
        }

        private void CheckAnswer(object sender, RoutedEventArgs e)
        {
            if (!answerTextBox.Text.IsNullOrEmpty())
            {
                Question current_question = _questions[_questionIndex];

                int userAnswer = int.Parse(answerTextBox.Text.ToString());
                if (current_question.RightAnswer == userAnswer)
                {
                    _points += current_question.Cost;
                    _right_answers++;
                }

                _userAnswers.Add(new UserAnswer(current_question, current_question.QuestionId, " Вопрос # " + current_question.QuestionId + ". " + current_question.Text, userAnswer, current_question.RightAnswer));

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
        }

        private void OnTextBoxTapped(object sender, MouseButtonEventArgs e)
        {
            TextBox image = (TextBox)e.Source;
            answerTextBox.Text = int.Parse(image.Tag.ToString()).ToString();
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
