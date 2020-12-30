
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Airport
{
    public class Job
    {
        public Job()
        {
            ResultInFinalTestToPass = 80;
        }

        public int JobId { get; set; }
        public string Title { get; set; }

        public int AirportServiceId { get; set; }

        public virtual AirportService AirportService { get; set; }

        public virtual ICollection<Category> Categories { get; private set; } = new ObservableCollection<Category>();

        public virtual ICollection<Employee> Employees { get; private set; } = new ObservableCollection<Employee>();

        public int ResultInFinalTestToPass { get; set; }

    }
}