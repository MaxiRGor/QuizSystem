namespace Airport
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }

        public int ThemeId { get; set; }
        public virtual Theme Theme { get; set; }

        public bool IsWithImage { get; set; }

        public string Answer1 { get; set; }

        public string Answer2 { get; set; }

        public string Answer3 { get; set; }

        public string Answer4 { get; set; }

        public string Answer5 { get; set; }

        public string Image1Path { get; set; }
        public string Image2Path { get; set; }

        public string Image3Path { get; set; }

        public string Image4Path { get; set; }

        public string Image5Path { get; set; }


        public int RightAnswer { get; set; }

        public int Cost { get; set; }

    }
}