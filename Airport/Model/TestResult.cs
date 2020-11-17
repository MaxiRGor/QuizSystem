using System;

namespace Airport
{
    public class TestResult
    {
        public TestResult() { }


        public TestResult(int employeeId, Employee employee, int themeId, Theme theme, int result, DateTime datePass, bool tutorialWathed)
        {
            EmployeeId = employeeId;
            Employee = employee;
            ThemeId = themeId;
            Theme = theme;
            Result = result;
            DatePass = datePass;
            TutorialWathed = tutorialWathed;
        }

        public int TestResultId { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public int ThemeId { get; set; }
        public virtual Theme Theme { get; set; }

       public int Result { get; set; }
        public DateTime DatePass { get; set; }

        public bool TutorialWathed { get; set; }
    }
}