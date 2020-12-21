using System;
using System.Collections.Generic;
using System.Text;

namespace Airport.Model
{
    public class ExcelModelAnswerOfEmployee
    {
        public ExcelModelAnswerOfEmployee()
        {
        }

        public ExcelModelAnswerOfEmployee(string questionText, string rightAnswerTest, string userAnswerTest, bool isRight)
        {
            QuestionText = questionText;
            RightAnswerTest = rightAnswerTest;
            UserAnswerTest = userAnswerTest;
            IsRight = isRight ? "Верно" : "Неверно";
        }

        public string QuestionText { get; set; }
        public string RightAnswerTest { get; set; }
        public string UserAnswerTest { get; set; }
        public string IsRight { get; set; }
    }
}
