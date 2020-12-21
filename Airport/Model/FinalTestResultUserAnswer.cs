using System;
using System.Collections.Generic;
using System.Text;

namespace Airport.Model
{
    public class FinalTestResultUserAnswer
    {
        public FinalTestResultUserAnswer()
        {
        }

        public FinalTestResultUserAnswer(Question question, int questionId, string questionText, int userAnswer, int rightAnswer)
        {
            Question = question;
            QuestionId = questionId;
            QuestionText = questionText;
            UserAnswerInt = userAnswer;
            RightAnswerInt = rightAnswer;
        }

        public int FinalTestResultUserAnswerId { get; set; }

        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }

        public int FinalTestResultId { get; set; }

        public virtual FinalTestResult FinalTestResult { get; set; }

        public string QuestionText { get; set; }

        public int UserAnswerInt { get; set; }

        public int RightAnswerInt { get; set; }

    }
}
