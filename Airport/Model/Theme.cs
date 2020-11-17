using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Airport
{
    public class Theme
    {
        public int ThemeId { get; set; }
        public string Title { get; set; }

        public int CategoryId { get; set; }
        
        public virtual Category Category { get; set; }
        public string TutorialPath { get; set; }

        public virtual ICollection<Question> Questions { get; private set; } = new ObservableCollection<Question>();
    }
}