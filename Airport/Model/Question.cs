namespace Airport
{
    public class Question
    {
        public Question()
        {
            RightAnswer = 1;
            Cost = 1;
        }

        public int QuestionId { get; set; }
        public string Text { get; set; }

        public int ThemeId { get; set; }
        public virtual Theme Theme { get; set; }

        public string QuestionImagePath { get; set; }
        public bool IsWithQuestionImage { get; set; }
        public bool IsWithAnswerImage { get; set; }

        public string Answer1 { get; set; }

        public string Answer2 { get; set; }

        public string Answer3 { get; set; }

        public string Answer4 { get; set; }

        public string Answer5 { get; set; }
        public string Answer6 { get; set; }

        public string Image1Path { get; set; }
        public string Image2Path { get; set; }

        public string Image3Path { get; set; }

        public string Image4Path { get; set; }

        public string Image5Path { get; set; }
        public string Image6Path { get; set; }


        public int RightAnswer { get; set; }

        public int Cost { get; set; }

    }
}