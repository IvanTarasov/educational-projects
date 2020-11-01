using AppLib;
using System;

namespace CalcOfWagesLib.Employees
{
    class Freelancer : Employee
    {
        public Freelancer(string name)
        {
            Name = name;
            Role = Roles.Freelancer;
            SalaryPerHour = 1000;
            SalaryPerOvertimeHour = SalaryPerHour;
        }

        public override void AddWorkedHours(DateTime date, byte workedHours, string comment)
        {
            if (date < DateTime.Today.AddDays(-2)) throw new Exception("Добавление времени ранее чем за два дня от текущего времени.");
            base.AddWorkedHours(date, workedHours, comment);
        }
    }
}
