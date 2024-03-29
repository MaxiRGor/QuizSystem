﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Airport.Model
{
    public class UserAnswer
    {
        public UserAnswer()
        {
        }

        public UserAnswer(Question question, int questionId, string questionText, int userAnswer, int rightAnswer)
        {
            Question = question;
            QuestionId = questionId;
            QuestionText = questionText;
            UserAnswerInt = userAnswer;
            RightAnswerInt = rightAnswer;
            IsRight = RightAnswerInt == UserAnswerInt;
        }

        public int UserAnswerId { get; set; }

        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }

        public int TestResultId { get; set; }

        public virtual TestResult TestResult { get; set; }

        public string QuestionText { get; set; }

        public int UserAnswerInt { get; set; }

        public int RightAnswerInt { get; set; }

        public bool IsRight { get; set; }
    }
}
