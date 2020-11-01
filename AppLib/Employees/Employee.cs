using CalcOfWagesLib.Data_Base;
using System;
using System.Collections.Generic;

namespace AppLib
{
    abstract class Employee
    {
        public string Name { get; protected set; }
        public Roles Role { get; protected set; }

        public decimal SalaryPerHour { get; protected set; }
        public decimal SalaryPerOvertimeHour { get; protected set; }
        public readonly byte WorkingHoursForDay = 8;

        public enum Roles
        {
            Leader,
            Salaried,
            Freelancer
        }

        public virtual void AddWorkedHours(DateTime date, byte workedHours, string comment) 
        {
            if (date > DateTime.Today) throw new Exception("Добавление времени за будущие дни.");

            HoursWorkedRecord record = new HoursWorkedRecord(date, Name, workedHours, comment);
            switch (Role)
            {
                case Roles.Leader:
                    record.Save("hours_worked_leaders.csv");
                    break;
                case Roles.Salaried:
                    record.Save("hours_worked_salaried.csv");
                    break;
                case Roles.Freelancer:
                    record.Save("hours_worked_freelancers.csv");
                    break;
            }
        }

        public string GetReport(DateTime from, DateTime till)
        {
            if (till > DateTime.Today) throw new Exception("Просмотр отчета за будущие дни.");

            List<HoursWorkedRecord> records = GetRecordsByRole();
            string report = "Отчет по сотруднику: " + Name + " за период за период с " + from.ToShortDateString() + " по " + from.ToShortDateString() + ": \n";
            int workedHours = 0;
            foreach (var record in records)
            {
                if (from <= DateTime.Parse(record.Date) && DateTime.Parse(record.Date) <= till)
                {
                    if (record.EmployeeName == Name)
                    {
                        report += record.Date + ", " + record.WorkedHours.ToString() + ", " + record.Comment + "\n";
                        workedHours += record.WorkedHours;
                    }
                }
            }

            // подсчет переработок
            int overtimeHours = CalcOvertimeHours(workedHours, (till - from).Days);
            workedHours -= overtimeHours;

            report += "Итого: " + (workedHours + overtimeHours).ToString() + " часов, заработано: " + (workedHours * SalaryPerHour + overtimeHours * SalaryPerOvertimeHour).ToString() + "\n";
            return report;
        }

        public string GetReportPerMonth()
        {
            DateTime till = DateTime.Today;
            DateTime from = till.AddMonths(-1);

            List<HoursWorkedRecord> records = GetRecordsByRole();
            
            string report = "Отчет по сотруднику: " + Name + " за период за период с " + from.ToShortDateString() + " по " + from.ToShortDateString() + ": \n";
            int workedHours = 0;
            foreach (var record in records)
            {
                if (from <= DateTime.Parse(record.Date) && DateTime.Parse(record.Date) <= till)
                {
                    if (record.EmployeeName == Name)
                    {
                        report += record.Date + ", " + record.WorkedHours.ToString() + ", " + record.Comment + "\n";
                        workedHours += record.WorkedHours;
                    }
                }
            }

            // подсчет переработок
            int overtimeHours = CalcOvertimeHours(workedHours, (till - from).Days);
            workedHours -= overtimeHours;
            
            decimal salary = (workedHours * SalaryPerHour + overtimeHours * SalaryPerOvertimeHour);
            if (Role == Roles.Leader && overtimeHours > 0)
            {
                salary += 20000;
            }

            report += "Итого: " + (workedHours + overtimeHours).ToString() + " часов, заработано: " + salary.ToString() + "\n";
            return report;
        }


        protected int CalcOvertimeHours(int workedHours, int period)
        {
            if (workedHours > period * WorkingHoursForDay)
            {
                return workedHours - (period * WorkingHoursForDay);
            }
            return 0;
        }

        protected List<HoursWorkedRecord> GetRecordsByRole()
        {
            List<HoursWorkedRecord> records = new List<HoursWorkedRecord>();
            switch (Role)
            {
                case Roles.Leader:
                    records = HoursWorkedRecord.ReadRecords("hours_worked_leaders.csv");
                    break;
                case Roles.Salaried:
                    records = HoursWorkedRecord.ReadRecords("hours_worked_salaried.csv");
                    break;
                case Roles.Freelancer:
                    records = HoursWorkedRecord.ReadRecords("hours_worked_freelancers.csv");
                    break;
            }

            return records;
        }
    }
}
