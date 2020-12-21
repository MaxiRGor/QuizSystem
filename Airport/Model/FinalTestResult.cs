using Airport.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Airport
{
    public class FinalTestResult
    {
        public FinalTestResult()
        {
        }

        public FinalTestResult(int employeeId, Employee employee, int result, DateTime datePass, bool isPassed, int wrongAnswerCount)
        {
            EmployeeId = employeeId;
            Employee = employee;
            Result = result;
            DatePass = datePass;
            IsPassed = isPassed;
            WrongAnswersCount = wrongAnswerCount;
        }

        public int FinalTestResultId { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public int Result { get; set; }
        public DateTime DatePass { get; set; }

        public bool IsPassed { get; set; }

        public virtual ICollection<FinalTestResultUserAnswer> Answers { get; private set; } = new ObservableCollection<FinalTestResultUserAnswer>();

        public int WrongAnswersCount { get; set; }

    }
}