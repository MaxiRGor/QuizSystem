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
            Title = "Новая служба";
            PasswordForRedacting = "сложный_пароль_для_входа_в_режим_редактирования";
            PasswordForTesting = "простой_пароль_для_входа_в_режим_тестирования";
        }

        public AirportService(string title, string password)
        {
            Title = title;
            PasswordForTesting = PasswordForRedacting = password;
        }

        public int AirportServiceId { get; set; }
        public string Title { get; set; }
        public string PasswordForRedacting { get; set; }
        public string PasswordForTesting { get; set; }
        public virtual ICollection<Job> Jobs { get; private set; } = new ObservableCollection<Job>();
    }
}
