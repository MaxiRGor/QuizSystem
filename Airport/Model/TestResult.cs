using Airport.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Airport
{
    public class TestResult
    {
        public TestResult() { }

        public TestResult(int employeeId, Employee employee, int themeId, Theme theme, DateTime datePass, bool tutorialWathed)
        {
            EmployeeId = employeeId;
            Employee = employee;
            ThemeId = themeId;
            Theme = theme;
            DatePass = datePass;
            TutorialWathed = tutorialWathed;
        }

        public TestResult(int employeeId, Employee employee, int themeId, Theme theme, int result, DateTime datePass, bool isPassed)
        {
            EmployeeId = employeeId;
            Employee = employee;
            ThemeId = themeId;
            Theme = theme;
            Result = result;
            DatePass = datePass;
            IsPassed = isPassed;
        }

        public int TestResultId { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public int ThemeId { get; set; }
        public virtual Theme Theme { get; set; }

        public int Result { get; set; }

        public bool IsPassed { get; set; }
        public DateTime DatePass { get; set; }

        public virtual ICollection<UserAnswer> WrongAnswers { get; private set; } = new ObservableCollection<UserAnswer>();

        public bool TutorialWathed { get; set; }
    }
}