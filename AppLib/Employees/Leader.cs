using CalcOfWagesLib.Data_Base;
using System;
using System.Collections.Generic;

namespace AppLib
{
    class Leader : Employee
    {
        public Leader(string name)
        {
            Name = name;
            Role = Roles.Leader;
            SalaryPerHour = 1250;
            SalaryPerOvertimeHour = SalaryPerHour;
        }

        public void AddWorkedHoursForEmployee(DateTime date, byte workedHours, string comment, Employee employee)
        {
            HoursWorkedRecord record = new HoursWorkedRecord(date, employee.Name, workedHours, comment);
            switch (employee.Role)
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

        public string GetReportForAllEmployees(DateTime from, DateTime till, Employee employee)
        {
            if (till > DateTime.Today) throw new Exception("Просмотр отчета за будущие дни.");

            List<HoursWorkedRecord> records = HoursWorkedRecord.ReadRecords("hours_worked_leaders.csv");
            records.AddRange(HoursWorkedRecord.ReadRecords("hours_worked_salaried.csv"));
            records.AddRange(HoursWorkedRecord.ReadRecords("hours_worked_freelancers.csv"));

            string report = "Отчет за период с " + from.ToShortDateString() + " по " + till.ToShortDateString() + ": \n";
            int allWorkedHours = 0;
            decimal summToPay = 0;

            foreach (var record in records)
            {
                if (from <= DateTime.Parse(record.Date) && DateTime.Parse(record.Date) <= till)
                {
                    int workedHours = record.WorkedHours;
                    int overtimeHours = 0;

                    if (workedHours > 8)
                    {
                        overtimeHours = workedHours - 8;
                        workedHours = 8;
                    }

                    decimal salary = (workedHours * SalaryPerHour + overtimeHours * SalaryPerOvertimeHour);
                    report += record.EmployeeName + " отработал " + record.WorkedHours.ToString() + " и заработал " + salary.ToString() +"\n";

                    allWorkedHours += record.WorkedHours;
                    summToPay += salary;
                }
            }

            report += "Всего часов отработано за период " + allWorkedHours.ToString() + ", сумма к выплате " + summToPay.ToString() + "руб.\n";

            return report;
        }

        public string GetReportForEmployee(DateTime from, DateTime till, Employee employee)
        {
            return employee.GetReport(from, till);
        }

        public string GetReportPerMonthForEmployee(Employee employee)
        {
            return employee.GetReportPerMonth();
        }

        public void AddEmployee(Employee employee)
        {
            EmployeesRecord record = new EmployeesRecord(employee);
            record.Save();
        }
    }
}
