using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Airport
{
    public class Theme
    {
        public Theme()
        {
            NumberOfQuestions = 10;
            ResultToPass = 80;
            NumberOfFinalQuestions = 0;
        }

        public Theme(string title)
        {
            Title = title;
            ResultToPass = 80;
        }

        public int ThemeId { get; set; }
        public string Title { get; set; }

        public int CategoryId { get; set; }
        
        public virtual Category Category { get; set; }
        public string TutorialPath { get; set; }

        public int NumberOfQuestions { get; set; }

        public int NumberOfFinalQuestions { get; set; }

        public int ResultToPass { get; set; }

        public virtual ICollection<Question> Questions { get; private set; } = new ObservableCollection<Question>();
    }
}