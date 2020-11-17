using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Airport
{
    public class AirportService
    {
        public AirportService()
        {
        }

        public AirportService(string title, string password)
        {
            Title = title;
            Password = password;
        }

        public int AirportServiceId { get; set; }
        public string Title { get; set; }
        public string Password { get; set; }


        public virtual ICollection<Job> Jobs { get; private set; } = new ObservableCollection<Job>();
    }
}
