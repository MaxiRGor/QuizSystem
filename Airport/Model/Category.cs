using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Airport
{
    public  class Category
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }

       // public int JobId { get; set; }
        //public virtual Job Job { get; set; }

        public virtual ICollection<Job> Jobs { get; private set; }

        public virtual ICollection<Theme> Themes { get; private set; } = new ObservableCollection<Theme>();
    }
}
