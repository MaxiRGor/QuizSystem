
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Airport
{
    public class Employee
    {
        public Employee() { }
        public Employee(DateTime now)
        {
            RegistrationDate = now;
        }

        public int EmployeeId { get; set; }

        public int JobId { get; set; }
        public virtual Job Job { get; set; }

        public string Name { get; set; }

        public DateTime RegistrationDate { get; set; }

        public virtual ICollection<TestResult> TestResults { get; private set; } = new ObservableCollection<TestResult>();
    }
}