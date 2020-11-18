using System;
using System.Collections.Generic;
using System.Text;

namespace Airport.Model
{
    public class UserAnswer
    {
        public UserAnswer()
        {
        }

        public UserAnswer(string question, int userAnswer, int rightAnswer)
        {
            Question = question;
            UserAnswerInt = userAnswer;
            RightAnswerInt = rightAnswer;
        }

        public int UserAnswerId { get; set; }

        public int TestResultId { get; set; }

        public virtual TestResult TestResult { get; set; }

        public string Question { get; set; }

        public int UserAnswerInt { get; set; }

        public int RightAnswerInt { get; set; }
    }
}
