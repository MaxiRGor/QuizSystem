using Airport.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Airport
{
    public class TestResult
    {
        public TestResult() { }

        public TestResult(int employeeId, Employee employee, int themeId, Theme theme, DateTime datePass, bool tutorialWathed, int wrongAnswersCount)
        {
            EmployeeId = employeeId;
            Employee = employee;
            ThemeId = themeId;
            Theme = theme;
            DatePass = datePass;
            TutorialWathed = tutorialWathed;
            WrongAnswersCount = wrongAnswersCount;
        }

        public TestResult(int employeeId, Employee employee, int themeId, Theme theme, int result, DateTime datePass, bool isPassed, int wrongAnswersCount)
        {
            EmployeeId = employeeId;
            Employee = employee;
            ThemeId = themeId;
            Theme = theme;
            Result = result;
            DatePass = datePass;
            IsPassed = isPassed;
            WrongAnswersCount = wrongAnswersCount;
        }

        public int TestResultId { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public int ThemeId { get; set; }
        public virtual Theme Theme { get; set; }

        public int Result { get; set; }

        public bool IsPassed { get; set; }
        public DateTime DatePass { get; set; }

        public virtual ICollection<UserAnswer> Answers { get; private set; } = new ObservableCollection<UserAnswer>();

        public bool TutorialWathed { get; set; }

        public int WrongAnswersCount { get; set; }
    }
}