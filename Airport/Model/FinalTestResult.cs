using System;

namespace Airport
{
    public class FinalTestResult
    {
        public FinalTestResult()
        {
        }

        public FinalTestResult(int employeeId, Employee employee, int result, DateTime datePass, bool isPassed)
        {
            EmployeeId = employeeId;
            Employee = employee;
            Result = result;
            DatePass = datePass;
            IsPassed = isPassed;
        }

        public int FinalTestResultId { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public int Result { get; set; }
        public DateTime DatePass { get; set; }
        public bool IsPassed { get; set; }

    }
}